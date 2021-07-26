import 'dart:convert';

import 'package:shared_preferences/shared_preferences.dart';
import 'package:tinder_clone/Models/Internet.dart';
import 'package:flutter/cupertino.dart';
import 'package:http/http.dart';
import 'package:http/http.dart' as http;
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';

class ImagesService {
  Internet _internetImage = new Internet(urlAPI: '/Images');
  Future<String> addImages({
    @required String userId,
    @required List<MultipartFile> fileImages,
    @required Map<String, String> token,
    @required String titleImages,
  }) async {
    MultipartRequest request =
        http.MultipartRequest("POST", Uri.parse(_internetImage.urlMain))
          ..fields['UserId'] = userId
          ..fields['Title'] = titleImages;
    request.headers.addAll(token);
    for (MultipartFile image in fileImages) {
      request.files.add(image);
    }
    print(request.fields);
    for (var test in request.files) {
      print(test.contentType);
      print(test.filename);
      print(test.field);
    }
    print(request.toString());

    var response = await request.send();
    if (response.statusCode == 200) {
      final responseString = await response.stream.bytesToString();
      return responseString;
    } else
      return null;
  }

  Future<dynamic> getImagesByUserId(String userId) async {
    String url =
        _internetImage.urlMain + '/user/$userId?pageIndex=${1}&pageSize=${30}';
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
}
