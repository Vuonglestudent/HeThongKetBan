import 'package:flutter/foundation.dart';

class Loading extends ChangeNotifier {
  bool loading = false;
  void setLoading() {
    loading = !loading;
    notifyListeners();
  }
}
