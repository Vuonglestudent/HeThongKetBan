import 'package:flutter/foundation.dart';
import 'package:multi_image_picker/multi_image_picker.dart';
import 'package:tinder_clone/Models/ChatFriend.dart';
import 'package:tinder_clone/Models/Message.dart';
import 'package:tinder_clone/Models/Providers/NotificationsProvider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Services/MessageService.dart';
import 'package:tinder_clone/Services/UsersService.dart';

class MessageProvider extends ChangeNotifier {
  List<ChatFriend> _friendList = [];
  List<ChatFriend> _saveStateFriendList = [];
  int _pageSize = 200000;
  bool isExistScreenMessage = false;
  ChatFriend userChatCurrent;
  Future<bool> getFriendList() async {
    try {
      if (_friendList.length == 0) {
        await MessageService().getFriendList().then((response) {
          if (response.length != _friendList.length) {
            for (var item in response) {
              ChatFriend newItem = ChatFriend();
              newItem.setChatFriends(item);
              _friendList.removeWhere(
                  (element) => element.userInfo.id == newItem.userInfo.id);
              _saveStateFriendList.removeWhere(
                  (element) => element.userInfo.id == newItem.userInfo.id);
              _friendList.add(newItem);
              _saveStateFriendList.add(newItem);
            }
          }
          print('lenght friend chat: ' + _friendList.length.toString());
        });
      }
      return true;
    } catch (e) {
      print("Error Load FriendList: " + e.toString());
      return false;
    }
  }

  List<ChatFriend> get friendList {
    return _friendList;
  }

  Future<ChatFriend> findUserById(String id) async {
    try {
      //if (_friendList.length == 0) {
      return await getFriendList().then((value) {
        return _friendList
                .where((element) => element.userInfo.id == id)
                .isNotEmpty
            ? _friendList.firstWhere((element) => element.userInfo.id == id)
            : new ChatFriend();
      });
      // } else {
      //   return _friendList.firstWhere((element) => element.userInfo.id == id) ??
      //       new ChatFriend();
      // }
    } catch (e) {
      print('error get UserInfo chat: ' + e.toString());
      return new ChatFriend();
    }
  }

  Future<bool> getDataMessagesUser(String friendId, int pageIndex) async {
    try {
      userChatCurrent = await findUserById(friendId);
      if (userChatCurrent.userInfo.id != null) {
        _friendList.removeWhere((element) => element.userInfo.id == friendId);
        userChatCurrent.resetMessagesUser();
        print('Seen info user: ' + userChatCurrent.userInfo.fullName);
        var response = await MessageService()
            .getDataMessagesUser(pageIndex, _pageSize, friendId);
        userChatCurrent.setMessagesUser(response);
      } else {
        var response = await UserService().getUserById(friendId);
        userChatCurrent.userInfo.setUserSimilarMessage(response);
        print('new user: ' + userChatCurrent.userInfo.fullName);
      }
      isExistScreenMessage = true;
      return true;
    } catch (e) {
      print('Error setUserChat: ' + e.toString());
      return false;
    }
  }

  void resetDataMessage() {
    _friendList = [];
    _saveStateFriendList = [];
    isExistScreenMessage = false;
    userChatCurrent = new ChatFriend();
  }

  void saveUserWhenExistChat() {
    if (userChatCurrent != null) {
      _friendList.insert(0, userChatCurrent);
      userChatCurrent = null;
      isExistScreenMessage = false;
      notifyListeners();
    }
  }

  Future<void> sendMessage(
    String destUserId,
    String content,
    List<Asset> files,
  ) async {
    if (destUserId != '' && content != '') {
      try {
        if (content == 'file' && files.length != 0) {
          await UserProvider().sendImageChat(
              destUserId: destUserId, content: content, assets: files);
        } else
          await MessageService().sendMessage(destUserId, content, []);
      } catch (e) {
        print('error SendMessage: ' + e.toString());
      }
    }
  }

  void listenMessageSignalRInChat(String userLocalId, Message message) async {
    if (message != null) {
      if (message.senderId == userLocalId) {
        print('sender');
        message.setType('sent');
        try {
          userChatCurrent.pushMessageListenHubInChat(message);
        } catch (e) {
          print('error send mess: ' + e.toString());
        }
      } else if (message.receiverId == userLocalId) {
        if (!isExistScreenMessage) {
          NotificationsProvider().showNotification(
              message.fullName,
              'đã nhắn tin: ${message.content}',
              'chat_${message.senderId}_${message.fullName}_${message.avatar}');
          return;
        }
        print('received');
        message.setType('received');
        message.isSeen = true;
        try {
          if (userChatCurrent != null &&
              message.senderId == userChatCurrent.userInfo.id) {
            userChatCurrent.pushMessageListenHubInChat(message);
          } else {
            ChatFriend newUser = await findUserById(message.senderId);
            if (newUser == null) {
              newUser = new ChatFriend();
              newUser.setChatFriendAtTimeFirst(message);
            } else {
              newUser.pushMessageListenHubInChat(message);
            }
            _friendList.removeWhere(
                (element) => element.userInfo.id == newUser.userInfo.id);
            _friendList.insert(0, newUser);
          }
          print(_friendList.length);
        } catch (e) {
          print('error listen Message: ' + e.toString());
        }
      }
      notifyListeners();
    }
  }

  int getFriendsIndex(String userId) {
    int value = -1;
    if (_friendList.length > 0)
      for (int i = 0; i < _friendList.length; i++) {
        if (_friendList[i].userInfo.id == userId) {
          value = i;
        }
      }
    return value;
  }

  void searchFriendMessage(String strSearch) {
    print(strSearch);
    if (strSearch != "") {
      for (ChatFriend user in _friendList) {
        List<String> stringName = [];
        stringName = user.userInfo.fullName.split(RegExp(" ")).toList();
        for (String str in stringName) {
          if (str.toUpperCase().contains(strSearch.toUpperCase())) {
            _friendList.remove(user);
            _friendList.insert(0, user);
          }
        }
      }
    } else {
      _friendList = [];
      for (int i = 0; i < _saveStateFriendList.length; i++) {
        _friendList.add(_saveStateFriendList[i]);
      }
    }
    notifyListeners();
  }
}
