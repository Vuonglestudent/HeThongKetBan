import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/contants.dart';

class AddImagesProfile extends StatefulWidget {
  @override
  _AddImagesProfileState createState() => _AddImagesProfileState();
}

class _AddImagesProfileState extends State<AddImagesProfile> {
  @override
  Widget build(BuildContext context) {
    return Expanded(
        child: Stack(
      children: <Widget>[
        Container(
          color: Colors.white,
        ),
        Positioned(
          right: 1.w,
          bottom: 30.h,
          left: 1.w,
          child: Column(
            mainAxisAlignment: MainAxisAlignment.end,
            children: <Widget>[
              FlatButton(
                shape: CircleBorder(),
                color: Colors.white,
                onPressed: () async {
                  await Provider.of<UserProvider>(context, listen: false)
                      .loadAssets();
                },
                onLongPress: () {
                  Provider.of<UserProvider>(context, listen: false)
                      .cancelAddImages();
                },
                child: Container(
                  width: 170.w,
                  height: 160.h,
                  decoration: BoxDecoration(
                      gradient: LinearGradient(
                          colors: [
                            Theme.of(context).accentColor,
                            Theme.of(context).secondaryHeaderColor,
                            Theme.of(context).primaryColor
                          ],
                          begin: Alignment.topRight,
                          end: Alignment.bottomRight,
                          stops: [0.0, 0.35, 1.0]),
                      color: Colors.green,
                      borderRadius: BorderRadius.circular(150.0)),
                  child: Icon(
                    Icons.camera_alt,
                    color: Colors.white,
                    size: ScreenUtil().setSp(125.0),
                  ),
                ),
              ),
              SizedBox(height: ScreenUtil().setHeight(10.0)),
              Text(
                "THÊM ẢNH",
                style: kFunctionTextStyle.copyWith(
                  color: Colors.blueGrey.shade200,
                ),
              )
            ],
          ),
        ),
        Positioned(
          right: ScreenUtil().setWidth(80.0),
          bottom: ScreenUtil().setWidth(107.0),
          child: Container(
            width: ScreenUtil().setHeight(50),
            height: ScreenUtil().setHeight(50),
            decoration: BoxDecoration(boxShadow: [
              BoxShadow(
                color: Colors.grey,
                offset: Offset(2.0, 3.0),
                blurRadius: 5.0,
              )
            ], color: Colors.white, borderRadius: BorderRadius.circular(25)),
            child: Center(
              child: Icon(
                Icons.add,
                size: 50.sp,
                color: Theme.of(context).accentColor,
              ),
            ),
          ),
        ),
      ],
    ));
  }
}

final Shader linearGradient = new LinearGradient(
    colors: [Colors.amber.shade800, Colors.amber.shade600],
    begin: Alignment.topRight,
    end: Alignment.bottomLeft,
    stops: [0.0, 1.0]).createShader(
  Rect.fromLTWH(
    0.0,
    0.0,
    ScreenUtil().setWidth(30),
    ScreenUtil().setHeight(20),
  ),
);
