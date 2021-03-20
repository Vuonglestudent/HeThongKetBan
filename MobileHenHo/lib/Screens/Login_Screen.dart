import 'package:flutter/material.dart';
import 'package:tinder_clone/Screens/LoginGmail_Screen.dart';
import 'package:tinder_clone/Services/SignalRService.dart';
import 'package:tinder_clone/contants.dart';
import 'package:tinder_clone/Widgets/LoginButton.dart';
import 'package:animated_text_kit/animated_text_kit.dart';

class LoginScreen extends StatefulWidget {
  static const String id = 'Login_Screen';
  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen>
    with SingleTickerProviderStateMixin {
  //GlobalKey<ScaffoldState> _scaffoldkey = new GlobalKey<ScaffoldState>();
  AnimationController _controller;

  SignalRService signalRService = SignalRService();
  @override
  void initState() {
    super.initState();
    signalRService.startConnectHub();
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
                          height: _controller.value * 80.0,
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
                    padding: EdgeInsets.all(30.0),
                    child: Column(
                      children: <Widget>[
                        Text(
                          'By clicking "Log in",you agree with our Terms.\n Learn how we process your data in our Privacy  Policy and Cookies Policy',
                          textAlign: TextAlign.center,
                          style: kRuleTextStyle,
                        ),
                        SizedBox(height: 20.0),
                        LoginButton(
                          title: "LOG IN WITH ACCOUNT SYSTEM",
                          onPressed: () {
                            Navigator.pushNamed(context, LoginGmailScreen.id);
                          },
                        ),
                        SizedBox(height: 35.0),
                        Text(
                          "Trouble logging in?",
                          style: kTroubleLoginTextStyle,
                        )
                      ],
                    ),
                  ))
            ],
          )),
    );
  }
}
