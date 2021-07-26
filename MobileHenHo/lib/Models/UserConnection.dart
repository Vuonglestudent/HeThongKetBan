import 'package:tinder_clone/Services/SignalRService.dart';

class IUser {
  String userId;
  String connectionId;
  String userName;
  String avatarPath;
  CallType callType;

  IUser({
    this.userId,
    this.connectionId,
    this.userName,
    this.avatarPath,
    this.callType,
  });

  getConnectionId() {
    return this.connectionId;
  }
}
