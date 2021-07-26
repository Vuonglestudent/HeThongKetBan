import 'dart:convert';

import 'package:shared_preferences/shared_preferences.dart';
import 'package:tinder_clone/Models/Internet.dart';
import 'package:http/http.dart' as http;
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';

class MessageService {
  Internet _internetChats = new Internet(urlAPI: '/chats');

  Future<dynamic> getFriendList() async {
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    String userId = prefs.getString('userId');
    String url = _internetChats.urlMain + '/friends/$userId';

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

  Future<dynamic> getDataMessagesUser(
      int pageIndex, int pageSize, String receiverId) async {
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    String senderId = prefs.getString('userId');
    String query =
        '?PageIndex=$pageIndex&PageSize=$pageSize&SenderId=$senderId&ReceiverId=$receiverId';
    String url = _internetChats.urlMain + '/MoreMessages' + query;
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

  Future<dynamic> sendMessage(
    String destUserId,
    String content,
    List<http.MultipartFile> files,
  ) async {
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    String senderId = prefs.getString('userId');
    var header = AuthenticationProvider().getHeaders(token);
    http.MultipartRequest request =
        http.MultipartRequest("POST", Uri.parse(_internetChats.urlMain))
          ..fields['SenderId'] = senderId
          ..fields['ReceiverId'] = destUserId
          ..fields['Content'] = content;

    request.headers.addAll(header);
    for (http.MultipartFile file in files) {
      request.files.add(file);
    }

    return request.send();
    // print(senderId);
    // var data = {
    //   "SenderId": senderId,
    //   "ReceiverId": destUserId,
    //   "Content": content
    // };
    // print(data);
    // return await http
    //     .post(_internetChats.urlMain, body: data, headers: header)
    //     .then((http.Response response) {
    //   print('send messstatus code: ' + response.statusCode.toString());
    //   print('send messstatus body: ' + response.body.toString());
    //   if (response.statusCode < 200 ||
    //       response.statusCode >= 400 ||
    //       response.body == null) {
    //     return Exception(response.body);
    //   }
    //   return jsonDecode(response.body);
    // });
  }
}
