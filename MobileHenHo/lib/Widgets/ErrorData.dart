import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class ErrorData extends StatelessWidget {
  final String titleError;

  ErrorData({@required this.titleError});
  @override
  Widget build(BuildContext context) {
    return Container(
      child: Column(
        children: <Widget>[
          SizedBox(
            height: ScreenUtil().setHeight(100.0),
          ),
          Image(
              width: ScreenUtil().setWidth(600),
              height: ScreenUtil().setHeight(400),
              fit: BoxFit.cover,
              image: new AssetImage('assets/images/sorry.png')),
          SizedBox(
            height: ScreenUtil().setHeight(50.0),
          ),
          Text(
            "Check back later",
            style: new TextStyle(
                wordSpacing: 1.5,
                fontSize: ScreenUtil().setSp(75.0),
                fontWeight: FontWeight.w400),
          ),
          SizedBox(
            height: ScreenUtil().setWidth(20.0),
          ),
          Padding(
            padding:
                EdgeInsets.symmetric(horizontal: ScreenUtil().setWidth(60.0)),
            child: new Text(titleError,
                textAlign: TextAlign.center,
                style: new TextStyle(
                    fontSize: ScreenUtil().setSp(40.0),
                    fontWeight: FontWeight.w300,
                    color: Colors.grey.shade600)),
          )
        ],
      ),
    );
  }
}
