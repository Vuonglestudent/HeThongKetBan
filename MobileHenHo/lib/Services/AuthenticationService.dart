import 'package:flutter/cupertino.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:tinder_clone/Models/Internet.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/SocialUser.dart';

class AuthenticationService {
  Internet internet = new Internet(urlAPI: '/Authenticates');

  Future<dynamic> authenticationLoginSystem(
      {@required username, @required password}) async {
    var url = internet.urlMain + '/login';
    var data = {'Email': username, 'Password': password};
    print(data);
    return await http.post(url, body: data).then((http.Response response) {
      print(response.statusCode);
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return Exception(response.body);
      }
      return jsonDecode(response.body);
    });
  }

  Future<void> logoutSystem() async {
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    var url = internet.urlMain + '/logout';
    return await http.post(url, headers: header);
  }

  Future<dynamic> loginFacebook(SocialUser account) async {
    String url = internet.urlMain + '/social';
    var str = account.photoUrl;
    var newStr;
    if (account.provider == "FACEBOOK") {
      var regEx = new RegExp(r"normal", caseSensitive: false);
      newStr = str.replaceAll(regEx, "large");
    }
    var body = {
      "Avatar": newStr,
      "Email": account.email,
      "FullName": account.name,
      "Provider": account.provider,
    };

    return await http.post(url, body: body).then((http.Response response) {
      print(response.statusCode);
      print(response.body);
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return Exception(response.body);
      }
      return jsonDecode(response.body);
    });
  }
}
