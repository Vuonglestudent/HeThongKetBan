import 'dart:convert';
import 'dart:io';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:tinder_clone/Models/Internet.dart';
import 'package:http/http.dart' as http;
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/User.dart';

class UserService {
  Internet _internetUser = new Internet(urlAPI: '/users');
  Internet _internetProfiles = new Internet(urlAPI: '/Profiles');

  Future<dynamic> getUserById(String userId) async {
    String url = _internetUser.urlMain + '/$userId';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    return await http.get(url, headers: header).then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return Exception(response.body);
      }
      return jsonDecode(response.body);
    });
  }

  Future<dynamic> getProfileData() async {
    String url = _internetProfiles.urlMain + '/features';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    return await http.get(url, headers: header).then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return Exception(response.body);
      }
      return jsonDecode(response.body);
    });
  }

  Future<dynamic> updateUserProfile(User newUserProfile) async {
    Map<String, dynamic> data = newUserProfile.getUserJson();
    data['location'] = data['location'].toString().replaceAll(RegExp(" "), "_");
    data['job'] = data['job'].toString().replaceAll(RegExp(" "), "_");
    data['findAgeGroup'] =
        data['findAgeGroup'].toString().replaceAll(RegExp(" "), "_");

    //print(data);
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    //print(header);
    return await http
        .put(_internetProfiles.urlMain, body: json.encode(data), headers: {
      HttpHeaders.authorizationHeader: 'Bearer $token',
      "Content-type": "application/json"
    }).then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return Exception(response.body);
      }
      return jsonDecode(response.body);
    });
  }
}
