import 'dart:async';
import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:flutter_webrtc/flutter_webrtc.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/User.dart';
import 'package:tinder_clone/Models/UserConnection.dart';
import 'package:tinder_clone/Services/SignalRService.dart';

class CallVideoScreen extends StatefulWidget {
  final BuildContext context;
  final UsersSimilar myFriend;
  final CallType callType;
  final bool isAccept;
  CallVideoScreen(
      {@required this.myFriend,
      @required this.isAccept,
      @required this.context,
      @required this.callType});
  @override
  _CallVideoScreenState createState() => _CallVideoScreenState();
}

class _CallVideoScreenState extends State<CallVideoScreen> {
  RTCVideoRenderer _localRenderer = RTCVideoRenderer();
  RTCVideoRenderer _remoteRenderer = RTCVideoRenderer();

  MediaStream localStream;

  IUser partner;
  RTCPeerConnection rtcConnection;
  AuthenticationProvider authen;
  bool getData = false;
  final TextEditingController textEditingController = TextEditingController();

  final mediaConstraints = <String, dynamic>{
    'audio': true,
    'video': {
      'mandatory': {
        'minWidth': '600', // Provide your own width, height and frame rate here
        'minHeight': '600',
        'minFrameRate': '15',
      },
      'facingMode': 'user',
      'optional': [],
    }
  };
  Signal signal;

  @override
  void dispose() {
    super.dispose();
    _localRenderer.dispose();
    _remoteRenderer.dispose();
  }

  @override
  initState() {
    super.initState();
    _timer = Timer.periodic(Duration(seconds: 1), (timer) {
      this._secondCount += 1;
    });
    this.authen = Provider.of<AuthenticationProvider>(widget.context);
    initRenderers();
    getStream(authen);
  }

  initRenderers() async {
    await _localRenderer.initialize();
    await _remoteRenderer.initialize();
  }

  Future onStartACall(AuthenticationProvider authen) async {
    print("onStartACall");
    await this.initPeerConnection();
    await this.requestMediaDevices();
    this.startLocalVideo();
    this.partner = await this.authen.signalR.getUserById(widget.myFriend.id);

    if (this.partner == null) {
      print('Người dùng hiện không online!');
      return;
    }

    this.authen.signalR.callRequest(this.partner.userId, widget.callType);
  }

  Future onAcceptACall(AuthenticationProvider authen) async {
    this.partner = await authen.signalR.getUserById(widget.myFriend.id);

    if (this.partner == null) {
      //Thông báo người dùng ko online;
      print("NOT ONLINE");
      return;
    }

    await this.authen.signalR.callAccept(widget.myFriend.id);
    await this.call();
  }

  Future call() async {
    await this.initPeerConnection();
    await this.requestMediaDevices();
    this.startLocalVideo();
    // Add the tracks from the local stream to the RTCPeerConnection
    this.localStream.getTracks().forEach(
        (track) => this.rtcConnection.addTrack(track, this.localStream));

    try {
      var offerOptions = {
        'offerToReceiveAudio': true,
        'offerToReceiveVideo':
            widget.callType == CallType.videoCall ? true : false
      };
      var offer = await this.rtcConnection.createOffer(offerOptions);
      // Establish the offer as the local peer's current description.

      await this.rtcConnection.setLocalDescription(offer);
      // this.dataService.sendMessage({ type: 'offer', data: offer });
      var json = {"sdp": offer.sdp, "type": offer.type};
      this.sendSignal(new Signal(SignalType.offer, json));
    } catch (err) {
      this.handleGetUserMediaError(err);
    }
  }

  Future requestMediaDevices() async {
    try {
      var mediaRequire;

      if (widget.callType == CallType.videoCall) {
        mediaRequire = <String, dynamic>{
          'audio': true,
          'video': {
            'mandatory': {
              'minWidth':
                  '600', // Provide your own width, height and frame rate here
              'minHeight': '600',
              'minFrameRate': '15',
            },
            'facingMode': 'user',
            'optional': [],
          }
        };
      } else {
        mediaRequire = <String, dynamic>{'audio': true, 'video': false};
      }

      this.localStream =
          await navigator.mediaDevices.getUserMedia(mediaRequire);
      // pause all tracks
      this.pauseLocalVideo();
    } catch (e) {
      print(e);
    }
  }

  void startLocalVideo() {
    this.localStream.getTracks().forEach((track) {
      track.enabled = true;
    });
    setState(() {
      _localRenderer.srcObject = this.localStream;
    });
  }

  void pauseLocalVideo() {
    this.localStream.getTracks().forEach((track) {
      track.enabled = false;
    });
    // setState(() {
    //   _localRenderer.srcObject = localStream;
    // });
  }

  void getStream(AuthenticationProvider authen) {
    this.authen.signalR.signalSub.listen((value) {
      this.newSignal(value);
    });

    try {
      if (widget.isAccept) {
        this.onAcceptACall(authen);
        print("Accept a call");
      } else {
        this.onStartACall(authen);
        print("Start a call");
      }
    } catch (error) {
      print('get Stream error: ${error.toString()}');
    }
  }

  Map<String, dynamic> configuration = {
    "iceServers": [
      {"url": "stun:stun.l.google.com:19302", "username": "", "credential": ""},
    ],
    "sdpSemantics": "unified-plan"
  };
  final Map<String, dynamic> constraints = {
    "mandatory": {},
    "optional": [
      {"DtlsSrtpKeyAgreement": true},
    ],
  };

  Future initPeerConnection() async {
    this.rtcConnection = await createPeerConnection(configuration, constraints);

    this.rtcConnection.onIceCandidate = this.handleICECandidateEvent;
    this.rtcConnection.onIceConnectionState =
        this.handleICEConnectionStateChangeEvent;
    this.rtcConnection.onSignalingState = this.handleSignalingStateChangeEvent;
    this.rtcConnection.onTrack = this.handleTrackEvent;
  }

  /* ########################  EVENT HANDLER  ################################## */
  void handleICECandidateEvent(RTCIceCandidate event) {
    if (event.candidate != null) {
      //       final String? candidate;
      // final String? sdpMid;
      // final int? sdpMlineIndex;
      var json = {
        "candidate": event.candidate,
        "sdpMid": event.sdpMid,
        "sdpMlineIndex": event.sdpMlineIndex
      };
      sendSignal(Signal(SignalType.iceCandidate, json));
    }
  }

  void handleICEConnectionStateChangeEvent(RTCIceConnectionState event) {
    switch (rtcConnection.iceConnectionState) {
      case RTCIceConnectionState.RTCIceConnectionStateDisconnected:
      case RTCIceConnectionState.RTCIceConnectionStateClosed:
      case RTCIceConnectionState.RTCIceConnectionStateFailed:
        this.closeVideoCall();
        break;
      default:
        break;
    }
  }

  void handleSignalingStateChangeEvent(RTCSignalingState event) {
    switch (this.rtcConnection.signalingState) {
      case RTCSignalingState.RTCSignalingStateClosed:
        this.closeVideoCall();
        break;
      default:
        break;
    }
  }

  void handleTrackEvent(RTCTrackEvent event) {
    // this.remoteVideo.nativeElement.srcObject = event.streams[0];
    print('handleTrackEvent');
    setState(() {
      this._remoteRenderer.srcObject = event.streams[0];
    });
  }

  void sendSignal(Signal signal) {
    this.authen.signalR.sendSignal(signal, widget.myFriend.id);
  }

  Future closeVideoCall() async {
    if (rtcConnection != null) {
      this.rtcConnection.onTrack = null;
      this.rtcConnection.onIceCandidate = null;
      this.rtcConnection.onIceConnectionState = null;
      this.rtcConnection.onSignalingState = null;

      // Stop all transceivers on the connection
      var transceivers = await this.rtcConnection.getTransceivers();
      transceivers.forEach((element) {
        element.stop();
      });

      // Close the peer connection
      this.rtcConnection.close();
      this.rtcConnection = null;
    }
  }

  Future newSignal(Signal signal) async {
    // Map<String, dynamic> signal = jsonDecode(data);

    switch (signal.type) {
      case SignalType.offer:
        this.handleOfferMessage(signal.data);
        break;

      case SignalType.answer:
        this.handleAnswerMessage(signal.data);
        break;

      case SignalType.iceCandidate:
        this.handleICECandidateMessage(signal.data);
        break;

      case SignalType.hangUp:
        this.handleHangupMessage(signal);
        break;

      default:
        print('unknown message of type ' + signal.type.toString());
        break;
    }
  }

  /* ########################  MESSAGE HANDLER  ################################## */

  Future handleOfferMessage(dynamic msg) async {
    print('rtcConnection.signalingState');
    print(this.rtcConnection.signalingState.toString());
    if (this.rtcConnection != null) {
      await this.initPeerConnection();
    }

    if (this.localStream != null) {
      this.startLocalVideo();
    }

    try {
      Map<String, dynamic> des = msg;
      var description = new RTCSessionDescription(des['sdp'], des['type']);
      await this.rtcConnection.setRemoteDescription(description);

      // add media stream to local video
      setState(() {
        this._localRenderer.srcObject = this.localStream;
      });

      // add media tracks to remote connection
      this.localStream.getTracks().forEach(
          (track) => this.rtcConnection.addTrack(track, this.localStream));

      var answer = await this.rtcConnection.createAnswer();

      await this.rtcConnection.setLocalDescription(answer);

      var localDescription = await rtcConnection.getLocalDescription();

      var json = {"sdp": localDescription.sdp, "type": localDescription.type};

      sendSignal(Signal(SignalType.answer, json));
      // this.sendSignal({ type: SignalType.answer, data: this.rtcConnection.localDescription });
    } catch (e) {
      print('ERRRRRRRRRRRRRRRRRRRRRRRR');
      print(e);
    }
  }

  void handleAnswerMessage(dynamic data) {
    Map<String, dynamic> des = data;
    var d = new RTCSessionDescription(des['sdp'], des['type']);
    print('setRemoteDescription');
    this.rtcConnection.setRemoteDescription(d);
  }

  void handleHangupMessage(Signal msg) {
    this.closeVideoCall();
  }

  void handleICECandidateMessage(dynamic candidate) {
    print('addCandidate:');
    Map<String, dynamic> candi = candidate;
    RTCIceCandidate c = RTCIceCandidate(
        candi['candidate'], candi['sdpMid'], candi['sdpMLineIndex']);
    // RTCIceCandidate(this.candidate, this.sdpMid, this.sdpMlineIndex);

    this.rtcConnection.addCandidate(c);
  }

  /* ########################  ERROR HANDLER  ################################## */
  void handleGetUserMediaError(dynamic e) {
    print("ERRRRRRRRRRRRRRRRRRR222222222222222222");
    switch (e.name) {
      case 'NotFoundError':
        print(
            'Unable to open your call because no camera and/or microphone were found.');
        break;
      case 'SecurityError':
      case 'PermissionDeniedError':
        // Do nothing; this is the same as the user canceling the call.
        break;
      default:
        print(e);
        print('Error opening your camera and/or microphone: ' + e.message);
        break;
    }

    this.closeVideoCall();
  }

  Timer _timer;
  int _secondCount = 0;
  String constructTime(int seconds) {
    int hour = seconds ~/ 3600;
    int minute = seconds % 3600 ~/ 60;
    int second = seconds % 60;
    return formatTime(hour) +
        ":" +
        formatTime(minute) +
        ":" +
        formatTime(second);
  }

  String formatTime(int timeNum) {
    return timeNum < 10 ? "0" + timeNum.toString() : timeNum.toString();
  }

  bool switchCmr = false;
  bool micOff = false;

  @override
  Widget build(BuildContext context) {
    if (authen.getUserInfo != null) {
      return Scaffold(
        appBar: AppBar(
          title: Text(widget.myFriend.fullName),
          actions: <Widget>[
            IconButton(
              icon: const Icon(Icons.settings),
              onPressed: null,
              tooltip: 'setup',
            ),
          ],
        ),
        floatingActionButtonLocation: FloatingActionButtonLocation.centerFloat,
        floatingActionButton: SizedBox(
            width: widget.callType == CallType.videoCall ? 200.0 : 150.0,
            child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: <Widget>[
                  widget.callType == CallType.videoCall
                      ? new FloatingActionButton(
                          backgroundColor: this.switchCmr
                              ? Theme.of(context).secondaryHeaderColor
                              : null,
                          heroTag: Text("camera)"),
                          child: Icon(Icons.switch_camera),
                          onPressed: () {
                            if (localStream != null) {
                              setState(() {
                                this.switchCmr = !this.switchCmr;
                              });
                              Helper.switchCamera(
                                  localStream.getVideoTracks()[0]);
                            }
                          },
                        )
                      : Container(
                          height: 0,
                          width: 0,
                        ),
                  new FloatingActionButton(
                    heroTag: Text("hangUp"),
                    onPressed: () {
                      this.closeVideoCall();
                      Navigator.pop(context);
                    },
                    tooltip: 'Hangup',
                    child: Icon(Icons.call_end),
                    backgroundColor: Colors.pink,
                  ),
                  new FloatingActionButton(
                    backgroundColor: this.micOff
                        ? Theme.of(context).secondaryHeaderColor
                        : null,
                    heroTag: Text("micOff"),
                    child: const Icon(Icons.mic_off),
                    onPressed: () {
                      if (this.localStream != null) {
                        localStream.getAudioTracks()[0].enabled = micOff;
                        setState(() {
                          this.micOff = !this.micOff;
                        });
                      }
                    },
                  )
                ])),
        body: widget.callType == CallType.videoCall
            ? OrientationBuilder(builder: (context, orientation) {
                return Container(
                  child: Stack(children: <Widget>[
                    Positioned(
                        left: 0.0,
                        right: 0.0,
                        top: 0.0,
                        bottom: 0.0,
                        child: Container(
                          margin: EdgeInsets.fromLTRB(0.0, 0.0, 0.0, 0.0),
                          width: MediaQuery.of(context).size.width,
                          height: MediaQuery.of(context).size.height,
                          child: RTCVideoView(_remoteRenderer),
                          decoration: BoxDecoration(color: Colors.black54),
                        )),
                    Positioned(
                      left: 20.0,
                      top: 20.0,
                      child: Container(
                        width:
                            orientation == Orientation.portrait ? 90.0 : 120.0,
                        height:
                            orientation == Orientation.portrait ? 120.0 : 90.0,
                        child: RTCVideoView(_localRenderer),
                        decoration: BoxDecoration(color: Colors.black54),
                      ),
                    ),
                  ]),
                );
              })
            : Container(
                child: Center(
                  child: Container(
                    constraints: BoxConstraints.expand(),
                    decoration: BoxDecoration(
                      image: DecorationImage(
                        image: NetworkImage(widget.myFriend.avatarPath),
                        fit: BoxFit.cover,
                      ),
                    ),
                    child: ClipRRect(
                      child: BackdropFilter(
                        filter: ImageFilter.blur(sigmaX: 5, sigmaY: 5),
                        child: Container(
                          alignment: Alignment.center,
                          color: Colors.black.withOpacity(0.3),
                          child: Container(
                            padding: EdgeInsets.only(top: 80.0),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.center,
                              children: [
                                Container(
                                  width: 200.0,
                                  height: 200.0,
                                  child: CircleAvatar(
                                    backgroundImage: NetworkImage(
                                        widget.myFriend.avatarPath),
                                  ),
                                ),
                                SizedBox(
                                  height: 10.0,
                                ),
                                Container(
                                  child: Text(
                                    widget.myFriend.fullName,
                                    textAlign: TextAlign.center,
                                    style: TextStyle(
                                      fontSize: 35,
                                      fontWeight: FontWeight.bold,
                                      color: Colors.white,
                                    ),
                                  ),
                                ),
                                SizedBox(
                                  height: 5.0,
                                ),
                                Text(
                                  constructTime(this._secondCount),
                                  style: TextStyle(
                                    fontSize: 20,
                                    color: Colors.white,
                                  ),
                                ),
                              ],
                            ),
                          ),
                        ),
                      ),
                    ),
                  ),
                ),
              ),
      );
    } else {
      return Container();
    }
  }
}
