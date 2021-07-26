import 'package:flutter/material.dart';
import 'package:tinder_clone/Widgets/Animation/FadeAnimation.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class LoginWithOutSystemButton extends StatelessWidget {
  final String titleButton;
  final Color colour;
  final String pathImage;
  final Function onPressed;
  double timerFade = 0;

  LoginWithOutSystemButton({
    @required this.titleButton,
    @required this.colour,
    this.pathImage,
    this.timerFade,
    @required this.onPressed,
  });

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: FadeAnimation(
        timerFade,
        MaterialButton(
          onPressed: onPressed,
          child: Container(
            height: 130.h,
            decoration: BoxDecoration(
              borderRadius: BorderRadius.circular(50),
              color: colour,
            ),
            child: Row(
              crossAxisAlignment: CrossAxisAlignment.center,
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Image(
                  image: AssetImage(pathImage),
                  height: 60.h,
                ),
                SizedBox(
                  width: 5.w,
                ),
                Text(
                  titleButton,
                  style: TextStyle(
                    color: Colors.white,
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
