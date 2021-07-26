import 'package:tinder_clone/Models/Message.dart';
import 'package:tinder_clone/Models/User.dart';

class ChatFriend {
  UsersSimilar _user = new UsersSimilar();
  List<Message> _messages = [];
  int _pageIndex;

  void setChatFriends(dynamic response) {
    _user.setUserSimilar(response['user']);
    for (var item in response['messages']) {
      Message newMessage = new Message();
      newMessage.setChatFriendMessage(item);
      _messages.add(newMessage);
    }
    _pageIndex = 1;
  }

  void setChatFriendAtTimeFirst(Message response) {
    _user.setUserAtTime(response);
    Message newMessage = new Message();
    newMessage.setMessageItem(response);
    _messages.add(newMessage);
  }

  void resetMessagesUser() {
    _messages = [];
  }

  void setMessagesUser(List<dynamic> response) {
    for (var item in response) {
      Message newItem = new Message();
      newItem.setChatFriendMessage(item);
      //print(newItem.content);
      _messages.add(newItem);
    }
  }

  void pushMessageListenHubInChat(Message message) {
    print('listenMessageHub: ${message.content}');
    // print("=====");
    // for (Message item in _messages) {
    //   print(item.content);
    // }
    Message item = new Message();
    item.setMessageItem(message);
    _messages.insert(0, item);
    //print("lenght messages: ${_messages.length}");
  }

  UsersSimilar get userInfo {
    return _user;
  }

  String get firstMessage {
    return _messages.first.content;
  }

  bool get firstSeenMessage {
    return _messages.first.isSeen;
  }

  String get firstSendAt {
    return _messages.first.sentAt;
  }

  List<Message> get messagesUser {
    return _messages;
  }

  int get pageIndex {
    return _pageIndex;
  }
}
