import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class SaveAndCanceSetting extends StatelessWidget {
  final Function onPressSave;
  final Function onPRessCance;

  SaveAndCanceSetting({this.onPressSave, this.onPRessCance});

  @override
  Widget build(BuildContext context) {
    return Align(
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          RaisedButton(
            child: Text('Hủy', style: TextStyle(fontSize: 55.sp)),
            color: Theme.of(context).accentColor,
            textColor: Colors.white,
            elevation: 5,
            onPressed: onPRessCance,
          ),
          SizedBox(width: 20.w),
          RaisedButton(
            child: Text('Lưu', style: TextStyle(fontSize: 55.sp)),
            color: Theme.of(context).secondaryHeaderColor,
            textColor: Colors.white,
            elevation: 5,
            onPressed: onPressSave,
          )
        ],
      ),
    );
  }
}
