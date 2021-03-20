import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:modal_progress_hud/modal_progress_hud.dart';
import 'package:provider/provider.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:smart_select/smart_select.dart';
import 'package:tinder_clone/Models/FeatureVM.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Models/User.dart';
import 'package:tinder_clone/Screens/ListImage_Screen.dart';
import 'package:tinder_clone/Widgets/Notification.dart';
import 'package:tinder_clone/Widgets/RadiantGradientMask.dart';
import 'package:tinder_clone/contants.dart';
import 'package:datetime_picker_formfield/datetime_picker_formfield.dart';

class ProfileDetails extends StatefulWidget {
  final String userId;
  final String authenticationUserId;
  ProfileDetails({@required this.authenticationUserId, @required this.userId});
  @override
  _ProfileDetailsState createState() => _ProfileDetailsState();
}

class _ProfileDetailsState extends State<ProfileDetails> {
  final List<Map> collections = [
    {"title": "Food joint", "image": "assets/images/person1.jpg"},
    {"title": "Photos", "image": "assets/images/person2.jpg"},
    {"title": "Travel", "image": "assets/images/person3.jpg"},
    {"title": "Nepal", "image": "assets/images/person4.jpg"},
  ];

  bool editUser = false;
  bool checkUser;
  bool getDataProfile = false;

  Future<bool> getUserById(BuildContext context) async {
    if (getDataProfile == false) {
      if (widget.authenticationUserId == widget.userId) {
        checkUser = true;
      } else {
        checkUser = false;
      }
      getDataProfile = await Provider.of<UserProvider>(context)
          .getProfileUser(widget.userId);
      return getDataProfile;
    } else
      return getDataProfile;
  }

  @override
  Widget build(BuildContext context) {
    return ModalProgressHUD(
      inAsyncCall: Provider.of<Loading>(context).loading,
      child: Scaffold(
        appBar: AppBar(
          elevation: 0,
          flexibleSpace: Container(
            decoration: BoxDecoration(
              gradient: colorApp,
            ),
          ),
        ),
        body: Stack(
          children: <Widget>[
            Container(
              height: 150.0,
              decoration: BoxDecoration(
                gradient: colorApp,
              ),
            ),
            FutureBuilder<bool>(
              future: getUserById(context),
              builder: (BuildContext context, AsyncSnapshot<bool> snapshot) {
                Widget children;
                if (snapshot.hasData) {
                  children = ListView.builder(
                    itemCount: 7,
                    itemBuilder: _mainListBuilder,
                  );
                } else if (snapshot.hasError) {
                  children = Center(
                    child: Text("Error Get Data"),
                  );
                } else {
                  children = Center(
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      crossAxisAlignment: CrossAxisAlignment.center,
                      children: [
                        SizedBox(
                          child: CircularProgressIndicator(),
                          width: 60,
                          height: 60,
                        ),
                        const Padding(
                          padding: EdgeInsets.only(top: 16),
                          child: Text('Awaiting result...'),
                        ),
                      ],
                    ),
                  );
                }
                return children;
              },
            ),
          ],
        ),
      ),
    );
  }

  Widget _mainListBuilder(BuildContext context, int index) {
    if (index == 0) return _buildHeader(context);
    if (Provider.of<UserProvider>(context).getlistImagesUser.length != 0) {
      if (index == 1) return _buildSectionHeader(context);
      if (index == 2) return _buildCollectionsRow();
    } else {
      if (index == 1) return Container();
      if (index == 2) return Container();
    }

    if (index == 3) return _userInfomation(context);
    if (index == 4) return _userHobby(context);
    if (index == 5) return _userSearch(context);
    return _buildListItem(context);
  }

  Container _buildHeader(BuildContext context) {
    return Container(
      height: 260.0,
      child: Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Stack(
            children: <Widget>[
              Container(
                padding: EdgeInsets.only(
                    top: 40.0, left: 40.0, right: 40.0, bottom: 10.0),
                child: Material(
                  shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(10.0)),
                  elevation: 5.0,
                  color: Colors.white,
                  child: Column(
                    children: <Widget>[
                      SizedBox(
                        height: 50.0,
                      ),
                      Text(
                        userProfile.getUserProfile.fullName,
                        style: Theme.of(context).textTheme.title,
                      ),
                      SizedBox(
                        height: 5.0,
                      ),
                      Text(
                          "${userProfile.getUserProfile.age} | ${userProfile.getUserProfile.gender} | ${userProfile.getUserProfile.location}"),
                      Container(
                        height: 60.0,
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: <Widget>[
                            Expanded(
                              child: ListTile(
                                title: Text(
                                  userProfile.getUserProfile.numberOfImages
                                      .toString(),
                                  textAlign: TextAlign.center,
                                  style: TextStyle(fontWeight: FontWeight.bold),
                                ),
                                subtitle: Text("Hình ảnh",
                                    textAlign: TextAlign.center,
                                    style: TextStyle(fontSize: 13.0)),
                              ),
                            ),
                            Expanded(
                              child: ListTile(
                                title: Text(
                                  userProfile.getUserProfile.numberOfFollowers
                                      .toString(),
                                  textAlign: TextAlign.center,
                                  style: TextStyle(fontWeight: FontWeight.bold),
                                ),
                                subtitle: Text("Theo dõi",
                                    textAlign: TextAlign.center,
                                    style: TextStyle(fontSize: 13.0)),
                              ),
                            ),
                            Expanded(
                              child: ListTile(
                                title: Text(
                                  userProfile.getUserProfile.numberOfFavoritors
                                      .toString(),
                                  textAlign: TextAlign.center,
                                  style: TextStyle(fontWeight: FontWeight.bold),
                                ),
                                subtitle: Text(
                                  "Yêu thích",
                                  textAlign: TextAlign.center,
                                  style: TextStyle(fontSize: 13.0),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                      Container(
                        height: 50.0,
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.center,
                          crossAxisAlignment: CrossAxisAlignment.center,
                          children: [
                            FollowAndFavorite(
                              icon: [
                                Icons.add_circle_outline,
                                Icons.add_circle
                              ],
                              titleButton: "Theo dõi",
                              checkUser: checkUser,
                            ),
                            SizedBox(
                              width: 5.0,
                            ),
                            FollowAndFavorite(
                              icon: [Icons.favorite_border, Icons.favorite],
                              titleButton: "Yêu Thích",
                              checkUser: checkUser,
                            )
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: <Widget>[
                  Material(
                    elevation: 5.0,
                    shape: CircleBorder(),
                    child: CircleAvatar(
                      radius: 40.0,
                      backgroundImage:
                          NetworkImage(userProfile.getUserProfile.avatarPath),
                    ),
                  ),
                ],
              ),
            ],
          );
        },
      ),
    );
  }

  Container _buildSectionHeader(BuildContext context) {
    return Container(
      color: Colors.white,
      padding: EdgeInsets.symmetric(horizontal: 20.0),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: <Widget>[
          Text(
            "Hình ảnh",
            style: Theme.of(context).textTheme.title,
          ),
        ],
      ),
    );
  }

  Container _buildCollectionsRow() {
    return Container(
      child: Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Container(
            color: Colors.white,
            height: 200.0,
            padding: EdgeInsets.symmetric(horizontal: 10.0),
            child: ListView.builder(
              physics: BouncingScrollPhysics(),
              scrollDirection: Axis.horizontal,
              itemCount: userProfile.getlistImagesUser.length > 5
                  ? 6
                  : userProfile.getlistImagesUser.length,
              itemBuilder: (BuildContext context, int index) {
                final widgetItem = (index == 5 &&
                        userProfile.getlistImagesUser.length > 5)
                    ? FlatButton(
                        shape: CircleBorder(),
                        onPressed: () {
                          Navigator.push(
                            context,
                            MaterialPageRoute(
                              builder: (BuildContext context) =>
                                  ListImageScreen(),
                            ),
                          );
                        },
                        child: Container(
                          width: 170.w,
                          height: 160.h,
                          decoration: BoxDecoration(
                              gradient: LinearGradient(
                                  colors: [
                                    Theme.of(context).accentColor,
                                    Theme.of(context).secondaryHeaderColor,
                                    Theme.of(context).primaryColor
                                  ],
                                  begin: Alignment.topRight,
                                  end: Alignment.bottomRight,
                                  stops: [0.0, 0.35, 1.0]),
                              color: Colors.green,
                              borderRadius: BorderRadius.circular(150.0)),
                          child: Icon(
                            Icons.arrow_right_sharp,
                            color: Colors.white,
                            size: ScreenUtil().setSp(125.0),
                          ),
                        ),
                      )
                    : Container(
                        margin: EdgeInsets.symmetric(
                            vertical: 5.0, horizontal: 10.0),
                        width: 130.0,
                        height: 200.0,
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: <Widget>[
                            Expanded(
                                child: ClipRRect(
                                    borderRadius: BorderRadius.circular(5.0),
                                    child: Image.network(
                                        userProfile.getlistImagesUser[index]
                                            .getImagePath,
                                        fit: BoxFit.cover))),
                          ],
                        ));
                return widgetItem;
              },
            ),
          );
        },
      ),
    );
  }

  Container _userInfomation(BuildContext context) {
    return Container(
      color: Colors.white,
      padding: EdgeInsets.only(left: 20.0, top: 20.0),
      child: Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Theme(
            data: Theme.of(context).copyWith(dividerColor: Colors.transparent),
            child: ExpansionTile(
                tilePadding: EdgeInsets.only(right: 10.0),
                title: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text("Thông tin", style: Theme.of(context).textTheme.title),
                    SizedBox(
                      width: 55.0,
                      child: FlatButton(
                        shape: CircleBorder(),
                        onPressed: () {
                          setState(() {
                            UniqueKey();
                            editUser = !editUser;
                          });
                        },
                        child: RadiantGradientMask(
                          child: Icon(
                            Icons.edit,
                            color: editUser
                                ? Colors.white
                                : Colors.blueGrey.shade200,
                          ),
                          setColor: editUser,
                        ),
                      ),
                    ),
                  ],
                ),
                children: [
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: childrenUserInfomation(context, userProfile),
                  ),
                ]),
          );
        },
      ),
    );
  }

  Widget _userHobby(BuildContext context) {
    return Container(
      color: Colors.white,
      padding: EdgeInsets.only(left: 20.0),
      child: Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Theme(
            data: Theme.of(context).copyWith(dividerColor: Colors.transparent),
            child: ExpansionTile(
                tilePadding: EdgeInsets.only(right: 10.0),
                title: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text("Sở thích", style: Theme.of(context).textTheme.title),
                    SizedBox(
                      width: 55.0,
                      child: FlatButton(
                        shape: CircleBorder(),
                        onPressed: () {
                          setState(() {
                            editUser = !editUser;
                          });
                        },
                        child: RadiantGradientMask(
                          child: Icon(
                            Icons.edit,
                            color: editUser
                                ? Colors.white
                                : Colors.blueGrey.shade200,
                          ),
                          setColor: editUser,
                        ),
                      ),
                    ),
                  ],
                ),
                children: [
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: childrenUserHobby(context, userProfile),
                  ),
                ]),
          );
        },
      ),
    );
  }

  Widget _userSearch(BuildContext context) {
    return Container(
      color: Colors.white,
      padding: EdgeInsets.only(left: 20.0),
      child: Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Theme(
            data: Theme.of(context).copyWith(dividerColor: Colors.transparent),
            child: ExpansionTile(
                tilePadding: EdgeInsets.only(right: 10.0),
                title: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text("Tìm kiếm", style: Theme.of(context).textTheme.title),
                    SizedBox(
                      width: 55.0,
                      child: FlatButton(
                        shape: CircleBorder(),
                        onPressed: () {
                          setState(() {
                            editUser = !editUser;
                          });
                        },
                        child: RadiantGradientMask(
                          child: Icon(
                            Icons.edit,
                            color: editUser
                                ? Colors.white
                                : Colors.blueGrey.shade200,
                          ),
                          setColor: editUser,
                        ),
                      ),
                    ),
                  ],
                ),
                children: [
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: childrenUserSearch(context, userProfile),
                  ),
                ]),
          );
        },
      ),
    );
  }

  Widget textFieldEditInfomation({
    UserProvider userProvider,
    String title,
    dynamic value,
    List<S2Choice<String>> listItem,
    String suff: "",
    String changeValue: "",
    int featureId,
  }) {
    Widget child;

    child = SmartSelect<String>.single(
        tileBuilder: (context, child) {
          return ListTile(
            enabled: editUser ? true : false,
            contentPadding: EdgeInsets.symmetric(horizontal: 10.0),
            title: TextFormField(
              key: UniqueKey(),
              enabled: false,
              initialValue: child.valueTitle,
              decoration: _inputDecoration(title: title, suff: suff),
            ),
            onTap: () {
              child.showModal();
              //print(child.context);
            },
          );
        },
        title: title,
        value: value,
        choiceItems: listItem != null ? listItem : options,
        onChange: (state) {
          setState(() {
            if (changeValue != "") {
              userProvider.setValueUserProfile(
                  changeValue, state.value, featureId);
              //state.value = state.valueTitle;
            }
          });
        });

    return child;
  }

  InputDecoration _inputDecoration({String title, String suff: ''}) {
    return InputDecoration(
      labelText: title,
      labelStyle: editUser
          ? kTitleProfileTextStyle.copyWith(color: Colors.blueGrey.shade700)
          : kTitleProfileTextStyle,
      contentPadding: EdgeInsets.only(bottom: 5.0),
      suffixText: suff,
      disabledBorder: editUser
          ? UnderlineInputBorder(
              borderSide: BorderSide(
                style: BorderStyle.solid,
                color: Theme.of(context).secondaryHeaderColor,
                width: 1.0,
              ),
            )
          : null,
    );
  }

  List<Widget> childrenUserInfomation(
      BuildContext context, UserProvider userProfile) {
    List<Widget> children = [];
    Widget child;
    if (editUser) {
      //Họ và Tên
      child = ListTile(
          enabled: editUser ? true : false,
          contentPadding: EdgeInsets.symmetric(horizontal: 10.0),
          title: TextFormField(
            key: UniqueKey(),
            initialValue: userProfile.getUserProfile.fullName,
            onChanged: (val) {
              userProfile.getUserProfile.fullName = val;
            },
            decoration: InputDecoration(
              labelText: "Họ và Tên",
              labelStyle: editUser
                  ? kTitleProfileTextStyle.copyWith(
                      color: Colors.blueGrey.shade700)
                  : kTitleProfileTextStyle,
              contentPadding: EdgeInsets.only(bottom: 5.0),
              enabledBorder: editUser
                  ? UnderlineInputBorder(
                      borderSide: BorderSide(
                        style: BorderStyle.solid,
                        color: Theme.of(context).secondaryHeaderColor,
                        width: 1.0,
                      ),
                    )
                  : null,
            ),
          ));
      children.add(child);
      child = const SizedBox(height: 20.0);
      children.add(child);
    }
    //Giới tính
    child = textFieldEditInfomation(
      userProvider: userProfile,
      listItem: userProfile.getProfileData.getGender,
      title: "Giới tính",
      changeValue: "gender",
      value: userProfile.getUserProfile.gender,
    );
    children.add(child);
    child = const SizedBox(height: 20.0);
    children.add(child);
    //Nơi ở
    child = textFieldEditInfomation(
      userProvider: userProfile,
      listItem: userProfile.getProfileData.getLocation,
      title: "Nơi ở",
      changeValue: "location",
      value: userProfile.getUserProfile.location.replaceAll(RegExp(r"_"), " "),
    );
    children.add(child);
    child = const SizedBox(height: 20.0);
    children.add(child);
    //Ngày sinh
    child = ListTile(
      contentPadding: EdgeInsets.symmetric(horizontal: 10.0),
      enabled: editUser ? true : false,
      title: DateTimeField(
        onChanged: (val) {
          userProfile.getUserProfile.dob =
              val.toString().replaceAll(RegExp(" "), "T").split(".")[0];
        },
        enabled: editUser ? true : false,
        initialValue: DateTime.parse(
            userProfile.getUserProfile.dob.split(RegExp(r"T"))[0]),
        resetIcon: null,
        decoration: InputDecoration(
          labelText: "Ngày sinh",
          labelStyle: editUser
              ? kTitleProfileTextStyle.copyWith(color: Colors.blueGrey.shade700)
              : kTitleProfileTextStyle,
          contentPadding: EdgeInsets.only(bottom: 5.0),
          enabledBorder: editUser
              ? UnderlineInputBorder(
                  borderSide: BorderSide(
                    style: BorderStyle.solid,
                    color: Theme.of(context).secondaryHeaderColor,
                    width: 1.0,
                  ),
                )
              : null,
        ),
        format: DateFormat("yyyy-MM-dd"),
        onShowPicker: (context, currentValue) {
          return showDatePicker(
              context: context,
              firstDate: DateTime(1900),
              initialDate: currentValue ?? DateTime.now(),
              lastDate: DateTime(2100));
        },
      ),
    ); // Ngày sinh
    children.add(child);
    child = const SizedBox(height: 20.0);
    children.add(child);
    //Chiều cao
    child = textFieldEditInfomation(
      userProvider: userProfile,
      title: "Chiều cao",
      suff: "cm",
      listItem: userProfile.getProfileData.getHeight,
      changeValue: "height",
      value: userProfile.getUserProfile.height.toString(),
    );
    children.add(child);
    child = const SizedBox(height: 20.0);
    children.add(child);
    //Cân nặng
    child = textFieldEditInfomation(
      userProvider: userProfile,
      title: "Cân nặng",
      suff: "kg",
      listItem: userProfile.getProfileData.getWeight,
      changeValue: "weight",
      value: userProfile.getUserProfile.weight.toString(),
    );
    children.add(child);
    child = const SizedBox(height: 20.0);
    children.add(child);
    //Cân nặng
    child = textFieldEditInfomation(
      userProvider: userProfile,
      title: "Công việc",
      listItem: userProfile.getProfileData.getJob,
      changeValue: "job",
      value: userProfile.getUserProfile.job.replaceAll(RegExp(r"_"), " "),
    );
    children.add(child);
    child = const SizedBox(height: 20.0);
    children.add(child);
    //featureVMTrue []
    for (FeatureVM item in userProfile.getUserProfile.getfeatureVMTrue) {
      child = textFieldEditInfomation(
        userProvider: userProfile,
        title: item.name,
        listItem: userProfile.getFeatureVM(item.featureId),
        changeValue: "features",
        featureId: item.featureId,
        value: item.featureDetailId.toString(),
      );
      children.add(child);
      child = const SizedBox(height: 20.0);
      children.add(child);
    }

    return children;
  }

  List<Widget> childrenUserHobby(
      BuildContext context, UserProvider userProfile) {
    List<Widget> children = [];
    Widget child;
    //featureVMFalse []
    for (FeatureVM item in userProfile.getUserProfile.getfeatureVMFalse) {
      child = textFieldEditInfomation(
        userProvider: userProfile,
        title: item.name,
        listItem: userProfile.getFeatureVM(item.featureId),
        changeValue: "features",
        featureId: item.featureId,
        value: item.featureDetailId.toString(),
      );
      children.add(child);
      child = const SizedBox(height: 20.0);
      children.add(child);
    }
    return children;
  }

  List<Widget> childrenUserSearch(
      BuildContext context, UserProvider userProfile) {
    List<Widget> children = [];
    Widget child;
    //Giới tính gợi ý
    child = textFieldEditInfomation(
      userProvider: userProfile,
      title: "Giới tính gợi ý",
      listItem: userProfile.getProfileData.getFindPeople,
      changeValue: 'findPeople',
      value: userProfile.getUserProfile.findPeople,
    );
    children.add(child);
    child = const SizedBox(height: 20.0);
    children.add(child);
    //Độ tuổi gợi ý
    child = child = textFieldEditInfomation(
      userProvider: userProfile,
      title: "Độ tuổi gợi ý",
      listItem: userProfile.getProfileData.getAgeGroup,
      changeValue: 'findAgeGroup',
      value:
          userProfile.getUserProfile.findAgeGroup.replaceAll(RegExp(r'_'), " "),
    );
    children.add(child);
    child = const SizedBox(height: 20.0);
    children.add(child);
    //searchfeatureVMTrue []
    for (FeatureVM item in userProfile.getUserProfile.getSearchFeatureVM) {
      child = textFieldEditInfomation(
        userProvider: userProfile,
        title: item.name,
        listItem: userProfile.getFeatureVM(item.featureId),
        changeValue: "searchFeatures",
        featureId: item.featureId,
        value: item.featureDetailId.toString(),
      );
      children.add(child);
      child = const SizedBox(height: 20.0);
      children.add(child);
    }
    return children;
  }

  String value = 'flutter';
  List<S2Choice<String>> options = [
    S2Choice<String>(value: 'ion', title: 'Ionic'),
    S2Choice<String>(value: 'flu', title: 'Flutter'),
    S2Choice<String>(value: 'rea', title: 'React Native'),
  ];
  Widget _buildListItem(BuildContext context) {
    if (editUser) {
      return Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Container(
            padding: EdgeInsets.symmetric(horizontal: 20, vertical: 5),
            color: Colors.white,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                Container(
                  padding: EdgeInsets.symmetric(vertical: 0.0),
                  decoration: BoxDecoration(
                    gradient: colorApp,
                    borderRadius: BorderRadius.circular(25.0),
                  ),
                  child: MaterialButton(
                    child: Row(
                      crossAxisAlignment: CrossAxisAlignment.center,
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          Icons.cloud_upload,
                        ),
                        SizedBox(width: 10.0),
                        Text(
                          "Cập nhật",
                          style: TextStyle(
                            color: Colors.white,
                            fontSize: 20.0,
                          ),
                        )
                      ],
                    ),
                    onPressed: () async {
                      Provider.of<Loading>(context, listen: false).setLoading();
                      var updated = await userProfile.updateUserProfile();
                      Provider.of<Loading>(context, listen: false).setLoading();
                      if (updated) {
                        notificationWidget(
                          context,
                          typeNotication: AlertType.info,
                          title: "Cập nhật Hồ Sơ",
                          desc: "Hồ sơ người dùng cập nhật thành công",
                          textButton: "OK",
                        ).show();
                      } else {
                        notificationWidget(
                          context,
                          typeNotication: AlertType.error,
                          title: "Cập nhật Hồ Sơ",
                          desc: "Hồ sơ người dùng cập nhật thất bại",
                          textButton: "Hủy",
                        ).show();
                      }
                    },
                    textColor: Colors.white,
                    padding: const EdgeInsets.all(16.0),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(30.0),
                    ),
                  ),
                ),
              ],
            ),
          );
        },
      );
    } else
      return Container();
  }
}

class FollowAndFavorite extends StatefulWidget {
  final bool checkUser;
  final List<IconData> icon;
  final String titleButton;
  FollowAndFavorite({this.icon, this.titleButton, this.checkUser});
  @override
  _FollowAndFavoriteState createState() => _FollowAndFavoriteState();
}

class _FollowAndFavoriteState extends State<FollowAndFavorite> {
  bool clickButton = false;

  @override
  Widget build(BuildContext context) {
    return RaisedButton(
      shape: !clickButton
          ? RoundedRectangleBorder(side: BorderSide(color: Colors.white))
          : RoundedRectangleBorder(
              side: BorderSide(color: Theme.of(context).secondaryHeaderColor)),
      onPressed: widget.checkUser
          ? () {}
          : () {
              setState(() {
                clickButton = !clickButton;
              });
            },
      color: Colors.white,
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceAround,
        children: [
          Container(
            child: Icon(
              !clickButton ? widget.icon[0] : widget.icon[1],
              color: !clickButton
                  ? Colors.blueGrey.shade600
                  : Theme.of(context).secondaryHeaderColor,
            ),
          ),
          SizedBox(
            width: 5.0,
          ),
          Container(
            width: 70.0,
            child: Text(
              widget.titleButton,
              style: kFunctionTextStyle.copyWith(
                color: Colors.blueGrey.shade600,
                fontSize: 15.0,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
