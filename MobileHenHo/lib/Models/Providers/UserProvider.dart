import 'dart:convert';
import 'dart:typed_data';

import 'package:flutter/foundation.dart';
import 'package:flutter/services.dart';
import 'package:http/http.dart';
import 'package:http_parser/http_parser.dart';
import 'package:multi_image_picker/multi_image_picker.dart';
import 'package:smart_select/smart_select.dart';
import 'package:tinder_clone/Models/Feature.dart';
import 'package:tinder_clone/Models/FeatureDetail.dart';
import 'package:tinder_clone/Models/FeatureVM.dart';
import 'package:tinder_clone/Models/Image.dart';
import 'package:tinder_clone/Models/ProfileData.dart';
import 'package:tinder_clone/Models/User.dart';
import 'package:tinder_clone/Services/ImagesService.dart';
import 'package:tinder_clone/Services/UsersService.dart';

class UserProvider extends ChangeNotifier {
  List<Asset> _images = [];
  bool isAddImages = false;
  //User currentUser = new User();
  User _selectedProfileUser = User();
  ProfileData _profileData = ProfileData();
  List<FeatureVM> _features = [];
  List<FeatureVM> _searchFeatures = [];
  List<Image> _listImage = [];
  bool editUser = false;
  List<Asset> get getImages {
    return _images;
  }

  ProfileData get getProfileData {
    return _profileData;
  }

  User get getUserProfile {
    return _selectedProfileUser;
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

  Future<bool> updateUserProfile() async {
    try {
      var response =
          await UserService().updateUserProfile(_selectedProfileUser);

      return true;
    } catch (e) {
      print(e.toString());
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
          Image newImage = Image(item);
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

  void cancelAddImages() {
    _images = [];
    isAddImages = false;
    notifyListeners();
  }

  Future<bool> uploadImagesToServer(
      {@required String userId, @required Map<String, String> token}) async {
    List<MultipartFile> multipartImageList = [];
    for (Asset image in _images) {
      ByteData byteData = await image.getByteData();
      List<int> imageData = byteData.buffer.asInt8List();
      MultipartFile multipartFile = MultipartFile.fromBytes(
        'Images',
        imageData,
        filename: 'some-file-name.jpg',
        contentType: MediaType("image", "jpg"),
      );
      multipartImageList.add(multipartFile);
    }

    var messenger = await ImagesService().addImages(
      userId: userId,
      fileImages: multipartImageList,
      token: token,
    );
    if (messenger != null)
      return true;
    else
      return false;
  }

  List<Image> get getlistImagesUser {
    return _listImage;
  }
}
