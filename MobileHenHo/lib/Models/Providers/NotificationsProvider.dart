import 'dart:math';

import 'package:flutter/foundation.dart';
import 'package:flutter/services.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';
import 'package:rxdart/rxdart.dart';
import 'package:tinder_clone/Models/INotification.dart';
import 'package:tinder_clone/Models/IRelationship.dart';
import 'package:tinder_clone/Services/NotificationsService.dart';
import 'package:tinder_clone/Services/RelationshipService.dart';
import 'package:tinder_clone/main.dart';

String selectedNotificationPayload;

class NotificationsProvider extends ChangeNotifier {
  List<INotification> _listNotification = [];
  IRelationship _relationship = new IRelationship();

  int _pageIndex = 0;
  int _pageSize = 8;

  Future<bool> getNotification() async {
    try {
      bool get = nextPageIndex();
      // if (_pageIndex == 1) {
      //   _listNotification = [];
      // }
      if (get) {
        var response = await NotificationsService()
            .getNotifications(this._pageIndex, this._pageSize);
        if (response.isNotEmpty) {
          response.forEach((i) {
            INotification newItem = INotification(
              id: i['id'],
              fromId: i['fromId'],
              fullName: i['fullName'],
              toId: i['toId'],
              type: i['type'],
              createdAt: DateTime.parse(i['createdAt']),
              content: i['content'],
              avatar: i['avatar'],
            );
            _listNotification
                .removeWhere((element) => element.id == newItem.id);
            _listNotification.add(newItem);
          });
          if (_pageIndex != 1) notifyListeners();
        }
      }
      return true;
    } catch (error) {
      print(error.toString());
      return false;
    }
  }

  bool nextPageIndex() {
    if (_listNotification.length == 0) {
      _pageIndex = 1;
      return true;
    } else {
      int page = (_listNotification.length ~/ 8).toInt();
      if (page + 1 > _pageIndex) {
        _pageIndex = page + 1;
        return true;
      }
    }
    return false;
  }

  void resetNotification() {
    _listNotification = [];
    _relationship = new IRelationship();
  }

  void listenNotificationHub(INotification notification, bool isNoti) async {
    var check = _listNotification.where((e) => e.id == notification.id).isEmpty;
    if (check) {
      if (isNoti)
        showNotification(notification.fullName, notification.content,
            'tab3_${notification.fromId}');
      if (_listNotification.length == 0) {
        await getNotification();
      } else
        _listNotification.insert(0, notification);
      notifyListeners();
    }
  }

  void onAccept(String fromId, int index) async {
    try {
      await getRelationship(fromId);
      if (_relationship.id != null) {
        var response =
            await RelationshipService().onAcceptRelationship(_relationship.id);
        if (response) {
          showNotification(
              "Bạn",
              "đã có mối quan hệ mới với ${_relationship.fromName}, hãy cố gắng nhé!",
              '');
          _listNotification = [];
          await getNotification();
        } else {
          showNotification("Bạn", "không thể chấp nhận lúc này", '');
        }
      }
      notifyListeners();
    } catch (e) {
      print('onAccept Relationship error: ' + e.toString());
    }
  }

  void onDecline(String fromId, index) async {
    try {
      await getRelationship(fromId);
      if (_relationship.id != null) {
        var response = await RelationshipService()
            .onDeclinetRelationship(_relationship.id);
        if (response) {
          showNotification("Bạn",
              "đã từ chối mối quan hệ mới với ${_relationship.fromName}", '');
          _listNotification = [];
          await getNotification();
        } else {
          showNotification("Bạn", "không thể từ chối lúc này", '');
        }
      }
      notifyListeners();
    } catch (e) {
      print('onAccept Relationship error: ' + e.toString());
    }
  }

  Future<void> deleteNotification(bool response, int index) async {
    try {
      //var response =
      // await NotificationsService().deleteNotification(_relationship.id);
      //print(response);
      if (response) {
        _listNotification.removeAt(index);
        print("remove success");
      } else {
        print("Bạn không thể xóa notification");
      }
      notifyListeners();
    } catch (e) {
      print("error deleteNotifi: " + e.toString());
    }
  }

  Future<void> getRelationship(String fromId) async {
    try {
      var r = await RelationshipService().getRelationshipById(fromId);
      if (r != null) {
        _relationship = new IRelationship(
          id: int.parse(r['id'].toString()),
          fromId: r['fromId'],
          toId: r['toId'],
          relationshipType: RelationshipType.values[r['relationshipType']],
          createdAt: DateTime.parse(r['createdAt']),
          updatedAt: DateTime.parse(r['updatedAt']),
          fromName: r['fromName'],
          toName: r['toName'],
          fromAvatar: r['fromAvatar'],
          toAvatar: r['toAvatar'],
        );
      }
    } catch (e) {
      print('getRelationship error: ' + e.toString());
    }
  }

  /// Cách dạng thông báo
  Future<void> showNotification(
      String fullName, String content, String userId) async {
    const AndroidNotificationDetails androidPlatformChannelSpecifics =
        AndroidNotificationDetails(
      'CHANNEL_ID',
      'CHANNEL_NAME',
      'CHANGE_DESCRIPSTION',
      importance: Importance.max,
      priority: Priority.high,
      playSound: true,
      ticker: 'ticker',
    );
    const NotificationDetails platformChannelSpecifics =
        NotificationDetails(android: androidPlatformChannelSpecifics);
    await flutterLocalNotificationsPlugin.show(
        0, '$fullName', '$content', platformChannelSpecifics,
        payload: '$userId');
  }

  //getData
  List<INotification> get listNotifications {
    return _listNotification;
  }
}
