import 'package:flutter/material.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:tinder_clone/contants.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

Alert notificationWidget(
  BuildContext context, {
  String title,
  String desc,
  String textButton,
  AlertType typeNotication,
  Function onPressed,
}) {
  return Alert(
    context: context,
    type: typeNotication,
    title: title,
    desc: desc,
    buttons: [
      DialogButton(
        child: Text(
          textButton,
          style: TextStyle(color: Colors.white, fontSize: 55.sp),
        ),
        onPressed: onPressed != null ? onPressed : () => Navigator.pop(context),
        radius: BorderRadius.circular(0.0),
        gradient: colorApp,
      ),
    ],
  );
}
