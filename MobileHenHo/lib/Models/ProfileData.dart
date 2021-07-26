import 'package:smart_select/smart_select.dart';
import 'package:tinder_clone/Models/Feature.dart';
import 'package:tinder_clone/Models/Models.dart';

class ProfileData {
  List<String> _findPeople = [];
  List<String> _job = [];
  List<String> _location = [];
  List<String> _ageGroup = [];
  List<Feature> _features = [];

  Iterable<int> get positiveIntegers sync* {
    int i = 0;
    while (true) yield i++;
  }

  List<S2Choice<String>> get getHeight {
    List<S2Choice<String>> array = [];
    for (int item in positiveIntegers.skip(50).take(200).toList()) {
      S2Choice<String> child =
          S2Choice<String>(title: item.toString(), value: item.toString());
      array.add(child);
    }
    return array;
  }

  List<S2Choice<String>> get getJob {
    List<S2Choice<String>> array = [];
    for (String item in _job) {
      S2Choice<String> child = S2Choice<String>(
        title: item.replaceAll(RegExp(r"_"), " "),
        value: item.replaceAll(RegExp(r"_"), " "),
      );
      array.add(child);
    }
    return array;
  }

  List<S2Choice<String>> get getFindPeople {
    List<S2Choice<String>> array = [];
    for (String item in _findPeople) {
      S2Choice<String> child = S2Choice<String>(
        title: item,
        value: item,
      );
      array.add(child);
    }
    return array;
  }

  List<S2Choice<String>> get getAgeGroup {
    List<S2Choice<String>> array = [];
    for (String item in _ageGroup) {
      S2Choice<String> child = S2Choice<String>(
        title: item.replaceAll(RegExp(r'_'), " "),
        value: item.replaceAll(RegExp(r'_'), " "),
      );
      array.add(child);
    }
    return array;
  }

  List<S2Choice<String>> get getWeight {
    List<S2Choice<String>> array = [];
    for (int item in positiveIntegers.skip(20).take(110).toList()) {
      S2Choice<String> child =
          S2Choice<String>(title: item.toString(), value: item.toString());
      array.add(child);
    }
    return array;
  }

  List<S2Choice<String>> get getLocation {
    List<S2Choice<String>> array = [];
    for (String item in _location) {
      S2Choice<String> child = S2Choice<String>(
        title: item.replaceAll(RegExp(r"_"), " "),
        value: item.replaceAll(RegExp(r"_"), " "),
      );
      array.add(child);
    }
    return array;
  }

  List<S2Choice<String>> get getGender {
    List<S2Choice<String>> array = [
      S2Choice<String>(title: "Nam", value: "Nam"),
      S2Choice<String>(title: "Nữ", value: "Nữ"),
    ];
    return array;
  }

  void setProfileData(Map<String, dynamic> response) {
    for (String item in response['findPeople']) {
      _findPeople.add(item);
    }
    for (String item in response['job']) {
      _job.add(item);
    }
    for (String item in response['location']) {
      _location.add(item);
    }
    for (String item in response['ageGroup']) {
      _ageGroup.add(item);
    }
    for (var item in response['features']) {
      Feature feature = new Feature();
      feature.id = item['id'];
      feature.isCalculated = item['isCalculated'];
      feature.name = item['name'];
      feature.weightRate = item['weightRate'];
      feature.isSearchFeature = item['isSearchFeature'];
      feature.setFeatureDetails(item['featureDetails']);
      _features.add(feature);
    }
  }

  List<Feature> get getFeatures {
    return _features;
  }
}
