import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/contants.dart';
import 'package:modal_progress_hud/modal_progress_hud.dart';
import 'package:animated_text_kit/animated_text_kit.dart';
import 'package:tinder_clone/Widgets/Animation/FadeAnimation.dart';
import 'package:tinder_clone/Widgets/LoginWithOutSystemButton.dart';
import 'package:tinder_clone/Widgets/InputLoginTextField.dart';

class LoginGmailScreen extends StatefulWidget {
  static const String id = 'LoginGmail_Screen';
  @override
  _LoginGmailScreenState createState() => _LoginGmailScreenState();
}

class _LoginGmailScreenState extends State<LoginGmailScreen> {
  String username = '';
  String password = '';
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
                height: 80,
              ),
              Padding(
                padding: EdgeInsets.all(20),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: <Widget>[
                    Row(
                      children: [
                        Hero(
                          tag: 'logoIcon',
                          child: Container(
                            height: 60.0,
                            child: Image.asset('assets/images/logo.png'),
                          ),
                        ),
                        TyperAnimatedTextKit(
                          text: ["", "inger"],
                          speed: Duration(milliseconds: 1000),
                          textStyle: kLogoTextStyle.copyWith(fontSize: 60),
                          pause: Duration(milliseconds: 1000),
                        )
                      ],
                    ),
                    SizedBox(
                      height: 10,
                    ),
                    FadeAnimation(
                        1.3,
                        Text(
                          "Welcome Back",
                          style: TextStyle(color: Colors.white, fontSize: 18),
                        )),
                  ],
                ),
              ),
              SizedBox(height: 20),
              Expanded(
                child: Container(
                  decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.only(
                          topLeft: Radius.circular(60),
                          topRight: Radius.circular(60))),
                  child: SingleChildScrollView(
                    child: Padding(
                      padding: EdgeInsets.all(30),
                      child: Column(
                        children: <Widget>[
                          SizedBox(
                            height: 10,
                          ),
                          FadeAnimation(
                              1.4,
                              Container(
                                decoration: BoxDecoration(
                                    color: Colors.white,
                                    borderRadius: BorderRadius.circular(10),
                                    boxShadow: [
                                      BoxShadow(
                                          color:
                                              Color.fromRGBO(225, 95, 27, .3),
                                          blurRadius: 20,
                                          offset: Offset(0, 10))
                                    ]),
                                child: Column(
                                  children: <Widget>[
                                    InputLoginTextField(
                                      titleTextFile: "Email or Phone number",
                                      obscureText: false,
                                      onChanged: (value) {
                                        username = value;
                                      },
                                    ),
                                    InputLoginTextField(
                                      titleTextFile: "Password",
                                      obscureText: true,
                                      onChanged: (value) {
                                        password = value;
                                      },
                                    ),
                                  ],
                                ),
                              )),
                          SizedBox(
                            height: 40,
                          ),
                          FadeAnimation(
                              1.5,
                              Text(
                                "Forgot Password?",
                                style: TextStyle(color: Colors.grey),
                              )),
                          SizedBox(
                            height: 40,
                          ),
                          FadeAnimation(
                              1.6,
                              Container(
                                height: 50,
                                margin: EdgeInsets.symmetric(horizontal: 50),
                                decoration: BoxDecoration(
                                    borderRadius: BorderRadius.circular(50),
                                    color: Colors.orange[900]),
                                child: FlatButton(
                                  onPressed: () async {
                                    await Provider.of<AuthenticationProvider>(context,
                                            listen: false)
                                        .checkAuthenticationLogin(
                                      context: context,
                                      username: username,
                                      password: password,
                                    );
                                  },
                                  child: Center(
                                    child: Text(
                                      "Login",
                                      style: TextStyle(
                                          color: Colors.white,
                                          fontWeight: FontWeight.bold),
                                    ),
                                  ),
                                ),
                              )),
                          SizedBox(
                            height: 50,
                          ),
                          FadeAnimation(
                              1.7,
                              Text(
                                "Continue with social media",
                                style: TextStyle(color: Colors.grey),
                              )),
                          SizedBox(
                            height: 20,
                          ),
                          Row(
                            children: <Widget>[
                              LoginWithOutSystemButton(
                                titleButton: "Facebook",
                                colour: Colors.blue,
                                timerFade: 1.8,
                              ),
                              SizedBox(
                                width: 30,
                              ),
                              LoginWithOutSystemButton(
                                titleButton: "Github",
                                colour: Colors.black,
                                timerFade: 1.9,
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
