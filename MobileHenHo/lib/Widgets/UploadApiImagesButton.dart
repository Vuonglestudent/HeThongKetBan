import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Widgets/Notification.dart';
import 'package:tinder_clone/Widgets/RaisedGradientButton.dart';
import 'package:tinder_clone/contants.dart';

class UploadApiImagesButton extends StatelessWidget {
  final bool checkCreateButton;
  UploadApiImagesButton({@required this.checkCreateButton});

  @override
  Widget build(BuildContext context) {
    if (checkCreateButton) {
      return Consumer<AuthenticationProvider>(
        builder: (context, authentication, child) {
          return Expanded(
            flex: 1,
            child: Container(
                width: double.infinity,
                padding: EdgeInsets.only(bottom: 35.0),
                child: RaisedGradientButton(
                  child: Text(
                    'UPLOAD',
                    style: kFunctionTextStyle.copyWith(
                      color: Colors.white,
                      fontSize: 20.0,
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
                    );
                    Provider.of<Loading>(context, listen: false).setLoading();
                    if (checkaddImage) {
                      notificationWidget(
                        context,
                        title: 'ADD IMAGES',
                        desc: 'Thêm hình ảnh thành công',
                        textButton: "OK",
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
                        title: 'ADD IMAGES',
                        desc: 'Thêm hình ảnh thất bại. Vui lòng thực hiện lại',
                        textButton: "Cancel",
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
