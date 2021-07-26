import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/User.dart';
import 'package:tinder_clone/Models/UserConnection.dart';
import 'package:tinder_clone/Screens/CallVideo_Screen.dart';
import 'package:tinder_clone/Services/SignalRService.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class AccpectCallerScreen extends StatefulWidget {
  final IUser caller;
  AccpectCallerScreen({@required this.caller});
  @override
  _AccpectCallerScreenState createState() => _AccpectCallerScreenState();
}

class _AccpectCallerScreenState extends State<AccpectCallerScreen> {
  AuthenticationProvider authen;
  @override
  Widget build(BuildContext context) {
    authen = Provider.of<AuthenticationProvider>(context);
    authen.isCalling = true;
    return Scaffold(
      body: Stack(
        children: [
          Center(
            child: Container(
              constraints: BoxConstraints.expand(),
              decoration: BoxDecoration(
                image: DecorationImage(
                  image: NetworkImage(widget.caller.avatarPath),
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
                      padding: EdgeInsets.only(top: 220.h),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: [
                          Container(
                            width: 550.h,
                            height: 550.h,
                            child: CircleAvatar(
                              backgroundImage:
                                  NetworkImage(widget.caller.avatarPath),
                            ),
                          ),
                          SizedBox(
                            height: 20.h,
                          ),
                          Container(
                            child: Text(
                              widget.caller.userName,
                              textAlign: TextAlign.center,
                              style: TextStyle(
                                fontSize: 100.sp,
                                fontWeight: FontWeight.bold,
                                color: Colors.white,
                              ),
                            ),
                          ),
                          SizedBox(
                            height: 50.h,
                          ),
                          Container(
                            child: Text(
                              widget.caller.callType == CallType.videoCall
                                  ? "Đang gọi video"
                                  : "Đang gọi",
                              textAlign: TextAlign.center,
                              style: TextStyle(
                                fontSize: 60.sp,
                                color: Colors.white,
                              ),
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
          Positioned.fill(
            bottom: 200.h,
            top: 1300.h,
            left: 0,
            child: Container(
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  Container(
                    child: ElevatedButton(
                      style: ElevatedButton.styleFrom(
                        primary: Colors.red,
                        shape: CircleBorder(),
                        padding: EdgeInsets.all(60.sp),
                      ),
                      child: Icon(
                        Icons.call_end,
                        size: 120.sp,
                      ),
                      onPressed: () async {
                        authen.signalR
                            .callReject(widget.caller.connectionId)
                            .then((value) => Navigator.pop(context));
                      },
                    ),
                  ),
                  SizedBox(
                    width: 40.w,
                  ),
                  Container(
                    child: ElevatedButton(
                      style: ElevatedButton.styleFrom(
                        primary: Colors.green,
                        shape: CircleBorder(),
                        padding: EdgeInsets.all(60.sp),
                      ),
                      child: Icon(
                        widget.caller.callType == CallType.videoCall
                            ? Icons.video_call
                            : Icons.call,
                        size: 120.sp,
                      ),
                      onPressed: () {
                        UsersSimilar user = new UsersSimilar(
                          id: widget.caller.userId,
                          fullName: widget.caller.userName,
                          avatarPath: widget.caller.avatarPath,
                        );
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (BuildContext context) => CallVideoScreen(
                              myFriend: user,
                              callType: widget.caller.callType,
                              isAccept: true,
                              context: context,
                            ),
                          ),
                        ).then((value) => Navigator.pop(context));
                      },
                    ),
                  )
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}
