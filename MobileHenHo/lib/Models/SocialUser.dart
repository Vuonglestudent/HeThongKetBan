import 'package:flutter/cupertino.dart';

class SocialUser {
  final String provider;
  final String id;
  final String photoUrl;
  final String email;
  final String name;
  String token;
  String idToken;

  SocialUser({
    @required this.provider,
    @required this.id,
    @required this.photoUrl,
    @required this.email,
    @required this.name,
    this.token,
    this.idToken,
  });
}
