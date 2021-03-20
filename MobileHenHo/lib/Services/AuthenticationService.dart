import 'package:flutter/cupertino.dart';
import 'package:tinder_clone/Models/Internet.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

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
}
