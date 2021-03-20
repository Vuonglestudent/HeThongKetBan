import 'package:smart_select/smart_select.dart';
import 'package:tinder_clone/Models/FeatureDetail.dart';

class Feature {
  int id;
  String name;
  double weightRate;
  bool isCalculated;
  bool isSearchFeature;
  List<FeatureDetail> featureDetails = [];
  void setFeatureDetails(dynamic response) {
    for (var item in response) {
      FeatureDetail featureDetail = new FeatureDetail();
      featureDetail.id = item['id'];
      featureDetail.featureId = item['featureId'];
      featureDetail.content = item['content'];
      featureDetail.weight = item['weight'];
      featureDetails.add(featureDetail);
    }
  }
}
