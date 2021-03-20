import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:tinder_clone/contants.dart';

class IconsProfileExpanded extends StatelessWidget {
  final String titleButton;
  final IconData icon;
  final Function onPressed;
  IconsProfileExpanded(
      {@required this.titleButton,
      @required this.icon,
      @required this.onPressed});

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        children: <Widget>[
          FlatButton(
            shape: CircleBorder(),
            onPressed: onPressed,
            child: Container(
              width: 165.w,
              height: 140.h,
              decoration: BoxDecoration(
                  color: Colors.blueGrey.shade50,
                  borderRadius: BorderRadius.circular(100.0)),
              child: Icon(
                icon,
                size: 100.sp,
                color: Colors.blueGrey.shade200,
              ),
            ),
          ),
          SizedBox(height: 10.h),
          Text(
            titleButton,
            style: kFunctionTextStyle.copyWith(
              color: Colors.blueGrey.shade200,
            ),
          )
        ],
      ),
    );
  }
}
