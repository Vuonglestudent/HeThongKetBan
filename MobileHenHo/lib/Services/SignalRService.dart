import 'dart:convert';
import 'package:flutter/cupertino.dart';
import 'package:flutter_webrtc/flutter_webrtc.dart';
import 'package:provider/provider.dart';
import 'package:rxdart/rxdart.dart';
import 'package:signalr_client/signalr_client.dart';
import 'package:tinder_clone/Models/INotification.dart';
import 'package:tinder_clone/Models/Message.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Internet.dart';
import 'package:tinder_clone/Models/UserConnection.dart';

enum CallType {
  videoCall,
  voiceCall,
}

class Signal {
  SignalType type;
  dynamic data;

  Signal(this.type, this.data);
}

enum CallStatus { accepted, rejected }

enum SignalType { iceCandidate, offer, answer, hangUp }

String get sdpSemantics => WebRTC.platformIsWindows ? 'plan-b' : 'unified-plan';

class SignalRService {
  //final urlHub = 'http://10.0.2.2:5100/chatHub';
  final urlHub = 'https://hieuit.tech:5201/chatHub';

  String connectionId;
  int onlineCount = 0;
  HubConnection hubConnection;
  int idMessageCurrent = -1;
  String senderId = '';
  String recevierId = '';
  Internet internet = Internet(urlAPI: '/users');
  Message _message = new Message();
  var _messageSub = new BehaviorSubject<Message>();
  var _notificationSub = new BehaviorSubject<INotification>();
  var _callerSub = new BehaviorSubject<IUser>();
  var _myInfoSub = new BehaviorSubject<IUser>();
  var _signalSub = new BehaviorSubject<Signal>();

  List<dynamic> currentIceServers;

  final configuration = <String, dynamic>{
    'iceServers': [
      {
        'urls': 'stun:stun.l.google.com:19302',
        'username': '',
        'credential': ''
      },
    ],
    'sdpSemantics': sdpSemantics
  };
  final loopbackConstraints = <String, dynamic>{
    'mandatory': {},
    'optional': [
      {'DtlsSrtpKeyAgreement': true},
    ],
  };

  final Map<String, dynamic> _config = {
    'mandatory': {},
    'optional': [
      {'DtlsSrtpKeyAgreement': true},
    ]
  };
  IUser userInfo = new IUser();
  void startConnectHub(BuildContext context) async {
    hubConnection = HubConnectionBuilder().withUrl(urlHub).build();
    print("startConnection");
    (() async {
      await hubConnection.start();
      connectionId = await hubConnection.invoke('GetConnectionId');
      if (connectionId != null) {
        String userName =
            Provider.of<AuthenticationProvider>(context, listen: false)
                .getUserInfo
                .fullName;

        String userId =
            Provider.of<AuthenticationProvider>(context, listen: false)
                .getUserInfo
                .id;

        String avatarPath =
            Provider.of<AuthenticationProvider>(context, listen: false)
                .getUserInfo
                .avatarPath;

        userInfo.userId = userId;
        userInfo.userName = userName;
        userInfo.avatarPath = avatarPath;

        Map<String, dynamic> result = await hubConnection.invoke('saveMyInfo',
            args: <Object>[userId, connectionId, userName, avatarPath]);

        IUser user = IUser(
            userId: result['userId'],
            connectionId: result['connectionId'],
            userName: result['userName'],
            avatarPath: result['avatarPath']);

        _myInfoSub.add(user);
      }
    })();

    hubConnection.on('onlineCount', (data) async {
      onlineCount = int.parse(data[0].toString());
      print('online count' + onlineCount.toString());
    });

    //Nhận thông báo
    hubConnection.on('notification', (data) async {
      print('hubNotification');
      Map<String, dynamic> json = data[0];
      INotification noti = new INotification(
        id: json['id'],
        fromId: json['fromId'],
        fullName: json['fullName'],
        toId: json['toId'],
        createdAt: DateTime.parse(json['createdAt']),
        avatar: json['avatar'],
        type: json['type'],
        content: json['content'],
      );

      _notificationSub.add(noti);
    });

    //Tin nhắn
    hubConnection.on('messageResponse', (data) async {
      _message.setChatFriendMessage(data[0]);
      print('messageResponse: ${_message.content}');
      _messageSub.add(_message);
    });

    hubConnection.on('receiveSignal', (data) async {
      print('receiveSignal');
      Map<String, dynamic> json = jsonDecode(data[0]);

      Signal s = Signal(SignalType.values[json['type']], json['data']);

      _signalSub.add(s);
    });

    hubConnection.on('CallRequest', (data) async {
      print('callRequest');
      print(data);
      Map<String, dynamic> json = data[0];
      IUser user = new IUser(
        userId: json['userId'],
        userName: json['userName'],
        avatarPath: json['avatarPath'],
        connectionId: json['connectionId'],
        callType: CallType.values[data[1]],
      );

      callerSub.add(user);
    });

    hubConnection.on('callStatus', (data) async {
      print('callStatus');
      print(data);
    });
  }

  void closeConnect() {
    if (hubConnection != null) hubConnection.stop();
  }

  BehaviorSubject<Message> get messageSub {
    return _messageSub;
  }

  BehaviorSubject<INotification> get notificationSub {
    return _notificationSub;
  }

  BehaviorSubject<IUser> get callerSub {
    return _callerSub;
  }

  BehaviorSubject<IUser> get myInfoSub {
    return _myInfoSub;
  }

  BehaviorSubject<Signal> get signalSub {
    return _signalSub;
  }

  /////////////////  --- #INVOKE ---------------------------
  Future saveMyInfo() async {
    Map<String, dynamic> result = await this.hubConnection.invoke('saveMyInfo',
        args: <Object>[
          userInfo.userId,
          userInfo.connectionId,
          userInfo.userName,
          userInfo.avatarPath
        ]);

    IUser user = IUser(
        userId: result['userId'],
        connectionId: result['connectionId'],
        userName: result['userName'],
        avatarPath: result['avatarPath']);
    _myInfoSub.add(user);
  }

  Future<IUser> getUserById(String userId) async {
    Map<String, dynamic> result =
        await this.hubConnection.invoke('GetUserById', args: <String>[userId]);

    IUser user = IUser(
        userId: result['userId'],
        connectionId: result['connectionId'],
        userName: result['userName'],
        avatarPath: result['avatarPath']);
    return user;
  }

  Future<IUser> getUserByConnectionId(String connectionId) async {
    Map<String, dynamic> result = await this
        .hubConnection
        .invoke('GetUserByConnectionId', args: <Object>[connectionId]);

    IUser user = IUser(
        userId: result['userId'],
        connectionId: result['connectionId'],
        userName: result['userName'],
        avatarPath: result['avatarPath']);
    return user;
  }

  Future callRequest(String userId, CallType callType) async {
    await this
        .hubConnection
        .invoke('CallRequest', args: <Object>[userId, callType.index]);
  }

  Future callAccept(String connectionId) async {
    await this.hubConnection.invoke('CallAccept', args: <Object>[connectionId]);
  }

  Future callReject(String connectionId) async {
    await this.hubConnection.invoke('CallReject', args: <Object>[connectionId]);
  }

  Future<void> sendSignal(Signal signal, String receiverId) async {
    var jsonMessage = {'type': signal.type.index, 'data': signal.data};

    await this.hubConnection.invoke('SendSignal',
        args: <Object>[jsonEncode(jsonMessage), receiverId]);
  }

  // --------------------------------------------

  //Yêu cầu quyền truy cập
  Future<MediaStream> getUserMediaInternal(CallType callType) async {
    final Map<String, dynamic> mediaConstraints = {
      'audio': true,
      'video': {
        'mandatory': {
          'minWidth':
              '800', // Provide your own width, height and frame rate here
          'minHeight': '600',
          'minFrameRate': '15',
        },
        'facingMode': 'user',
        'optional': [],
      }
    };
    // if (this.currentMediaStream != null) {
    //   return this.currentMediaStream;
    // }

    try {
      return await navigator.mediaDevices.getUserMedia(mediaConstraints);
    } catch (error) {
      print('Failed to get hardware access ' + error.toString());
    }
  }
}
