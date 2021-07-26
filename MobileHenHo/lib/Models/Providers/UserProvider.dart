import 'dart:convert';
import 'dart:typed_data';

import 'package:flutter/foundation.dart';
import 'package:flutter/services.dart';
import 'package:http/http.dart';
import 'package:http_parser/http_parser.dart';
import 'package:multi_image_picker/multi_image_picker.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:smart_select/smart_select.dart';
import 'package:tinder_clone/Models/Feature.dart';
import 'package:tinder_clone/Models/FeatureDetail.dart';
import 'package:tinder_clone/Models/FeatureVM.dart';
import 'package:tinder_clone/Models/IFindAround.dart';
import 'package:tinder_clone/Models/IRelationship.dart';
import 'package:tinder_clone/Models/Image.dart';
import 'package:tinder_clone/Models/ProfileData.dart';
import 'package:tinder_clone/Models/User.dart';
import 'package:tinder_clone/Services/ImagesService.dart';
import 'package:tinder_clone/Services/MessageService.dart';
import 'package:tinder_clone/Services/RelationshipService.dart';
import 'package:tinder_clone/Services/UsersService.dart';

class UserPages {
  int index;
  final int size;
  final int total;
  final int current;
  final int position;
  UserPages({this.index, this.size, this.total, this.current, this.position});
}

class UserProvider extends ChangeNotifier {
  String accountId;
  List<Asset> _images = [];
  bool isAddImages = false;
  //User currentUser = new User();
  User _selectedProfileUser = User();
  ProfileData _profileData = ProfileData();
  //
  List<FeatureVM> _features = [];
  List<FeatureVM> _searchFeatures = [];
  //
  UserPages friendPage = new UserPages(index: 1, size: 8);
  List<UsersSimilar> _friendList = [];
  List<UsersSimilar> _filterFriend = [];
  //
  List<Image> _listImage = [];
  UserPages userPages =
      new UserPages(index: 1, size: 8, total: 0, current: 1, position: 1);

  IFindAround aroundPage = new IFindAround(pageIndex: 1, pageSize: 4);

  List<UsersSimilar> usersSimilar = [];
  bool editUser = false;
  int indexUserZinger = 0;
  int indexUserLocation = 0;
  List<Image> _imageUserSimilar;
  IRelationship _relationshipUser = new IRelationship(id: 0);
  List<Asset> get getImages {
    return _images;
  }

  ProfileData get getProfileData {
    return _profileData;
  }

  User get getUserProfile {
    return _selectedProfileUser;
  }

  void resetFriends() {
    _friendList = [];
    _filterFriend = [];
  }

  Future<bool> getUsersSimilar(String userId) async {
    accountId = userId;
    print('page: ${userPages.index}');
    try {
      await callApiGetUsersSimilar();
      return true;
    } catch (e) {
      print(e.toString());
      return false;
    }
  }

  Future<void> callApiGetUsersSimilar() async {
    try {
      Map<String, dynamic> response = await UserService().getUsersSimilar(
          accountId, userPages.index.toString(), userPages.size.toString());
      usersSimilar = [];
      for (Map<String, dynamic> item in response['data']) {
        UsersSimilar newUser = new UsersSimilar();
        newUser.setUserSimilar(item);
        usersSimilar.add(newUser);
      }
      if (indexUserZinger > 0) {
        usersSimilar = usersSimilar
            .getRange(indexUserZinger, usersSimilar.length)
            .toList();
      } else if (userPages.index == 1) {
        usersSimilar = usersSimilar.getRange(0, 8).toList();
      }
    } catch (e) {
      print("LoadUserSimilar Error: " + e.toString());
    }
  }

  void refreshUserPage(bool isSearch) async {
    if (isSearch) {
      aroundPage.pageIndex = 1;
      indexUserLocation = 0;
    } else {
      userPages.index = 1;
      indexUserZinger = 0;
      notifyListeners();
    }
  }

  void nextUserPage(bool isSearch) async {
    print('nextPage');
    usersSimilar = [];
    if (isSearch) {
      indexUserLocation = 0;
      aroundPage.pageIndex = aroundPage.pageIndex + 1;
    } else {
      indexUserZinger = 0;
      userPages.index = userPages.index + 1;
    }
  }

  void setValueUserProfile(String title, dynamic value, int featureId) {
    Map<String, dynamic> newInfomation = _selectedProfileUser.getUserJson();
    if (title != 'features' && title != 'searchFeatures') {
      newInfomation['$title'] = value;
    } else if (title == "features") {
      for (var item in newInfomation['$title']) {
        if (item['featureId'] == featureId) {
          item['featureDetailId'] = int.parse(value);
          for (FeatureVM feature in _features) {
            for (FeatureDetail featureDetail in feature.featureDetails) {
              if (featureDetail.id == int.parse(value)) {
                item['content'] = featureDetail.content;
              }
            }
          }
        }
      }
    } else if (title == "searchFeatures") {
      for (var item in newInfomation['$title']) {
        if (item['featureId'] == featureId) {
          item['featureDetailId'] = int.parse(value);
          for (FeatureVM searchFeature in _searchFeatures) {
            for (FeatureDetail featureDetail in searchFeature.featureDetails) {
              if (featureDetail.id == int.parse(value)) {
                item['content'] = featureDetail.content;
              }
            }
          }
        }
      }
    }
    editUser = true;
    _selectedProfileUser.setUser(newInfomation);

    // print("User");
    // print(_selectedProfileUser.location);
    // print(_selectedProfileUser.gender);
    // print(_selectedProfileUser.height);
    // print(_selectedProfileUser.weight);
    // print(_selectedProfileUser.job);
  }

  void setFavoritedZingerPage(int index) {
    if (usersSimilar[index].favorited) {
      usersSimilar[index].numberOfFavoritors =
          usersSimilar[index].numberOfFavoritors - 1;
    } else {
      usersSimilar[index].numberOfFavoritors =
          usersSimilar[index].numberOfFavoritors + 1;
    }
    try {
      UserService().setFavorite(usersSimilar[index].id);
      usersSimilar[index].favorited = !usersSimilar[index].favorited;
    } catch (e) {
      print(e.toString());
    }
    notifyListeners();
  }

  void setFollowedZingerPage(int index) {
    if (usersSimilar[index].followed) {
      usersSimilar[index].numberOfFollowers =
          usersSimilar[index].numberOfFollowers - 1;
    } else {
      usersSimilar[index].numberOfFollowers =
          usersSimilar[index].numberOfFollowers + 1;
    }
    try {
      UserService().setFollow(usersSimilar[index].id);
      usersSimilar[index].followed = !usersSimilar[index].followed;
    } catch (e) {
      print(e.toString());
    }
    notifyListeners();
  }

  void setFavoritedProfilePage() {
    if (getUserProfile.favorited) {
      getUserProfile.numberOfFavoritors = getUserProfile.numberOfFavoritors - 1;
    } else {
      getUserProfile.numberOfFavoritors = getUserProfile.numberOfFavoritors + 1;
    }
    getUserProfile.favorited = !getUserProfile.favorited;
    notifyListeners();
  }

  void setFollowedProfilePage() {
    if (getUserProfile.followed) {
      getUserProfile.numberOfFollowers = getUserProfile.numberOfFollowers - 1;
    } else {
      getUserProfile.numberOfFollowers = getUserProfile.numberOfFollowers + 1;
    }
    getUserProfile.followed = !getUserProfile.followed;
    notifyListeners();
  }

  Future<bool> updateRelationship(int indexRelationship) async {
    try {
      if (indexRelationship != null) {
        if (_relationshipUser.id != 0) {
          if (_relationshipUser.relationshipType.index != indexRelationship) {
            await RelationshipService()
                .saveRelationship(_selectedProfileUser.id, indexRelationship);
          }
        } else {
          await RelationshipService()
              .saveRelationship(_selectedProfileUser.id, indexRelationship);
        }
        return true;
      }
    } catch (e) {
      print("error createRelationship: " + e.toString());
      return false;
    }
  }

  Future<bool> updateUserProfile() async {
    try {
      var response =
          await UserService().updateUserProfile(_selectedProfileUser);
      return true;
    } catch (e) {
      print('update profile error: ' + e.toString());
      return false;
    }
  }

  List<S2Choice<String>> getFeatureVM(int featureId) {
    List<S2Choice<String>> array;
    for (FeatureVM item in _features) {
      if (item.featureId == featureId && item.isSearchFeature) {
        array = item.getFeatureDetails;
      } else if (item.featureId == featureId && !item.isSearchFeature) {
        array = item.getFeatureDetails;
      }
    }
    return array;
  }

  Future<bool> getProfileUser(String userId) async {
    try {
      var responseProfileUser = await UserService().getUserById(userId);
      _selectedProfileUser.setUser(responseProfileUser);
      var r = responseProfileUser['relationship'];
      if (r != null) {
        _relationshipUser = new IRelationship(
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
      } else {
        _relationshipUser = new IRelationship(id: 0);
      }
      print(_relationshipUser.id);
      try {
        var responseProfileData = await UserService().getProfileData();
        _profileData.setProfileData(responseProfileData);
        setFeatureAndSearchFeature();
      } catch (e) {
        print("Lỗi lấy profile data");
        print(e.toString());
      }
      try {
        _listImage = [];
        var responseListImage = await ImagesService().getImagesByUserId(userId);
        for (var item in responseListImage) {
          Image newImage = Image();
          newImage.setImage(item);
          _listImage.add(newImage);
        }
      } catch (e) {}
      notifyListeners();
      return true;
    } catch (e) {
      print("Lỗi lấy data");
      print(e.toString());
      return false;
    }
  }

  Future<void> getNewImageSeenUserSimilar(String userId) async {
    try {
      _imageUserSimilar = [];
      var responseListImage = await ImagesService().getImagesByUserId(userId);
      for (var item in responseListImage) {
        Image newImage = Image();
        newImage.setImage(item);
        _imageUserSimilar.add(newImage);
      }
    } catch (e) {}
    notifyListeners();
  }

  bool nextPageIndex() {
    if (_friendList.length == 0) {
      friendPage.index = 1;
      return true;
    } else {
      int page = (_friendList.length ~/ 8).toInt();
      if (page + 1 > friendPage.index) {
        friendPage.index = page + 1;
        return true;
      }
    }
    return false;
  }

  Future<bool> getFriendsUser() async {
    try {
      bool get = nextPageIndex();
      // if (friendPage.index == 1) {
      //   _friendList = [];
      // }
      if (get) {
        List<dynamic> response = await UserService().getFriendsUser(
            friendPage.index.toString(), friendPage.size.toString());
        for (var item in response) {
          UsersSimilar newUser = new UsersSimilar();
          newUser.setUserSimilar(item);
          _friendList.removeWhere((element) => element.id == newUser.id);
          _friendList.add(newUser);
        }
        print('Danh sách bạn bè: ' + _friendList.length.toString());
      } else {
        print('Đã lấy toàn bộ bạn bè');
      }
      if (friendPage.index != 1) notifyListeners();
      return true;
    } catch (e) {
      print(e.toString());
      return false;
    }
  }

  Future<bool> getSearchFriendByName(String name) async {
    try {
      print(name);
      if (name != '' && name != 'All') {
        var response = await UserService().searchFriendByName(name);
        if (response.length > 0) {
          _filterFriend = [];
          for (var item in response) {
            UsersSimilar newUser = new UsersSimilar();
            newUser.setUserSimilar(item);
            int index = _filterFriend
                    .indexWhere((element) => element.id == newUser.id) ??
                -1;
            index == -1 ? _filterFriend.add(newUser) : print("Da ton tai");
          }
          print('search: ' + _filterFriend.length.toString());
          notifyListeners();
          return true;
        }
      } else if (name == 'All') {
        _filterFriend = [];
        print('search: ' + _filterFriend.length.toString());
        notifyListeners();
        return true;
      }
      return false;
    } catch (e) {
      print('error filter friend name: ' + e.toString());
      return false;
    }
  }

  List<UsersSimilar> get friendList {
    return _friendList;
  }

  List<UsersSimilar> get filterFriendList {
    return _filterFriend;
  }

  List<Image> get getImageSeenUserSimilar {
    return _imageUserSimilar;
  }

  void setFeatureAndSearchFeature() {
    if (_profileData != null) {
      for (Feature itemProfileData in _profileData.getFeatures) {
        var isExist = false;
        if (_features.length > 0) {
          for (FeatureVM itemFeature in _features) {
            if (itemProfileData.id == itemFeature.featureId) {
              isExist = true;
              itemFeature.featureDetails = itemProfileData.featureDetails;
              break;
            }
          }
        }
        if (!isExist) {
          if (checkNonFeature(itemProfileData.id)) {
            continue;
          }
          FeatureVM feature = new FeatureVM();
          feature.featureId = itemProfileData.id;
          feature.featureDetailId = -1;
          feature.name = itemProfileData.name;
          feature.content = "CHƯA CẬP NHẬT!";
          feature.isSearchFeature = itemProfileData.isSearchFeature;
          feature.setFeatureDetails(itemProfileData.featureDetails);
          _features.add(feature);
        }
      }

      for (Feature itemProfileData in _profileData.getFeatures) {
        if (!itemProfileData.isSearchFeature) {
          continue;
        }
        var isExist = false;
        if (_searchFeatures.length > 0) {
          for (FeatureVM itemSearchFeature in _searchFeatures) {
            if (itemProfileData.id == itemSearchFeature.featureId) {
              isExist = true;
              itemSearchFeature.featureDetails = itemProfileData.featureDetails;
              break;
            }
          }
        }
        if (!isExist) {
          if (checkNonSearchFeature(itemProfileData.id)) {
            continue;
          }
          FeatureVM feature = new FeatureVM();
          feature.featureId = itemProfileData.id;
          feature.featureDetailId = -1;
          feature.name = itemProfileData.name;
          feature.content = "CHƯA CẬP NHẬT!";
          feature.isSearchFeature = itemProfileData.isSearchFeature;
          feature.setFeatureDetails(itemProfileData.featureDetails);
          _searchFeatures.add(feature);
        }
      }
    }
  }

  bool checkNonSearchFeature(int featureId) {
    for (int i = 0; i < _searchFeatures.length; i++) {
      if (_searchFeatures[i].featureId == featureId) {
        return true;
      }
    }
    return false;
  }

  bool checkNonFeature(int featureId) {
    for (int i = 0; i < _features.length; i++) {
      if (_features[i].featureId == featureId) {
        return true;
      }
    }
    return false;
  }

  Future<void> loadAssets() async {
    _images = [];

    List<Asset> resultList;

    try {
      resultList = await MultiImagePicker.pickImages(
        maxImages: 300,
      );
    } on Exception catch (e) {
      print(e.toString());
    }
    print(resultList);
    if (resultList != null) {
      _images = resultList;
      isAddImages = true;
    } else {
      isAddImages = false;
    }
    notifyListeners();
  }

  Future<dynamic> updateAvatar(String id) async {
    try {
      List<Asset> avatar = [];
      avatar = await MultiImagePicker.pickImages(maxImages: 1);
      ByteData byteData = await avatar[0].getByteData();
      List<int> imageData = byteData.buffer.asInt8List();
      MultipartFile multipartFile = MultipartFile.fromBytes(
        'Avatar',
        imageData,
        filename: 'some-file-name.jpg',
        contentType: MediaType("image", "jpg"),
      );

      return await UserService().updateAvatar(id, multipartFile);
    } on Exception catch (e) {
      print("err avatar: " + e.toString());
    }
  }

  void removeImage(int index) {
    if (_images.length > 1) {
      _images.removeAt(index);
      notifyListeners();
    } else {
      cancelAddImages();
    }
  }

  void cancelAddImages() {
    _images = [];
    isAddImages = false;
    notifyListeners();
  }

  Future<bool> uploadImagesToServer({
    @required String userId,
    @required Map<String, String> token,
    @required title,
  }) async {
    List<MultipartFile> multipartImageList =
        await convertImage(_images, 'Images');
    var messenger = await ImagesService().addImages(
      userId: userId,
      fileImages: multipartImageList,
      token: token,
      titleImages: title,
    );
    if (messenger != null)
      return true;
    else
      return false;
  }

  Future<void> sendImageChat({
    @required String destUserId,
    @required String content,
    @required List<Asset> assets,
  }) async {
    try {
      List<MultipartFile> multipartImageList =
          await convertImage(assets, 'Files');
      print(multipartImageList.length);
      return MessageService()
          .sendMessage(destUserId, content, multipartImageList)
          .then((value) {
        _images = [];
        notifyListeners();
      });
    } catch (e) {
      print("error send image: " + e.toString());
    }
  }

  Future<List<MultipartFile>> convertImage(
    List<Asset> assets,
    String titleData,
  ) async {
    List<MultipartFile> multipartImageList = [];
    for (Asset image in assets) {
      ByteData byteData = await image.getByteData();
      List<int> imageData = byteData.buffer.asInt8List();
      MultipartFile multipartFile = MultipartFile.fromBytes(
        titleData,
        imageData,
        filename: 'some-file-name.jpg',
        contentType: MediaType("image", "jpg"),
      );
      multipartImageList.add(multipartFile);
    }
    return multipartImageList;
  }

  List<Image> get getlistImagesUser {
    return _listImage;
  }

  IRelationship get relationship {
    return _relationshipUser;
  }

  Future<dynamic> findAround() async {
    try {
      var prefs = await SharedPreferences.getInstance();
      var distance = prefs.getString('radiusLocation') ?? '20';
      var gender = prefs.getString('genderLocation') ?? '0';
      var age = prefs.getString('ageLocation') ?? '0';
      var userId = prefs.getString('userId');
      print('search around: $distance km, $gender, $age');

      aroundPage = new IFindAround(
        pageIndex: aroundPage.pageIndex,
        pageSize: aroundPage.pageSize,
        userId: userId,
        distance: int.parse(distance),
        gender: int.parse(gender),
        ageGroup: int.parse(age),
      );
      var response = await UserService().findAround(aroundPage);
      if (response != []) {
        usersSimilar = [];
        for (Map<String, dynamic> item in response) {
          UsersSimilar newUser = new UsersSimilar();
          newUser.setUserSimilar(item);
          usersSimilar.add(newUser);
        }
        if (indexUserLocation > 0) {
          usersSimilar = usersSimilar
              .getRange(indexUserLocation, usersSimilar.length)
              .toList();
        } else if (userPages.index == 1) {
          usersSimilar = usersSimilar.getRange(0, usersSimilar.length).toList();
        }
        print('lenght: ${usersSimilar.length}');
      }
      return true;
    } catch (e) {
      print('error find around: ${e.toString()}');
      return false;
    }
  }

  Future<bool> blockUser(String userId) async {
    try {
      await UserService().blockUser(userId);
      return true;
    } catch (e) {
      print('error block user: ' + e.toString());
      return false;
    }
  }
}
