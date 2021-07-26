import 'package:flutter/material.dart';
import 'package:tinder_clone/Screens/LoginGmail_Screen.dart';
import 'package:tinder_clone/Services/SignalRService.dart';
import 'package:tinder_clone/contants.dart';
import 'package:tinder_clone/Widgets/LoginButton.dart';
import 'package:animated_text_kit/animated_text_kit.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class LoginScreen extends StatefulWidget {
  static const String id = 'Login_Screen';
  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen>
    with SingleTickerProviderStateMixin {
  //GlobalKey<ScaffoldState> _scaffoldkey = new GlobalKey<ScaffoldState>();
  AnimationController _controller;

  @override
  void initState() {
    super.initState();
    _controller = AnimationController(
      duration: Duration(seconds: 1),
      vsync: this,
    );
    _controller.forward();
    _controller.addListener(() {
      setState(() {});
    });
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Container(
          decoration: BoxDecoration(
              gradient: LinearGradient(
                  colors: [
                    Theme.of(context).accentColor,
                    Theme.of(context).secondaryHeaderColor,
                    Theme.of(context).primaryColor
                  ],
                  begin: Alignment.topRight,
                  end: Alignment.bottomRight,
                  stops: [0.0, 0.35, 1.0])),
          child: Column(
            children: <Widget>[
              Expanded(
                  flex: 6,
                  child: Center(
                      child: Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: <Widget>[
                      Hero(
                        tag: 'logoIcon',
                        child: Container(
                          height: _controller.value * 160.h,
                          child: Image(
                            image: AssetImage('assets/images/logo.png'),
                          ),
                        ),
                      ),
                      TyperAnimatedTextKit(
                        text: ["", "inger"],
                        speed: Duration(milliseconds: 1000),
                        textStyle: kLogoTextStyle,
                        pause: Duration(milliseconds: 1000),
                      )
                    ],
                  ))),
              Expanded(
                  flex: 3,
                  child: Padding(
                    padding: EdgeInsets.all(60.sp),
                    child: Column(
                      children: <Widget>[
                        Text(
                          'Nhấn "Đăng nhập" nếu bạn đồng ý các điều khoản của chúng tôi',
                          textAlign: TextAlign.center,
                          style: kRuleTextStyle.copyWith(fontSize: 40.sp),
                        ),
                        SizedBox(height: 40.h),
                        LoginButton(
                          title: "ĐĂNG NHẬP HỆ THỐNG",
                          onPressed: () {
                            Navigator.pushNamed(context, LoginGmailScreen.id);
                          },
                        ),
                        SizedBox(height: 70.h),
                        Text(
                          "Bạn gặp sự cố khi đăng nhập?",
                          style:
                              kTroubleLoginTextStyle.copyWith(fontSize: 50.sp),
                        )
                      ],
                    ),
                  ))
            ],
          )),
    );
  }
}
