import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Widgets/Notification.dart';
import 'package:tinder_clone/Widgets/RaisedGradientButton.dart';
import 'package:tinder_clone/contants.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class UploadApiImagesButton extends StatelessWidget {
  final bool checkCreateButton;
  final TextEditingController controller;
  UploadApiImagesButton(
      {@required this.checkCreateButton, @required this.controller});

  @override
  Widget build(BuildContext context) {
    if (checkCreateButton) {
      return Consumer<AuthenticationProvider>(
        builder: (context, authentication, child) {
          return Container(
            child: Container(
                width: double.infinity,
                padding: EdgeInsets.only(bottom: 90.h),
                child: RaisedGradientButton(
                  child: Text(
                    'CẬP NHẬT',
                    style: kFunctionTextStyle.copyWith(
                      color: Colors.white,
                      fontSize: 60.sp,
                    ),
                  ),
                  gradient: colorApp,
                  onPressed: () async {
                    Provider.of<Loading>(context, listen: false).setLoading();
                    bool checkaddImage =
                        await Provider.of<UserProvider>(context, listen: false)
                            .uploadImagesToServer(
                      userId: authentication.getUserInfo.id,
                      token: authentication
                          .getHeaders(authentication.getUserInfo.token),
                      title: controller.text == ""
                          ? "Cùng ngắm nhìn hình ảnh nào."
                          : controller.text,
                    );
                    Provider.of<Loading>(context, listen: false).setLoading();
                    if (checkaddImage) {
                      notificationWidget(
                        context,
                        title: 'THÊM HÌNH ẢNH',
                        desc: 'Thêm hình ảnh thành công',
                        textButton: "XÁC NHẬN",
                        typeNotication: AlertType.success,
                        onPressed: () {
                          Provider.of<UserProvider>(context, listen: false)
                              .cancelAddImages();
                          Navigator.pop(context);
                        },
                      ).show();
                    } else {
                      notificationWidget(
                        context,
                        title: 'THÊM HÌNH ẢNH',
                        desc: 'Thêm hình ảnh thất bại. Vui lòng thực hiện lại',
                        textButton: "HỦY",
                        typeNotication: AlertType.error,
                      ).show();
                    }
                  },
                )),
          );
        },
      );
    } else {
      return Container(
        color: Colors.white,
      );
    }
  }
}
