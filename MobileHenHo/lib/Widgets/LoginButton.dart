import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class LoginButton extends StatelessWidget {
  LoginButton({@required this.title, @required this.onPressed});
  final String title;
  final Function onPressed;
  @override
  Widget build(BuildContext context) {
    return Container(
      width: double.infinity,
      height: 120.h,
      child: RaisedButton(
        shape:
            RoundedRectangleBorder(borderRadius: BorderRadius.circular(90.0)),
        color: Colors.white,
        elevation: 0.0,
        onPressed: onPressed,
        child: Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Text(
              title,
              style: TextStyle(
                  color: Colors.grey, wordSpacing: 1.2, fontSize: 55.sp),
            )
          ],
        ),
      ),
    );
  }
}
