import 'dart:convert';
import 'dart:io';

import 'package:shared_preferences/shared_preferences.dart';
import 'package:tinder_clone/Models/Internet.dart';
import 'package:http/http.dart' as http;
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';

class RelationshipService {
  Internet _internetRelationship = new Internet(urlAPI: '/Relationships');

  Future<dynamic> getRelationshipById(String id) async {
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    String toId = prefs.getString('userId');

    var header = AuthenticationProvider().getHeaders(token);
    var url = _internetRelationship.urlMain + '?fromId=$id&toId=$toId';

    return await http.get(url, headers: header).then((http.Response response) {
      print("body relationship: " + response.body);
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return Exception(response.body);
      }
      return jsonDecode(response.body);
    });
  }

  Future<bool> onAcceptRelationship(int id) async {
    var url = _internetRelationship.urlMain + '/accept/$id';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    return await http.post(url, headers: header).then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return false;
      }
      return true;
    });
  }

  Future<bool> onDeclinetRelationship(int id) async {
    var url = _internetRelationship.urlMain + '/decline/$id';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    return await http.post(url, headers: header).then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return false;
      }
      return true;
    });
  }

  Future<dynamic> saveRelationship(String toId, int indexRelationship) async {
    var url = _internetRelationship.urlMain;
    var prefs = await SharedPreferences.getInstance();
    var token = prefs.getString('token');
    var userId = prefs.getString('userId');

    var body = jsonEncode({
      'fromId': userId,
      'toId': toId,
      'relationShipType': indexRelationship,
    });

    return await http.post(url, body: body, headers: {
      HttpHeaders.authorizationHeader: 'Bearer $token',
      'Content-Type': 'application/json',
      'accept': 'application/json',
    }).then((http.Response response) {
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
