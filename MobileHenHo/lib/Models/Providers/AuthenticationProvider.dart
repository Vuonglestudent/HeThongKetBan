import 'dart:async';
import 'dart:io';
import 'package:flutter/cupertino.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter_facebook_login/flutter_facebook_login.dart';
import 'package:geolocator/geolocator.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:tinder_clone/HomePage.dart';
import 'package:tinder_clone/Models/Providers/MessageProvider.dart';
import 'package:tinder_clone/Models/Providers/NotificationsProvider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Models/SocialUser.dart';
import 'package:tinder_clone/Models/User.dart';
import 'package:tinder_clone/Models/UserInfo.dart' as model;
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Services/AuthenticationService.dart';
import 'package:tinder_clone/Services/UsersService.dart';
import 'package:tinder_clone/Widgets/Notification.dart';
import 'package:tinder_clone/Services/SignalRService.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter_facebook_login/flutter_facebook_login.dart' as face;
import 'package:provider/provider.dart';

class AuthenticationProvider extends ChangeNotifier {
  //Login with Face
  final _auth = FirebaseAuth.instance;
  final _facebooklogin = face.FacebookLogin();

  model.UserInfo _userInfo;
  SignalRService _signalRService;
  bool isCalling = false;
  bool isSaveLocation = false;
  List<UsersSimilar> _blockList = [];
  List<UsersSimilar> get blockList {
    return _blockList;
  }

  model.UserInfo get getUserInfo {
    return _userInfo;
  }

  SignalRService get signalR {
    return _signalRService;
  }

  void logoutSystem(BuildContext context) async {
    final prefs = await SharedPreferences.getInstance();
    AuthenticationService().logoutSystem();
    _userInfo = null;
    _signalRService.closeConnect();
    Provider.of<NotificationsProvider>(context, listen: false)
        .resetNotification();
    Provider.of<MessageProvider>(context, listen: false).resetDataMessage();
    Provider.of<UserProvider>(context, listen: false).resetFriends();
    logout();
    prefs.clear();
  }

  Future<bool> checkLoginUserInfo(dynamic response) async {
    try {
      if (response != null) {
        _userInfo = model.UserInfo();
        _signalRService = SignalRService();
        _userInfo.setUserInfo(response);
        print(_userInfo);
        final prefs = await SharedPreferences.getInstance();
        prefs.setString('token', _userInfo.token);
        prefs.setString('userId', _userInfo.id);
        return true;
      } else
        return false;
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
    var response = await AuthenticationService()
        .authenticationLoginSystem(username: username, password: password);
    bool checkLogin = await checkLoginUserInfo(response);
    if (checkLogin) {
      Provider.of<Loading>(context, listen: false).setLoading();
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

  Future<bool> changePassword(
      String oldPass, String newPass, String againPass) async {
    try {
      return await UserService()
          .changePassword(_userInfo.email, oldPass, newPass, againPass);
    } catch (e) {
      return false;
    }
  }

  Future<bool> getBlockList() async {
    try {
      _blockList = [];
      var response = await UserService().getBlockList();
      for (Map<String, dynamic> item in response) {
        UsersSimilar newUser = new UsersSimilar();
        newUser.setUserSimilar(item);
        _blockList.add(newUser);
      }
      return true;
    } catch (e) {
      print('error load block list: ' + e.toString());
      return false;
    }
  }

  Map<String, String> getHeaders(String token) {
    return {HttpHeaders.authorizationHeader: 'Bearer $token'};
  }

  Future<void> determinePosition() async {
    bool serviceEnabled;
    LocationPermission permission;

    serviceEnabled = await Geolocator.isLocationServiceEnabled();
    if (!serviceEnabled) {
      return Future.error('Location services are disabled.');
    }

    permission = await Geolocator.checkPermission();
    if (permission == LocationPermission.deniedForever) {
      return Future.error(
          'Location permissions are permantly denied, we cannot request permissions.');
    }

    if (permission == LocationPermission.denied) {
      permission = await Geolocator.requestPermission();
      if (permission != LocationPermission.whileInUse &&
          permission != LocationPermission.always) {
        return Future.error(
            'Location permissions are denied (actual value: $permission).');
      }
    }

    StreamSubscription<Position> positionStream = Geolocator.getPositionStream(
      desiredAccuracy: LocationAccuracy.medium,
      distanceFilter: 2,
      timeLimit: Duration(minutes: 3),
    ).listen((Position position) async {
      //print('save point');
      print('latitude: ${position.latitude}, longitude: ${position.longitude}');
      if (_userInfo != null)
        await UserService()
            .savePointLocation(position.latitude, position.longitude)
            .then((value) {
          print(value['message']);
          isSaveLocation = true;
        });
    });
  }

  Future<bool> loginWithFacebook() async {
    _facebooklogin.loginBehavior = FacebookLoginBehavior.webViewOnly;
    final result = await _facebooklogin.logIn(['email']);
    if (result.status == face.FacebookLoginStatus.loggedIn) {
      final credential = FacebookAuthProvider.getCredential(
        accessToken: result.accessToken.token,
      );

      final user = (await _auth.signInWithCredential(credential)).user;
      SocialUser socialUser = new SocialUser(
        provider: "FACEBOOK",
        photoUrl: user.photoUrl,
        id: user.uid,
        email: user.email,
        name: user.displayName,
      );

      return await AuthenticationService().loginFacebook(socialUser).then(
          (value) async =>
              await checkLoginUserInfo(value).then((value) => value));
    }
  }

  Future logout() async {
    // SignOut khỏi Firebase Auth
    if (_auth != null) await _auth.signOut();
    // Logout facebook
    if (_facebooklogin != null) await _facebooklogin.logOut();
  }

  void openBlockUser(
    BuildContext context,
    UserProvider user,
    NotificationsProvider notification,
    int index,
  ) async {
    await user.blockUser(_blockList[index].id).then(
      (r) {
        print(r);
        if (r) {
          notification.showNotification(user.getUserProfile.fullName,
              "và bạn đã có thể tìm thấy nhau", user.getUserProfile.id);
          _blockList.removeAt(index);
          notifyListeners();
        } else {
          notification.showNotification(user.getUserProfile.fullName,
              "không thể được mở khóa lúc này", user.getUserProfile.id);
        }
      },
    );
  }
}
