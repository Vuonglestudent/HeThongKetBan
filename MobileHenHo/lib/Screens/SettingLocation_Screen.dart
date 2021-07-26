import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:provider/provider.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:smart_select/smart_select.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Screens/Zinger_Screen.dart';
import 'package:tinder_clone/Widgets/SaveAndCanceSetting.dart';
import 'package:tinder_clone/contants.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class SettingLocationScreen extends StatefulWidget {
  @override
  _SettingLocationScreenState createState() => _SettingLocationScreenState();
}

class _SettingLocationScreenState extends State<SettingLocationScreen> {
  TextEditingController myController;
  List<S2Choice<String>> _listGender = [
    S2Choice<String>(value: '0', title: 'Tất cả'),
    S2Choice<String>(value: '1', title: 'Nam'),
    S2Choice<String>(value: '2', title: 'Nữ'),
  ];
  String _gender;
  List<S2Choice<String>> _listAge = [
    S2Choice<String>(value: '-1', title: 'Tất cả'),
    S2Choice<String>(value: '0', title: 'Dưới 18 tuổi'),
    S2Choice<String>(value: '1', title: 'Từ 18 đến 25'),
    S2Choice<String>(value: '2', title: 'Từ 25 đến 30'),
    S2Choice<String>(value: '3', title: 'Từ 31 đến 40'),
    S2Choice<String>(value: '4', title: 'Từ 41 đến 50'),
    S2Choice<String>(value: '5', title: 'Trên 50'),
  ];
  String _age;
  String _radius;
  bool isUpdate;
  Future createSharedPrefernces() async {
    if (_gender == null && _age == null && _radius == null) {
      var prefs = await SharedPreferences.getInstance();
      _gender = (prefs.getString('genderLocation') ?? "0");
      _age = (prefs.getString('ageLocation') ?? "-1");
      _radius = prefs.getString('radiusLocation') ?? "20";
    }
  }

  @override
  void initState() {
    super.initState();
    myController = new TextEditingController();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey.shade200,
      appBar: AppBar(
        flexibleSpace: Container(
          decoration: BoxDecoration(
            gradient: colorApp,
          ),
        ),
        title: Text(
          'CÀI ĐẶT VỊ TRÍ TÌM KIẾM',
        ),
      ),
      body: Container(
        child: FutureBuilder(
          future: createSharedPrefernces(),
          builder: (context, AsyncSnapshot snapshot) {
            Widget child;
            if (_gender != null) {
              if (myController.text == '') {
                myController.text = _radius;
              }
              child = Consumer<AuthenticationProvider>(
                builder: (context, authen, child) {
                  return SingleChildScrollView(
                    padding: EdgeInsets.all(40.h),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        isUpdate != null
                            ? isUpdate
                                ? Row(
                                    children: [
                                      Container(
                                        padding: EdgeInsets.only(
                                            left: 50.w, right: 20.w),
                                        child: Text(
                                          "Cập nhật thành công",
                                          style: TextStyle(
                                            color: Colors.grey,
                                            fontStyle: FontStyle.italic,
                                          ),
                                        ),
                                      ),
                                      Icon(
                                        Icons.info_outline,
                                        color: Colors.green,
                                      )
                                    ],
                                  )
                                : Row(
                                    children: [
                                      Container(
                                        padding: EdgeInsets.only(
                                            left: 50.w, right: 20.w),
                                        child: Text(
                                          "Cập nhật thất bại",
                                          style: TextStyle(
                                            color: Colors.grey,
                                            fontStyle: FontStyle.italic,
                                          ),
                                        ),
                                      ),
                                      Icon(
                                        Icons.dangerous,
                                        color: Colors.red,
                                      )
                                    ],
                                  )
                            : Container(),
                        Card(
                          elevation: 0.5,
                          margin: EdgeInsets.symmetric(
                            vertical: 10.h,
                            horizontal: 0,
                          ),
                          child: Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: <Widget>[
                              SmartSelect<String>.single(
                                title: 'Giới tính',
                                value: _gender,
                                choiceItems: _listGender,
                                modalType: S2ModalType.bottomSheet,
                                onChange: (state) => setState(() {
                                  _gender = state.value;
                                  isUpdate = null;
                                }),
                              ),
                              buildDivider(),
                              SmartSelect<String>.single(
                                title: 'Độ tuổi',
                                value: _age,
                                choiceItems: _listAge,
                                modalType: S2ModalType.bottomSheet,
                                onChange: (state) => setState(() {
                                  _age = state.value;
                                  isUpdate = null;
                                }),
                              ),
                              buildDivider(),
                              ListTile(
                                title: Text("Bán kính"),
                                trailing: Container(
                                  child: Row(
                                    mainAxisSize: MainAxisSize.min,
                                    children: [
                                      Container(
                                        constraints:
                                            BoxConstraints(maxWidth: 200.w),
                                        child: TextField(
                                            controller: myController,
                                            onChanged: (value) {
                                              setState(() {
                                                isUpdate = null;
                                              });
                                            },
                                            textAlign: TextAlign.end,
                                            style:
                                                TextStyle(color: Colors.grey),
                                            keyboardType: TextInputType.number,
                                            inputFormatters: <
                                                TextInputFormatter>[
                                              FilteringTextInputFormatter
                                                  .digitsOnly
                                            ]),
                                      ),
                                      Padding(
                                        padding: EdgeInsets.only(left: 10.w),
                                        child: Text(
                                          'km',
                                          style: TextStyle(color: Colors.grey),
                                        ),
                                      ),
                                    ],
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                        SaveAndCanceSetting(
                          onPRessCance: () {
                            Navigator.pushReplacement(
                              context,
                              PageRouteBuilder(
                                pageBuilder:
                                    (context, animation1, animation2) =>
                                        super.widget,
                                transitionDuration: Duration(seconds: 0),
                              ),
                            );
                          },
                          onPressSave: () async {
                            try {
                              var prefs = await SharedPreferences.getInstance();
                              prefs.setString('genderLocation', _gender);
                              prefs.setString('ageLocation', _age);
                              prefs.setString('radiusLocation',
                                  myController.text.toString());
                              setState(() {
                                isUpdate = true;
                              });
                            } catch (e) {
                              print('Error update location: ' + e.toString());
                              isUpdate = false;
                            }
                          },
                        ),
                      ],
                    ),
                  );
                },
              );
            } else {
              child = SearchMatching(
                atCenter: true,
                chng: true,
                title: "Load data location...",
              );
            }
            return child;
          },
        ),
      ),
    );
  }
}
