import 'dart:convert';

import 'package:shared_preferences/shared_preferences.dart';
import 'package:tinder_clone/Models/Internet.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:http/http.dart' as http;

class NotificationsService {
  Internet _internetNotifications = new Internet(urlAPI: '/Notifications');

  Future<List<dynamic>> getNotifications(int pageIndex, int pageSize) async {
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    String userId = prefs.getString('userId');
    var url = _internetNotifications.urlMain +
        '/$userId' +
        '?pageIndex=$pageIndex&pageSize=$pageSize';
    var header = AuthenticationProvider().getHeaders(token);
    return await http.get(url, headers: header).then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return [
          {'e': Exception(response.body)}
        ];
      }
      return jsonDecode(response.body);
    });
  }

  Future<bool> deleteNotification(int id) async {
    var url = _internetNotifications.urlMain + '/$id';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    return await http
        .delete(url, headers: header)
        .then((http.Response response) {
      print("body delete relation ship:" + response.body);
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return false;
      }
      return true;
    });
  }
}
