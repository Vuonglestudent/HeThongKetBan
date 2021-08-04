import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:modal_progress_hud/modal_progress_hud.dart';
import 'package:provider/provider.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:smart_select/smart_select.dart';
import 'package:tinder_clone/Models/FeatureVM.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Models/Providers/MessageProvider.dart';
import 'package:tinder_clone/Models/Providers/NotificationsProvider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Models/User.dart';
import 'package:tinder_clone/Screens/InPersonChat_Screen.dart';
import 'package:tinder_clone/Screens/ListImage_Screen.dart';
import 'package:tinder_clone/Screens/Zinger_Screen.dart';
import 'package:tinder_clone/Services/UsersService.dart';
import 'package:tinder_clone/Widgets/FullScreenImage.dart';
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

  final List<S2Choice<String>> relationshipType = [
    S2Choice<String>(value: '0', title: 'Không có gì'),
    S2Choice<String>(value: '1', title: 'Đang tìm hiểu'),
    S2Choice<String>(value: '2', title: 'Đang yêu'),
    S2Choice<String>(value: '3', title: 'Đã kết hôn'),
  ];
  bool editUser = false;
  bool checkUser;
  bool getDataProfile = false;
  int indexRelationship;
  Future<bool> getUserById(BuildContext context) async {
    var user = Provider.of<UserProvider>(context);
    if (getDataProfile == false) {
      if (widget.authenticationUserId == widget.userId) {
        checkUser = true;
      } else {
        checkUser = false;
      }
      getDataProfile = await user.getProfileUser(widget.userId);
      return getDataProfile;
    } else
      return getDataProfile;
  }

  void _showPopupMenu(UserProvider user) async {
    await showMenu(
      context: context,
      position: RelativeRect.fromLTRB(200.w, 120.h, 60.w, 0),
      items: [
        PopupMenuItem(
          child: TextButton(
            onPressed: () async {
              UsersSimilar u = new UsersSimilar(
                id: user.getUserProfile.id,
                fullName: user.getUserProfile.fullName,
                avatarPath: user.getUserProfile.avatarPath,
              );
              await Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (BuildContext context) => InPersonChatScreen(
                    usersSimilar: u,
                  ),
                ),
              ).then((value) =>
                  Provider.of<MessageProvider>(context, listen: false)
                      .saveUserWhenExistChat());
            },
            child: Container(
              width: 450.w,
              child: Row(
                children: [
                  Icon(
                    Icons.message_outlined,
                    color: Colors.grey.shade600,
                    size: 45.w,
                  ),
                  SizedBox(
                    width: 35.w,
                  ),
                  Text(
                    "Nhắn tin",
                    style: TextStyle(
                      color: Colors.grey.shade600,
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
        PopupMenuItem(
          child: SmartSelect<String>.single(
              tileBuilder: (context, child) {
                return TextButton(
                  onPressed: () {
                    child.showModal();
                  },
                  child: Container(
                    width: 450.w,
                    child: Row(
                      children: [
                        Image(
                          image: AssetImage("assets/images/relationship.png"),
                          color: Colors.grey.shade600,
                          height: 40.w,
                        ),
                        SizedBox(width: 10.w),
                        Text(
                          "Tạo mối quan hệ",
                          style: TextStyle(
                            color: Colors.grey.shade600,
                          ),
                        ),
                      ],
                    ),
                  ),
                );
              },
              modalType: S2ModalType.bottomSheet,
              title: "Thiết lập quan hệ",
              value: indexRelationship == null
                  ? user.relationship.id != 0
                      ? user.relationship.relationshipType.index.toString()
                      : indexRelationship.toString()
                  : indexRelationship.toString(),
              choiceItems: relationshipType,
              onChange: (state) async {
                if (state.value != "null") {
                  var index = state.value;
                  if (user.relationship.id != 0) {
                    if (index !=
                        user.relationship.relationshipType.index.toString()) {
                      indexRelationship = int.parse(index);
                    }
                  } else {
                    indexRelationship = int.parse(index);
                  }
                  var createRelationship =
                      await user.updateRelationship(indexRelationship);
                  if (createRelationship) {
                    notificationWidget(
                      context,
                      typeNotication: AlertType.info,
                      title: "Tạo mối quan hệ",
                      desc: "Mối quan hệ được tạo thành công",
                      textButton: "OK",
                    ).show();
                  } else {
                    notificationWidget(
                      context,
                      typeNotication: AlertType.error,
                      title: "Tạo mối quan hệ",
                      desc: "Mối quan hệ được tạo thất bại",
                      textButton: "Hủy",
                    ).show();
                  }
                }
                print(indexRelationship);
              }),
        ),
      ],
      elevation: 8.0,
    );
  }

  @override
  Widget build(BuildContext context) {
    var user = Provider.of<UserProvider>(context);
    return ModalProgressHUD(
      inAsyncCall: Provider.of<Loading>(context).loading,
      child: Scaffold(
        appBar: AppBar(
          elevation: 0,
          actions: [
            widget.userId != widget.authenticationUserId
                ? IconButton(
                    icon: Icon(Icons.more_vert),
                    onPressed: () {
                      _showPopupMenu(user);
                    },
                  )
                : Container(),
          ],
          flexibleSpace: Container(
            decoration: BoxDecoration(
              gradient: colorApp,
            ),
          ),
        ),
        body: Stack(
          children: <Widget>[
            Container(
              height: 400.h,
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
                    itemCount: 8,
                    itemBuilder: _mainListBuilder,
                  );
                } else if (snapshot.hasError) {
                  children = Center(
                    child: Text("Error Get Data"),
                  );
                } else {
                  children = Center(
                    child: Container(
                      padding: EdgeInsets.only(top: 350.h),
                      height: double.infinity,
                      child: SearchMatching(
                        atCenter: true,
                        chng: true,
                        title: "Lấy dữ người dùng...",
                      ),
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
    var user = Provider.of<UserProvider>(context);
    if (index == 0) return _buildHeader(context);
    if (user.relationship.id != 0) {
      if (index == 1) return _buildRelationshipHeader(context);
    } else {
      if (index == 1) return Container();
    }
    if (user.getlistImagesUser.length > 0) {
      if (index == 2) return _buildSectionHeader(context);
      if (index == 3) return _buildCollectionsRow();
    } else {
      if (index == 2) return Container();
      if (index == 3) return Container();
    }

    if (index == 4) return _userInfomation(context);
    if (index == 5) return _userHobby(context);
    if (index == 6) return _userSearch(context);
    return _buildListItem(context);
  }

  Container _buildHeader(BuildContext context) {
    var notification = Provider.of<NotificationsProvider>(context);
    return Container(
      height: 680.h,
      child: Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Stack(
            children: <Widget>[
              Container(
                padding: EdgeInsets.only(
                    top: 100.h, left: 100.h, right: 100.h, bottom: 30.h),
                child: Material(
                  shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(30.sp)),
                  elevation: 5.0,
                  color: Colors.white,
                  child: Column(
                    children: <Widget>[
                      SizedBox(
                        height: 130.h,
                      ),
                      Text(
                        userProfile.getUserProfile.fullName,
                        style: Theme.of(context).textTheme.headline6.copyWith(
                            fontSize: 60.sp, fontWeight: FontWeight.w700),
                      ),
                      SizedBox(
                        height: 10.h,
                      ),
                      Text(
                        "${userProfile.getUserProfile.age} | ${userProfile.getUserProfile.gender} | ${userProfile.getUserProfile.location}",
                        style: kTitleProfileTextStyle.copyWith(
                            color: Colors.blueGrey.shade700, fontSize: 40.sp),
                      ),
                      Container(
                        height: 140.h,
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
                                    style: TextStyle(fontSize: 35.sp)),
                              ),
                            ),
                            // Expanded(
                            //   child: ListTile(
                            //     title: Text(
                            //       userProfile.getUserProfile.numberOfFollowers
                            //           .toString(),
                            //       textAlign: TextAlign.center,
                            //       style: TextStyle(fontWeight: FontWeight.bold),
                            //     ),
                            //     subtitle: Text("Theo dõi",
                            //         textAlign: TextAlign.center,
                            //         style: TextStyle(fontSize: 35.sp)),
                            //   ),
                            // ),
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
                                  style: TextStyle(fontSize: 35.sp),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                      Container(
                        height: 130.h,
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
                              userId: widget.userId,
                              clickButton: userProfile.getUserProfile.followed,
                              onPressed: () async {
                                try {
                                  Provider.of<Loading>(context, listen: false)
                                      .setLoading();
                                  await UserService().setFollow(widget.userId);
                                  userProfile.setFollowedProfilePage();
                                  Provider.of<Loading>(context, listen: false)
                                      .setLoading();
                                } catch (e) {}
                              },
                            ),
                            SizedBox(
                              width: 5.0,
                            ),
                            FollowAndFavorite(
                              icon: [Icons.favorite_border, Icons.favorite],
                              titleButton: "Yêu Thích",
                              checkUser: checkUser,
                              userId: widget.userId,
                              clickButton: userProfile.getUserProfile.favorited,
                              onPressed: () async {
                                try {
                                  Provider.of<Loading>(context, listen: false)
                                      .setLoading();
                                  await UserService()
                                      .setFavorite(widget.userId);
                                  userProfile.setFavoritedProfilePage();
                                  Provider.of<Loading>(context, listen: false)
                                      .setLoading();
                                } catch (e) {}
                              },
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
                  MaterialButton(
                    onPressed: () {
                      Navigator.push(context, MaterialPageRoute(builder: (_) {
                        return FullScreenImage(
                          imageUrl: userProfile.getUserProfile.avatarPath,
                          tag: 0,
                          galleryItems: null,
                        );
                      }));
                    },
                    elevation: 5.0,
                    shape: CircleBorder(),
                    child: CircleAvatar(
                      radius: 120.sp,
                      backgroundImage:
                          NetworkImage(userProfile.getUserProfile.avatarPath),
                    ),
                  ),
                ],
              ),
              checkUser
                  ? Container()
                  : Container(
                      padding: EdgeInsets.only(top: 80.h, right: 130.w),
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: [
                          Container(
                            child: MaterialButton(
                              minWidth: 50.w,
                              padding: EdgeInsets.all(0.0),
                              child: Row(
                                children: [
                                  Text(
                                    "Chặn",
                                    style: TextStyle(
                                      fontSize: 35.sp,
                                      color: Colors.grey,
                                    ),
                                  ),
                                  SizedBox(width: 5.w),
                                  Icon(
                                    Icons.block_outlined,
                                    color: Colors.grey,
                                    size: 65.sp,
                                  ),
                                ],
                              ),
                              onPressed: () async {
                                await Alert(
                                  context: context,
                                  type: AlertType.none,
                                  title: 'Chặn người dùng',
                                  style: AlertStyle(
                                    titleStyle: TextStyle(
                                      fontWeight: FontWeight.w700,
                                      fontSize: 70.sp,
                                    ),
                                    descStyle: TextStyle(fontSize: 55.sp),
                                  ),
                                  desc:
                                      'Bạn và người dùng sẽ không thể thấy thông tin của nhau. Bạn có muốn chặn!',
                                  buttons: [
                                    DialogButton(
                                      child: Text(
                                        "Hủy",
                                        style: TextStyle(
                                            color: Colors.white,
                                            fontSize: 60.sp),
                                      ),
                                      onPressed: () {
                                        Navigator.pop(context, false);
                                      },
                                      radius: BorderRadius.circular(0.0),
                                      color: Theme.of(context).accentColor,
                                    ),
                                    DialogButton(
                                      child: Text(
                                        "Đồng ý",
                                        style: TextStyle(
                                            color: Colors.white,
                                            fontSize: 60.sp),
                                      ),
                                      onPressed: () {
                                        Navigator.pop(context, true);
                                      },
                                      radius: BorderRadius.circular(0.0),
                                      color: Theme.of(context)
                                          .secondaryHeaderColor,
                                    ),
                                  ],
                                ).show().then((value) async {
                                  if (value) {
                                    await userProfile
                                        .blockUser(
                                            userProfile.getUserProfile.id)
                                        .then((r) {
                                      print(r);
                                      if (r) {
                                        notification.showNotification(
                                          userProfile.getUserProfile.fullName,
                                          "đã bị chặn thành công",
                                          userProfile.getUserProfile.id,
                                        );
                                        Navigator.pop(context);
                                      } else {
                                        notification.showNotification(
                                          userProfile.getUserProfile.fullName,
                                          "không thể bị chặn lúc này",
                                          userProfile.getUserProfile.id,
                                        );
                                      }
                                    });
                                  }
                                });
                              },
                            ),
                          )
                        ],
                      ),
                    ),
            ],
          );
        },
      ),
    );
  }

  Container _buildRelationshipHeader(BuildContext context) {
    return Container(
      child: Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Container(
            color: Colors.white,
            padding: EdgeInsets.only(left: 50.w, top: 30.h, right: 50.w),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: <Widget>[
                Text(
                  relationshipType[
                          userProfile.relationship.relationshipType.index]
                      .title,
                  style: Theme.of(context).textTheme.title.copyWith(
                        color: Colors.lightBlue.shade300,
                        fontSize: 55.sp,
                        fontWeight: FontWeight.w700,
                      ),
                ),
                SizedBox(height: 10.h),
                Text(
                  "với",
                  style: Theme.of(context)
                      .textTheme
                      .headline6
                      .copyWith(fontSize: 40.sp),
                ),
                Container(
                  color: Colors.white,
                  height: 350.h,
                  padding:
                      EdgeInsets.symmetric(horizontal: 40.w, vertical: 20.h),
                  child: Container(
                    child: _userRelationship(
                      userProfile.relationship.fromId ==
                              userProfile.getUserProfile.id
                          ? userProfile.relationship.toAvatar
                          : userProfile.relationship.fromAvatar,
                      userProfile.relationship.fromId ==
                              userProfile.getUserProfile.id
                          ? userProfile.relationship.toName
                          : userProfile.relationship.fromName,
                    ),
                  ),
                ),
              ],
            ),
          );
        },
      ),
    );
  }

  Widget _userRelationship(String avatar, String name) {
    return Column(
      children: [
        CircleAvatar(
          radius: 120.sp,
          backgroundImage: NetworkImage(avatar),
        ),
        SizedBox(height: 10.h),
        Text(
          name,
          style: Theme.of(context)
              .textTheme
              .headline6
              .copyWith(fontSize: 60.sp, fontWeight: FontWeight.w700),
        ),
      ],
    );
  }

  Container _buildSectionHeader(BuildContext context) {
    return Container(
      color: Colors.white,
      padding: EdgeInsets.symmetric(horizontal: 50.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: <Widget>[
          Text(
            "HÌNH ẢNH",
            style: headerStyle,
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
            height: 500.h,
            padding: EdgeInsets.symmetric(horizontal: 50.w),
            child: ListView.builder(
              physics: BouncingScrollPhysics(),
              scrollDirection: Axis.horizontal,
              itemCount: userProfile.getlistImagesUser.length > 5
                  ? 6
                  : userProfile.getlistImagesUser.length,
              itemBuilder: (BuildContext context, int index) {
                final widgetItem = (index == 5 &&
                        userProfile.getlistImagesUser.length > 5)
                    ? MaterialButton(
                        shape: CircleBorder(),
                        onPressed: () {
                          Navigator.push(
                            context,
                            MaterialPageRoute(
                              builder: (BuildContext context) =>
                                  ListImageScreen(
                                userId: userProfile.getUserProfile.id,
                              ),
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
                    : MaterialButton(
                        padding: EdgeInsets.zero,
                        minWidth: 350.w,
                        onPressed: () async {
                          await Navigator.push(context,
                              MaterialPageRoute(builder: (_) {
                            return FullScreenImage(
                              imageUrl: userProfile
                                  .getlistImagesUser[index].getImagePath,
                              tag: index,
                              galleryItems: userProfile.getlistImagesUser,
                            );
                          }));
                        },
                        child: Container(
                            margin: EdgeInsets.symmetric(
                                vertical: 10.h, horizontal: 10.w),
                            width: 350.w,
                            height: 400.h,
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: <Widget>[
                                Expanded(
                                  child: ClipRRect(
                                    borderRadius: BorderRadius.circular(5.0),
                                    child: Image.network(
                                        userProfile.getlistImagesUser[index]
                                            .getImagePath,
                                        fit: BoxFit.cover),
                                  ),
                                ),
                              ],
                            )),
                      );
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
      padding: EdgeInsets.symmetric(horizontal: 50.w),
      child: Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Theme(
            data: Theme.of(context).copyWith(dividerColor: Colors.transparent),
            child: ExpansionTile(
                tilePadding: EdgeInsets.only(right: 25.w),
                title: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text("THÔNG TIN", style: headerStyle),
                    SizedBox(
                      child: MaterialButton(
                        shape: CircleBorder(),
                        minWidth: 100.w,
                        onPressed: () {
                          setState(() {
                            UniqueKey();
                            if (checkUser) editUser = !editUser;
                          });
                        },
                        child: RadiantGradientMask(
                          child: Icon(
                            Icons.edit,
                            size: 70.sp,
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
      padding: EdgeInsets.symmetric(horizontal: 50.w),
      child: Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Theme(
            data: Theme.of(context).copyWith(dividerColor: Colors.transparent),
            child: ExpansionTile(
                tilePadding: EdgeInsets.only(right: 25.w),
                title: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text("SỞ THÍCH", style: headerStyle),
                    SizedBox(
                      child: MaterialButton(
                        shape: CircleBorder(),
                        minWidth: 100.w,
                        onPressed: () {
                          setState(() {
                            if (checkUser) editUser = !editUser;
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
      padding: EdgeInsets.symmetric(horizontal: 50.w),
      child: Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Theme(
            data: Theme.of(context).copyWith(dividerColor: Colors.transparent),
            child: ExpansionTile(
                tilePadding: EdgeInsets.only(right: 25.w),
                title: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text("TÌM KIẾM", style: headerStyle),
                    SizedBox(
                      child: MaterialButton(
                        shape: CircleBorder(),
                        minWidth: 100.w,
                        onPressed: () {
                          setState(() {
                            if (checkUser) editUser = !editUser;
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
            contentPadding: EdgeInsets.symmetric(horizontal: 30.w),
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
        modalType: S2ModalType.bottomSheet,
        modalFilter: true,
        modalFilterAuto: true,
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
      contentPadding: EdgeInsets.only(bottom: 5.h),
      suffixText: suff,
      disabledBorder: editUser
          ? UnderlineInputBorder(
              borderSide: BorderSide(
                style: BorderStyle.solid,
                color: Theme.of(context).secondaryHeaderColor,
                width: 2.h,
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
          contentPadding: EdgeInsets.symmetric(horizontal: 30.w),
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
              contentPadding: EdgeInsets.only(bottom: 5.h),
              enabledBorder: editUser
                  ? UnderlineInputBorder(
                      borderSide: BorderSide(
                        style: BorderStyle.solid,
                        color: Theme.of(context).secondaryHeaderColor,
                        width: 2.h,
                      ),
                    )
                  : null,
            ),
          ));
      children.add(child);
      child = SizedBox(height: 30.h);
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
    child = SizedBox(height: 30.h);
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
    child = SizedBox(height: 30.h);
    children.add(child);
    //Ngày sinh
    child = ListTile(
      contentPadding: EdgeInsets.symmetric(horizontal: 30.w),
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
          contentPadding: EdgeInsets.only(bottom: 5.h),
          enabledBorder: editUser
              ? UnderlineInputBorder(
                  borderSide: BorderSide(
                    style: BorderStyle.solid,
                    color: Theme.of(context).secondaryHeaderColor,
                    width: 2.w,
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
    child = SizedBox(height: 30.h);
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
    child = SizedBox(height: 30.h);
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
    child = SizedBox(height: 30.h);
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
    child = SizedBox(height: 30.h);
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
      child = SizedBox(height: 30.h);
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
      //print('${item.name} value : ' + item.featureDetailId.toString());
      child = textFieldEditInfomation(
        userProvider: userProfile,
        title: item.name,
        listItem: userProfile.getFeatureVM(item.featureId),
        changeValue: "features",
        featureId: item.featureId,
        value: item.featureDetailId.toString(),
      );
      children.add(child);
      child = SizedBox(height: 30.h);
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
    child = SizedBox(height: 30.h);
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
    child = SizedBox(height: 30.h);
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
      child = SizedBox(height: 30.h);
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
    if (editUser && checkUser) {
      return Consumer<UserProvider>(
        builder: (context, userProfile, child) {
          return Container(
            padding: EdgeInsets.symmetric(horizontal: 80.w, vertical: 10.h),
            color: Colors.white,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                Container(
                  padding: EdgeInsets.symmetric(vertical: 0.0),
                  decoration: BoxDecoration(
                    gradient: colorApp,
                    borderRadius: BorderRadius.circular(80.sp),
                  ),
                  child: MaterialButton(
                    child: Row(
                      crossAxisAlignment: CrossAxisAlignment.center,
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          Icons.cloud_upload,
                        ),
                        SizedBox(width: 20.w),
                        Text(
                          "Cập nhật",
                          style: TextStyle(
                            color: Colors.white,
                            fontSize: 60.sp,
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
                    padding: EdgeInsets.all(40.sp),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(60.sp),
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
  final Function onPressed;
  final String userId;
  final bool clickButton;

  FollowAndFavorite(
      {this.onPressed,
      this.icon,
      this.titleButton,
      this.checkUser,
      this.userId,
      this.clickButton});
  @override
  _FollowAndFavoriteState createState() => _FollowAndFavoriteState();
}

class _FollowAndFavoriteState extends State<FollowAndFavorite> {
  @override
  Widget build(BuildContext context) {
    return RaisedButton(
      shape: !widget.clickButton
          ? RoundedRectangleBorder(side: BorderSide(color: Colors.white))
          : RoundedRectangleBorder(
              side: BorderSide(color: Theme.of(context).secondaryHeaderColor)),
      onPressed: widget.checkUser ? () {} : widget.onPressed,
      color: Colors.white,
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceAround,
        children: [
          Container(
            child: Icon(
              !widget.clickButton ? widget.icon[0] : widget.icon[1],
              color: !widget.clickButton
                  ? Colors.blueGrey.shade600
                  : Theme.of(context).secondaryHeaderColor,
            ),
          ),
          SizedBox(
            width: 10.w,
          ),
          Container(
            width: 180.w,
            child: Text(
              widget.titleButton,
              style: kFunctionTextStyle.copyWith(
                color: Colors.blueGrey.shade600,
                fontSize: 40.sp,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
