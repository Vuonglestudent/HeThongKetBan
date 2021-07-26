import 'package:intl/intl.dart';

class Message {
  int _id;
  String _senderId;
  String _receiverId;
  String _content;
  DateTime _sentAt;
  String _type;
  String _avatar;
  String _fullName;
  int _onlineCount;
  //
  //String fromId;
  //String toId;
  //String createAt;
  String _messageType;
  //String filePath;
  List<String> _filePaths;
  bool isSeen = false;

  void setMessageItem(Message item) {
    _filePaths = [];
    _id = item._id;
    _senderId = item._senderId;
    _receiverId = item._receiverId;
    _content = item._content;
    _sentAt = item._sentAt;
    _type = item._type;
    _avatar = item._avatar;
    _fullName = item._fullName;
    _onlineCount = item._onlineCount;
    _messageType = item._messageType;
    _filePaths = item._filePaths;
    isSeen = item.isSeen;
  }

  void setChatFriendMessage(dynamic response) {
    _filePaths = [];
    _id = response['id'];
    _senderId = response['senderId'];
    _receiverId = response['receiverId'];
    _content = response['content'];
    _type = response['type'] ?? '0';
    _sentAt = DateTime.parse(response['sentAt']);
    _fullName = response['fullName'];
    _avatar = response['avatar'];
    _messageType = response['messageType'];
    for (var res in response['filePaths']) {
      String img = res;
      _filePaths.add(img);
    }
  }

  int get idMessage {
    return _id;
  }

  String get fullName {
    return _fullName;
  }

  String get avatar {
    return _avatar;
  }

  String get firstMessage {
    return _content;
  }

  String get type {
    return _type;
  }

  String setType(String type) {
    this._type = type;
  }

  String get receiverId {
    return _receiverId;
  }

  String get senderId {
    return _senderId;
  }

  String get content {
    return _content;
  }

  String get messageType {
    return _messageType;
  }

  List<String> get filePaths {
    return _filePaths;
  }

  String get sentAt {
    String day = DateFormat('hh:mm - dd.MMM').format(_sentAt);
    return '$day';
  }
}
