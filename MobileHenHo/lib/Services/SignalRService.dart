import 'dart:io';

import 'package:signalr_client/signalr_client.dart';
import 'package:tinder_clone/Models/UserInfo.dart';
import 'package:tinder_clone/Models/Internet.dart';
import 'package:http/http.dart' as http;

class SignalRService {
  final urlHub = 'http://10.0.2.2:5100/chatHub';
  //final urlHub = 'http://hieuit.tech:5100/chatHub';

  static String connectionId;
  HubConnection hubConnection;

  Internet internet = Internet(urlAPI: '/users');

  void startConnectHub() {
    print("start");
    hubConnection = HubConnectionBuilder().withUrl(urlHub).build();
    try {
      hubConnection
          .start()
          .then((value) => print('Connection started!'))
          .then((value) => getConnectionId());
    } catch (e) {
      print('Can not start connection with error: ' + e);
    }
  }

  Future<dynamic> saveHubId({UserInfo userInfo}) async {
    if (userInfo != null) {
      String url = internet.urlMain + '/hub?userId=${userInfo.id}';
      return await http.put(url, body: {
        'connectionId': connectionId
      }, headers: {
        HttpHeaders.authorizationHeader: 'Bearer ${userInfo.token}'
      });
    }
  }

  void getConnectionId() {
    hubConnection.invoke('getconnectionid').then((data) {
      connectionId = data;
      print(connectionId);
      saveHubId();
    });
  }
}
