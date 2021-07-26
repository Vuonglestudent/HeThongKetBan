import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:modal_progress_hud/modal_progress_hud.dart';
import 'package:multi_image_picker/multi_image_picker.dart';
import 'package:provider/provider.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Screens/ProfileDetail_Screen.dart';
import 'package:tinder_clone/Screens/Setting_Screen.dart';
import 'package:tinder_clone/Widgets/FullScreenImage.dart';
import 'package:tinder_clone/Widgets/Notification.dart';
import 'package:tinder_clone/Widgets/UploadApiImagesButton.dart';
import 'package:tinder_clone/contants.dart';
import 'package:tinder_clone/Widgets/AddImagesProfile.dart';
import 'package:tinder_clone/Widgets/IconsProfileExpanded.dart';
import 'package:tinder_clone/Widgets/CarouselProfile.dart';
import 'package:tinder_clone/Widgets/MyClipper.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';

class ProfileScreen extends StatefulWidget {
  static const String id = 'Profile_Screen';
  @override
  _ProfileScreenState createState() => _ProfileScreenState();
}

class _ProfileScreenState extends State<ProfileScreen> {
  TextEditingController controller;
  bool focusTextField = false;
  UserProvider userProvider;
  @override
  void initState() {
    super.initState();
    controller = new TextEditingController();
  }

  void _showPopupMenuAvatar(
      UserProvider user, String id, AuthenticationProvider authen) async {
    await showMenu(
      context: context,
      position: RelativeRect.fromLTRB(200.w, 500.h, 250.w, 0),
      items: [
        PopupMenuItem(
          child: TextButton(
            onPressed: () async {
              var response = await user.updateAvatar(id);
              response.stream.transform(utf8.decoder).listen((value) {
                try {
                  if (response.statusCode < 200 || response.statusCode >= 400) {
                    notificationWidget(
                      context,
                      typeNotication: AlertType.error,
                      title: "Cập nhật ảnh đại diện",
                      desc: "Ảnh đại diện cập nhật thất bại",
                      textButton: "Hủy",
                    ).show();
                  } else {
                    setState(() {
                      Provider.of<AuthenticationProvider>(context,
                              listen: false)
                          .getUserInfo
                          .avatarPath = (jsonDecode(value))['avatarPath'];
                    });
                  }
                } catch (e) {
                  print("error update path avatar: " + e.toString());
                }
              });
            },
            child: Container(
              width: 450.w,
              child: Row(
                children: [
                  Icon(
                    Icons.edit_outlined,
                    color: Colors.grey.shade600,
                    size: 45.w,
                  ),
                  SizedBox(
                    width: 35.w,
                  ),
                  Text(
                    "Đổi hình đại diện",
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
          child: TextButton(
            onPressed: () async {
              Navigator.push(context, MaterialPageRoute(builder: (_) {
                return FullScreenImage(
                  imageUrl: authen.getUserInfo.avatarPath,
                  tag: 0,
                  galleryItems: null,
                );
              }));
            },
            child: Container(
              width: 450.w,
              child: Row(
                children: [
                  Icon(
                    Icons.image_outlined,
                    color: Colors.grey.shade600,
                    size: 45.w,
                  ),
                  SizedBox(
                    width: 35.w,
                  ),
                  Text(
                    "Xem hình đại diện",
                    style: TextStyle(
                      color: Colors.grey.shade600,
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ],
      elevation: 8.0,
    );
  }

  Widget buildGridView(List<Asset> images) {
    if (images != null)
      return Center(
        child: GridView.count(
          crossAxisCount: 3,
          crossAxisSpacing: 2.0,
          mainAxisSpacing: 2.0,
          children: List.generate(images.length, (index) {
            Asset asset = images[index];
            return MaterialButton(
              padding: EdgeInsets.zero,
              onPressed: () {
                userProvider.removeImage(index);
              },
              child: AssetThumb(
                asset: asset,
                width: 300,
                height: 300,
              ),
            );
          }),
        ),
      );
    else
      return Container(color: Colors.white);
  }

  @override
  Widget build(BuildContext context) {
    userProvider = Provider.of<UserProvider>(context);
    return Consumer<AuthenticationProvider>(
      builder: (context, authentication, child) {
        return ModalProgressHUD(
          inAsyncCall: Provider.of<Loading>(context).loading,
          child: Scaffold(
            body: Container(
              child: Stack(
                children: <Widget>[
                  Container(
                    height: double.infinity,
                    width: MediaQuery.of(context).size.width,
                    color: Colors.grey.shade100,
                  ),
                  Positioned(
                    child: SingleChildScrollView(
                      child: ClipPath(
                        clipBehavior: Clip.antiAlias,
                        clipper: MyClipper(),
                        child: Container(
                          height:
                              Provider.of<UserProvider>(context).isAddImages ==
                                      true
                                  ? MediaQuery.of(context).size.height * 0.89
                                  : MediaQuery.of(context).size.height * 0.725,
                          decoration: BoxDecoration(
                              color: Colors.white,
                              boxShadow: [
                                BoxShadow(
                                    color: Colors.grey,
                                    offset: Offset(1.0, 10.0),
                                    blurRadius: 10.0)
                              ]),
                          child: Column(
                            children: <Widget>[
                              Expanded(
                                flex: 4,
                                child: Container(
                                  child: Column(
                                    children: <Widget>[
                                      SizedBox(height: 50.h),
                                      MaterialButton(
                                        onPressed: () {
                                          _showPopupMenuAvatar(
                                            userProvider,
                                            authentication.getUserInfo.id,
                                            authentication,
                                          );
                                        },
                                        child: Container(
                                            width: 315.w,
                                            height: 300.h,
                                            child: CircleAvatar(
                                              backgroundImage: NetworkImage(
                                                  authentication
                                                      .getUserInfo.avatarPath),
                                            )),
                                      ),
                                      SizedBox(height: 15.h),
                                      Text(
                                        authentication.getUserInfo.fullName,
                                        style: kNameTextStyle,
                                      ),
                                      SizedBox(
                                        height: 25.h,
                                      ),
                                      Expanded(
                                          child: Row(
                                        children: <Widget>[
                                          IconsProfileExpanded(
                                            titleButton: "CÀI ĐẶT",
                                            icon: Icons.settings,
                                            onPressed: () => Navigator.push(
                                              context,
                                              MaterialPageRoute(
                                                builder: (context) =>
                                                    SettingScreen(),
                                              ),
                                            ),
                                          ),
                                          AddImagesProfile(),
                                          IconsProfileExpanded(
                                            titleButton: "HỒ SƠ",
                                            icon: Icons.article_outlined,
                                            onPressed: () => Navigator.push(
                                              context,
                                              MaterialPageRoute(
                                                builder: (context) =>
                                                    ProfileDetails(
                                                  authenticationUserId:
                                                      authentication
                                                          .getUserInfo.id,
                                                  userId: authentication
                                                      .getUserInfo.id,
                                                ),
                                              ),
                                            ),
                                          ),
                                        ],
                                      ))
                                    ],
                                  ),
                                ),
                              ),
                              Expanded(
                                flex: Provider.of<UserProvider>(context)
                                        .isAddImages
                                    ? 4
                                    : 3,
                                child: Container(
                                  child: Provider.of<UserProvider>(context)
                                          .isAddImages
                                      ? Column(
                                          children: [
                                            TextFieldDescription(
                                                controller: controller),
                                            Expanded(
                                              child: buildGridView(
                                                  Provider.of<UserProvider>(
                                                          context)
                                                      .getImages),
                                            ),
                                          ],
                                        )
                                      : Container(),
                                ),
                              ),
                              UploadApiImagesButton(
                                checkCreateButton:
                                    Provider.of<UserProvider>(context)
                                        .isAddImages,
                                controller: controller,
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                  ),
                  CarouselProfile(),
                ],
              ),
            ),
          ),
        );
      },
    );
  }
}

class TextFieldDescription extends StatelessWidget {
  const TextFieldDescription({
    Key key,
    @required this.controller,
  }) : super(key: key);

  final TextEditingController controller;

  @override
  Widget build(BuildContext context) {
    return Container(
      height: 150.h,
      child: Column(
        children: [
          Container(
            margin: EdgeInsets.symmetric(horizontal: 10.w),
            child: Container(
              decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(30.sp),
                color: Colors.white,
              ),
              margin: EdgeInsets.all(5.sp),
              child: TextField(
                controller: controller,
                decoration: new InputDecoration(
                    border: InputBorder.none,
                    focusedBorder: InputBorder.none,
                    enabledBorder: InputBorder.none,
                    errorBorder: InputBorder.none,
                    disabledBorder: InputBorder.none,
                    contentPadding:
                        EdgeInsets.symmetric(horizontal: 35.w, vertical: 10.h),
                    hintText: "Điền mô tả hình ảnh"),
              ),
            ),
            decoration: BoxDecoration(
              borderRadius: BorderRadius.circular(30.sp),
              gradient: LinearGradient(
                colors: [
                  Color.fromRGBO(251, 195, 182, 1.0),
                  Color.fromRGBO(219, 138, 172, 1.0),
                  Color.fromRGBO(174, 61, 158, 1.0),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}
