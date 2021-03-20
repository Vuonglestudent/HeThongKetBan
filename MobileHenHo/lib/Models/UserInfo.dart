class UserInfo {
  String id;
  String username;
  String fullName;
  String email;
  String token;
  String role;
  bool isInfoUpdated;
  bool hasAvatar;
  String avatarPath;

  void setUserInfo(Map<String, dynamic> response) {
    id = response['id'];
    username = response['userName'];
    fullName = response['fullName'];
    email = response['email'];
    token = response['token'];
    role = response['role'];
    isInfoUpdated = response['isInfoUpdated'];
    hasAvatar = response['hasAvatar'];
    avatarPath = response['avatarPath'];
  }
}
