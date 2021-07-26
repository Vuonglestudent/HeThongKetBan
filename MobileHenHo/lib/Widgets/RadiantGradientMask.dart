import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class RadiantGradientMask extends StatelessWidget {
  RadiantGradientMask({this.child, this.setColor});
  final Widget child;
  final bool setColor;

  @override
  Widget build(BuildContext context) {
    if (setColor) {
      return ShaderMask(
        shaderCallback: (bounds) => RadialGradient(
          center: Alignment.center,
          radius: 0.5,
          colors: [
            Theme.of(context).accentColor,
            Theme.of(context).secondaryHeaderColor,
            Theme.of(context).primaryColor
          ],
          tileMode: TileMode.mirror,
        ).createShader(bounds),
        child: child,
      );
    } else {
      return Container(
        child: child,
      );
    }
  }
}
