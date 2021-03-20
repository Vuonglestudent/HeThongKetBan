import 'package:smart_select/smart_select.dart';
import 'package:tinder_clone/Models/Feature.dart';
import 'package:tinder_clone/Models/FeatureVM.dart';
import 'package:tinder_clone/Models/ProfileData.dart';
import 'package:json_annotation/json_annotation.dart';

class User {
  String id;
  String role;
  String userName;
  String fullName;
  String gender;
  String avatarPath;
  String status;
  String email;
  String phoneNumber;
  double point;
  bool hasAvatar;
  String summary;
  int numberOfFollowers;
  int numberOfFavoritors;
  int numberOfImages;
  bool followed;
  bool favorited;
  bool blocked;
  int age;
  bool isInfoUpdated;
  String token;
  String title;
  int weight;
  int height;
  String dob;
  String job;
  String location;
  String findPeople;
  String findAgeGroup;
  List<FeatureVM> features;
  List<FeatureVM> searchFeatures;
  List<FeatureVM> featureVMTrue;
  List<FeatureVM> featureVMFalse;

  void setUser(Map<String, dynamic> response) {
    id = response['id'];
    role = response['role'];
    userName = response['userName'];
    fullName = response['fullName'];
    gender = response['gender'];
    avatarPath = response['avatarPath'];
    status = response['status'];
    email = response['email'];
    phoneNumber = response['phoneNumber'];
    point = response['point'];
    hasAvatar = response['hasAvatar'];
    summary = response['summary'];
    numberOfFollowers = response['numberOfFollowers'];
    numberOfFavoritors = response['numberOfFavoritors'];
    numberOfImages = response['numberOfImages'];
    followed = response['followed'];
    favorited = response['favorited'];
    blocked = response['blocked'];
    age = response['age'].runtimeType == int
        ? response['age']
        : int.parse(response['age']);
    isInfoUpdated = response['isInfoUpdated'];
    token = response['token'];
    title = response['title'];
    weight = response['weight'].runtimeType == int
        ? response['weight']
        : int.parse(response['weight']);
    height = response['height'].runtimeType == int
        ? response['height']
        : int.parse(response['height']);
    dob = response['dob'];
    job = response['job'];
    location = response['location'];
    findPeople = response['findPeople'];
    findAgeGroup = response['findAgeGroup'];
    features = [];
    searchFeatures = [];
    if (response['features'] != null && response['searchFeatures'] != null) {
      features = [];
      searchFeatures = [];
      for (Map<String, dynamic> data in response['features']) {
        FeatureVM addData = new FeatureVM();
        addData.setFeatureVM(data);
        features.add(addData);
      }
      for (var data in response['searchFeatures']) {
        FeatureVM addData = new FeatureVM();
        addData.setFeatureVM(data);
        searchFeatures.add(addData);
      }
    }
  }

  Map<String, dynamic> getUserJson() {
    Map<String, dynamic> data = {
      'id': this.id,
      'role': this.role,
      'userName': this.userName,
      'fullName': this.fullName,
      'gender': this.gender,
      'avatarPath': this.avatarPath,
      'status': this.status,
      'email': this.email,
      'phoneNumber': this.phoneNumber,
      'point': this.point,
      'hasAvatar': this.hasAvatar,
      'summary': this.summary,
      'numberOfFollowers': this.numberOfFollowers,
      'numberOfFavoritors': this.numberOfFavoritors,
      'numberOfImages': this.numberOfImages,
      'followed': this.followed,
      'favorited': this.favorited,
      'blocked': this.blocked,
      'age': this.age,
      'isInfoUpdated': this.isInfoUpdated,
      'token': this.token,
      'title': this.title,
      'weight': this.weight,
      'height': this.height,
      'dob': this.dob,
      'job': this.job,
      'location': this.location,
      'findPeople': this.findPeople,
      'findAgeGroup': this.findAgeGroup,
    };
    List<Map<String, dynamic>> array = [];
    for (FeatureVM item in features) {
      Map<String, dynamic> child = item.getFeatureVMJson;
      array.add(child);
    }
    data['features'] = array;
    array = [];
    for (FeatureVM item in searchFeatures) {
      Map<String, dynamic> child = item.getFeatureVMJson;
      array.add(child);
    }
    data['searchFeatures'] = array;
    return data;
  }

  List<FeatureVM> get getfeatureVMFalse {
    featureVMFalse = [];
    for (FeatureVM feature in features) {
      if (!feature.isSearchFeature) {
        featureVMFalse.add(feature);
      }
    }
    return featureVMFalse;
  }

  List<FeatureVM> get getSearchFeatureVM {
    return searchFeatures;
  }

  List<FeatureVM> get getfeatureVMTrue {
    featureVMTrue = [];
    for (FeatureVM feature in features) {
      if (feature.isSearchFeature) {
        featureVMTrue.add(feature);
      }
    }
    return featureVMTrue;
  }
}
