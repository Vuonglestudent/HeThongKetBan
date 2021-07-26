import 'dart:convert';
import 'dart:io';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:tinder_clone/Models/IFindAround.dart';
import 'package:tinder_clone/Models/Internet.dart';
import 'package:http/http.dart' as http;
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/User.dart';

class UserService {
  Internet _internetUser = new Internet(urlAPI: '/users');
  Internet _internetProfiles = new Internet(urlAPI: '/Profiles');

  Future<Map<String, dynamic>> getUsersSimilar(
      String userId, String pageIndex, String pageSize) async {
    String url =
        '${_internetProfiles.urlMain}/similar/$userId?pageIndex=$pageIndex&pageSize=$pageSize';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    return await http.get(url, headers: header).then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return {"e": Exception(response.body)};
      }
      return jsonDecode(response.body);
    });
  }

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

  Future<dynamic> updateAvatar(
    String userId,
    http.MultipartFile fileImages,
  ) async {
    var url = _internetUser.urlMain + '/avatar';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    http.MultipartRequest request = http.MultipartRequest("PUT", Uri.parse(url))
      ..fields['UserId'] = userId;
    request.headers.addAll(header);

    request.files.add(fileImages);

    return await request.send();
  }

  Future<List<dynamic>> getFriendsUser(
      String pageIndex, String pageSize) async {
    final prefs = await SharedPreferences.getInstance();
    String userId = prefs.getString('userId');
    String url = _internetUser.urlMain +
        '/follow/$userId?pageIndex=$pageIndex&pageSize=$pageSize';
    final token = prefs.getString('token');
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

  Future<List<dynamic>> searchFriendByName(String name) async {
    String url = _internetUser.urlMain + '/friends?name=$name';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
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

  Future<dynamic> setFavorite(String userId) async {
    String url = _internetUser.urlMain + '/favorite';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    var body = {'userId': userId};
    return await http
        .post(url, body: body, headers: header)
        .then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return Exception(response.body);
      }
      return jsonDecode(response.body);
    });
  }

  Future<dynamic> setFollow(String userId) async {
    String url = _internetUser.urlMain + '/follow';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    var body = {'userId': userId};
    return await http
        .post(url, body: body, headers: header)
        .then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return Exception(response.body);
      }
      return jsonDecode(response.body);
    });
  }

  Future<dynamic> savePointLocation(double latitude, double longitude) async {
    var url = _internetUser.urlMain + '/position';
    var prefs = await SharedPreferences.getInstance();
    var token = prefs.getString('token');
    var userId = prefs.getString('userId');
    var body = jsonEncode({
      'userId': userId,
      'latitude': latitude.toString(),
      'longitude': longitude.toString(),
    });
    return await http.put(url, body: body, headers: {
      HttpHeaders.authorizationHeader: 'Bearer $token',
      'Content-Type': 'application/json',
      'accept': 'application/json',
    }).then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return Exception(response.body);
      }
      return jsonDecode(response.body);
    });
  }

  Future<dynamic> findAround(IFindAround iFindAround) async {
    var url = _internetUser.urlMain + '/around';
    var prefs = await SharedPreferences.getInstance();
    var token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    var body = jsonEncode({
      'pageIndex': iFindAround.pageIndex,
      'pageSize': iFindAround.pageSize,
      'userId': iFindAround.userId,
      'distance': iFindAround.distance,
      'gender': iFindAround.gender,
      'ageGroup': iFindAround.ageGroup,
    });
    return await http.post(url, body: body, headers: {
      HttpHeaders.authorizationHeader: 'Bearer $token',
      'Content-Type': 'application/json',
      'accept': 'application/json',
    }).then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return Exception(response.body);
      }
      return jsonDecode(response.body);
    });
  }

  Future<dynamic> changePassword(
      String email, String oldPass, String newPass, String againPass) async {
    var url = _internetUser.urlMain + '/changePassword';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    var body = {
      "Email": email,
      "OldPassword": oldPass,
      "NewPassword": newPass,
      "ConfirmPassword": againPass
    };
    return await http
        .put(url, body: body, headers: header)
        .then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return false;
      }
      return true;
    });
  }

  Future<dynamic> getBlockList() async {
    var url = _internetUser.urlMain + '/blackList';
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

  Future<dynamic> blockUser(String userId) async {
    var url = _internetUser.urlMain + '/blackList/$userId';
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token');
    var header = AuthenticationProvider().getHeaders(token);
    var body = {};
    return await http
        .post(url, body: body, headers: header)
        .then((http.Response response) {
      if (response.statusCode < 200 ||
          response.statusCode >= 400 ||
          response.body == null) {
        return Exception(response.body);
      }
      return jsonDecode(response.body);
    });
  }
}
