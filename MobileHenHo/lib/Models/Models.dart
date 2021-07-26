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
