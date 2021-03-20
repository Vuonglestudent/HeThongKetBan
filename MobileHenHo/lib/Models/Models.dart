import 'package:tinder_clone/Models/FeatureDetail.dart';

class SocialUser {
  String provider;
  String id;
  String photoUrl;
  String email;
  String name;
  String token;
  String idToken;
}

class UserDisplay {
  String id;
  String fullName;
  String dob;
  DateTime createdAt;
  String avatarPath;
  bool hasAvatar;
  String summary;
  int numberOfFollowers;
  bool followed;
  int numberOfFavoritors;
  bool favorited;
  int numberOfImages;
  double point;
  String status;
}

class Message {
  int id;
  String senderId;
  String receiverId;
  String content;
  DateTime sentAt;
  String type;
  bool hasAvatar;
  String avatar;
  String fullName;
  int onlineCount;
}

class ChatFriend {
  UserDisplay user;
  List<Message> messages;
  int pageIndex;
}

// export class ProfileData {
//     gender: string[];
//     job: string[];
//     location: string[];
//     operationType: string[];
//     typeAccount: string[];
//     userStatus: string[];
//     ageGroup: string[];
// }




class ImageScore {
  bool active;
  bool autoFilter;
  int drawings;
  int hentai;
  int neutral;
  int porn;
  int sexy;
  DateTime updatedAt;
}
