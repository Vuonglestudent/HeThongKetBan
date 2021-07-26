import 'package:smart_select/smart_select.dart';
import 'package:tinder_clone/Models/FeatureDetail.dart';

class FeatureVM {
  int featureId;
  int featureDetailId;
  String name;
  String content;
  int updateFeatureId;
  bool isSearchFeature;
  List<FeatureDetail> featureDetails = [];
  void setFeatureVM(Map<String, dynamic> response) {
    featureId = response['featureId'];
    featureDetailId = response['featureDetailId'];
    name = response['name'];
    content = response['content'];
    isSearchFeature = response['isSearchFeature'];
  }

  void setFeatureDetails(List<FeatureDetail> response) {
    for (FeatureDetail item in response) {
      featureDetails.add(item);
    }
  }

  Map<String, dynamic> get getFeatureVMJson {
    var data = {
      'featureId': this.featureId,
      'featureDetailId': this.featureDetailId,
      'name': this.name,
      'content': this.content,
      'isSearchFeature': this.isSearchFeature,
    };
    return data;
  }

  List<S2Choice<String>> get getFeatureDetails {
    List<S2Choice<String>> array = [];
    for (FeatureDetail item in featureDetails) {
      S2Choice<String> child =
          S2Choice<String>(title: item.content, value: item.id.toString());
      array.add(child);
    }
    return array;
  }
}
