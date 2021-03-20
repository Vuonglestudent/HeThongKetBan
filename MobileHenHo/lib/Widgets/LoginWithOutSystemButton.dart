import 'package:flutter/material.dart';
import 'package:tinder_clone/Widgets/Animation/FadeAnimation.dart';

class LoginWithOutSystemButton extends StatelessWidget {
  final String titleButton;
  final Color colour;
  double timerFade = 0;

  LoginWithOutSystemButton(
      {@required this.titleButton, @required this.colour, this.timerFade});

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: FadeAnimation(
          timerFade,
          Container(
            height: 50,
            decoration: BoxDecoration(
              borderRadius: BorderRadius.circular(50),
              color: colour,
            ),
            child: Center(
              child: Text(
                titleButton,
                style: TextStyle(
                  color: Colors.white,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
          )),
    );
  }
}
