import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:tinder_clone/HomePage.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Widgets/Notification.dart';
import 'package:tinder_clone/contants.dart';
import 'package:modal_progress_hud/modal_progress_hud.dart';
import 'package:animated_text_kit/animated_text_kit.dart';
import 'package:tinder_clone/Widgets/Animation/FadeAnimation.dart';
import 'package:tinder_clone/Widgets/LoginWithOutSystemButton.dart';
import 'package:tinder_clone/Widgets/InputLoginTextField.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class LoginGmailScreen extends StatefulWidget {
  static const String id = 'LoginGmail_Screen';
  @override
  _LoginGmailScreenState createState() => _LoginGmailScreenState();
}

class _LoginGmailScreenState extends State<LoginGmailScreen> {
  String username = 'tien@gmail.com';
  String password = '1111';
  @override
  Widget build(BuildContext context) {
    return ModalProgressHUD(
      inAsyncCall: Provider.of<Loading>(context).loading,
      child: Scaffold(
        backgroundColor: Colors.white,
        body: Container(
          width: double.infinity,
          decoration: BoxDecoration(
              gradient: LinearGradient(begin: Alignment.topCenter, colors: [
            Theme.of(context).accentColor,
            Theme.of(context).secondaryHeaderColor,
            Theme.of(context).primaryColor,
          ])),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: <Widget>[
              SizedBox(
                height: 160.h,
              ),
              Padding(
                padding: EdgeInsets.all(60.h),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: <Widget>[
                    Row(
                      children: [
                        Hero(
                          tag: 'logoIcon',
                          child: Container(
                            height: 150.h,
                            child: Image.asset('assets/images/logo.png'),
                          ),
                        ),
                        TyperAnimatedTextKit(
                          text: ["", "inger"],
                          speed: Duration(milliseconds: 1000),
                          textStyle: kLogoTextStyle.copyWith(fontSize: 120.sp),
                          pause: Duration(milliseconds: 1000),
                        )
                      ],
                    ),
                    SizedBox(
                      height: 20.h,
                    ),
                    FadeAnimation(
                        1.3,
                        Text(
                          "Chào mừng bạn đến với ứng dụng",
                          style:
                              TextStyle(color: Colors.white, fontSize: 50.sp),
                        )),
                  ],
                ),
              ),
              SizedBox(height: 40.h),
              Expanded(
                child: Container(
                  decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.only(
                          topLeft: Radius.circular(180.sp),
                          topRight: Radius.circular(180.sp))),
                  child: SingleChildScrollView(
                    child: Padding(
                      padding: EdgeInsets.all(80.sp),
                      child: Column(
                        children: <Widget>[
                          SizedBox(
                            height: 20.sp,
                          ),
                          FadeAnimation(
                              1.4,
                              Container(
                                decoration: BoxDecoration(
                                    color: Colors.white,
                                    borderRadius: BorderRadius.circular(20.sp),
                                    boxShadow: [
                                      BoxShadow(
                                          color:
                                              Color.fromRGBO(225, 95, 27, .3),
                                          blurRadius: 40.sp,
                                          offset: Offset(0.w, 20.h))
                                    ]),
                                child: Column(
                                  children: <Widget>[
                                    InputLoginTextField(
                                      titleTextFile: "Email hoặc số điện thoại",
                                      obscureText: false,
                                      onChanged: (value) {
                                        username = value;
                                      },
                                    ),
                                    InputPasswordTextField(
                                      titleTextFile: "Mật khẩu",
                                      obscureText: true,
                                      onChanged: (value) {
                                        password = value;
                                      },
                                    ),
                                  ],
                                ),
                              )),
                          SizedBox(
                            height: 110.h,
                          ),
                          FadeAnimation(
                              1.5,
                              Text(
                                "Quên mật khẩu?",
                                style: TextStyle(color: Colors.grey),
                              )),
                          SizedBox(
                            height: 110.h,
                          ),
                          FadeAnimation(
                              1.6,
                              Container(
                                height: 130.h,
                                margin:
                                    EdgeInsets.symmetric(horizontal: 150.sp),
                                decoration: BoxDecoration(
                                    borderRadius: BorderRadius.circular(100.sp),
                                    color: Colors.orange[900]),
                                child: TextButton(
                                  onPressed: () async {
                                    await Provider.of<AuthenticationProvider>(
                                            context,
                                            listen: false)
                                        .checkAuthenticationLogin(
                                      context: context,
                                      username: username,
                                      password: password,
                                    );
                                  },
                                  child: Center(
                                    child: Row(
                                      mainAxisAlignment:
                                          MainAxisAlignment.center,
                                      children: [
                                        Icon(
                                          Icons.login,
                                          color: Colors.white,
                                        ),
                                        SizedBox(
                                          width: 15.h,
                                        ),
                                        Text(
                                          "Đăng nhập",
                                          style: TextStyle(
                                              color: Colors.white,
                                              fontWeight: FontWeight.bold),
                                        ),
                                      ],
                                    ),
                                  ),
                                ),
                              )),
                          SizedBox(
                            height: 120.h,
                          ),
                          FadeAnimation(
                              1.7,
                              Text(
                                "Đăng nhập với mạng xã hội",
                                style: TextStyle(color: Colors.grey),
                              )),
                          SizedBox(
                            height: 50.h,
                          ),
                          Row(
                            children: <Widget>[
                              LoginWithOutSystemButton(
                                titleButton: "Facebook",
                                colour: Color.fromRGBO(24, 119, 242, 1),
                                pathImage: "assets/images/face_logo.png",
                                timerFade: 1.8,
                                onPressed: () async {
                                  Provider.of<Loading>(context, listen: false)
                                      .setLoading();
                                  await Provider.of<AuthenticationProvider>(
                                          context,
                                          listen: false)
                                      .loginWithFacebook()
                                      .then((value) {
                                    value
                                        ? Navigator.pushNamed(
                                            context, HomePage.id)
                                        : notificationWidget(
                                            context,
                                            title: 'Lỗi Đăng Nhập',
                                            desc:
                                                'Sử dụng đăng nhập bằng facebook không thành công!',
                                            textButton: 'Cancel',
                                            typeNotication: AlertType.error,
                                          ).show();
                                  });
                                  Provider.of<Loading>(context, listen: false)
                                      .setLoading();
                                },
                              ),
                              SizedBox(
                                width: 20.h,
                              ),
                              LoginWithOutSystemButton(
                                titleButton: "Google",
                                colour: Colors.black,
                                pathImage: "assets/images/google_logo.png",
                                timerFade: 1.9,
                                onPressed: () async {},
                              ),
                            ],
                          )
                        ],
                      ),
                    ),
                  ),
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}
