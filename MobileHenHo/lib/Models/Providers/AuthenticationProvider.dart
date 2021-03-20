import 'dart:io';
import 'package:flutter/cupertino.dart';
import 'package:flutter/foundation.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:tinder_clone/HomePage.dart';
import 'package:tinder_clone/Models/UserInfo.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Services/AuthenticationService.dart';
import 'package:tinder_clone/Widgets/Notification.dart';
import 'package:tinder_clone/Services/SignalRService.dart';
import 'package:provider/provider.dart';

class AuthenticationProvider extends ChangeNotifier {
  UserInfo _userInfo = UserInfo();
  bool isLogin = false;

  UserInfo get getUserInfo {
    return _userInfo;
  }

  bool get getIsLogin {
    return isLogin;
  }

  Future<bool> checkLoginUserInfo(
      {@required username, @required password}) async {
    try {
      var response = await AuthenticationService()
          .authenticationLoginSystem(username: username, password: password);
      _userInfo.setUserInfo(response);
      print(_userInfo.avatarPath);
      final prefs = await SharedPreferences.getInstance();
      prefs.setString('token', _userInfo.token);
      return true;
    } catch (e) {
      print(e.toString());
      return false;
    }
  }

  Future<void> checkAuthenticationLogin(
      {@required BuildContext context,
      @required String username,
      @required String password,
      String typeSystem}) async {
    if (username == '' || password == '') {
      notificationWidget(
        context,
        title: 'Lỗi Đăng Nhập',
        desc: 'Vui lòng nhập đầy đủ Username và Password',
        textButton: 'Cancel',
        typeNotication: AlertType.error,
      ).show();
      return;
    }
    Provider.of<Loading>(context, listen: false).setLoading();
    bool checkLogin =
        await checkLoginUserInfo(username: username, password: password);
    if (checkLogin) {
      Provider.of<Loading>(context, listen: false).setLoading();
      try {
        await SignalRService()
            .saveHubId(userInfo: _userInfo)
            .then((value) => print("saveHubId success"));
        isLogin = true;
        print(_userInfo.id);
        print(_userInfo.fullName);
      } catch (e) {
        print(e);
      }
      Navigator.pushNamed(context, HomePage.id);
    } else {
      Provider.of<Loading>(context, listen: false).setLoading();
      notificationWidget(
        context,
        title: 'Lỗi Đăng Nhập',
        desc: 'Username và Password không chính xác. Vui lòng đăng nhập lại',
        textButton: 'Cancel',
        typeNotication: AlertType.error,
      ).show();
    }
  }


  Map<String, String> getHeaders(String token) {
    return {HttpHeaders.authorizationHeader: 'Bearer $token'};
  }
}
