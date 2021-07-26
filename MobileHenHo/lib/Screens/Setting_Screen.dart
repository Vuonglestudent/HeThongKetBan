/**
 * Author: Damodar Lohani
 * profile: https://github.com/lohanidamodar
  */

import 'package:flutter/material.dart';
import 'package:page_transition/page_transition.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Screens/BlockUsersScreen.dart';
import 'package:tinder_clone/Screens/ChangePassword_Screen.dart';
import 'package:tinder_clone/Screens/LoginGmail_Screen.dart';
import 'package:tinder_clone/Screens/SettingLocation_Screen.dart';
import 'package:tinder_clone/contants.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class SettingScreen extends StatefulWidget {
  @override
  _SettingScreenState createState() => _SettingScreenState();
}

class _SettingScreenState extends State<SettingScreen> {
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
          'CÀI ĐẶT',
        ),
      ),
      body: Container(
        child: Consumer<AuthenticationProvider>(
          builder: (context, authen, child) {
            return SingleChildScrollView(
              padding: EdgeInsets.all(40.h),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  Text(
                    "TÀI KHOẢN",
                    style: headerStyle,
                  ),
                  SizedBox(height: 20.h),
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
                        ListTile(
                          leading: CircleAvatar(
                            backgroundImage:
                                NetworkImage(authen.getUserInfo.avatarPath),
                          ),
                          title: Text(
                            authen.getUserInfo.fullName,
                            style: Theme.of(context)
                                .textTheme
                                .headline6
                                .copyWith(fontWeight: FontWeight.w700),
                          ),
                          onTap: () {},
                        ),
                        buildDivider(),
                        // Đổi mật khẩu
                        ListTileButton(
                          icon: Icons.account_box,
                          title: "Đổi mật khẩu",
                          onTaped: () {
                            Navigator.push(
                              context,
                              PageTransition(
                                type: PageTransitionType.rightToLeft,
                                child: ChangePasswordScreen(),
                              ),
                            );
                          },
                        ),
                        // Danh sách block
                        ListTileButton(
                          icon: Icons.person_add_disabled,
                          title: "Danh sách chặn",
                          onTaped: () {
                            Navigator.push(
                              context,
                              PageTransition(
                                type: PageTransitionType.rightToLeft,
                                child: BlockUsersScreen(),
                              ),
                            );
                          },
                        ),
                      ],
                    ),
                  ),
                  SizedBox(height: 50.h),
                  Text(
                    "TÙY CHỌN",
                    style: headerStyle,
                  ),
                  SizedBox(height: 20.h),
                  Card(
                    margin: EdgeInsets.symmetric(
                      vertical: 10.h,
                      horizontal: 0,
                    ),
                    child: Column(
                      children: <Widget>[
                        ListTileButton(
                          icon: Icons.location_on,
                          title: "Cài đặt vị trí tìm kiếm",
                          onTaped: () {
                            Navigator.push(
                              context,
                              PageTransition(
                                type: PageTransitionType.rightToLeft,
                                child: SettingLocationScreen(),
                              ),
                            );
                          },
                        ),
                        ListTileButton(
                          icon: Icons.warning,
                          title: "Báo cáo vấn đề kỹ thuật",
                          onTaped: () {},
                        ),
                        ListTileButton(
                          icon: Icons.help_outlined,
                          title: "Trợ giúp",
                          onTaped: () {},
                        ),
                      ],
                    ),
                  ),
                  Card(
                    margin: EdgeInsets.symmetric(
                      vertical: 30.h,
                      horizontal: 0,
                    ),
                    child: ListTileButton(
                      icon: Icons.exit_to_app,
                      title: "Đăng xuất",
                      onTaped: () {
                        authen.logoutSystem(context);
                        Navigator.pushAndRemoveUntil(
                          context,
                          MaterialPageRoute(
                              builder: (BuildContext context) =>
                                  LoginGmailScreen()),
                          ModalRoute.withName('/'),
                        );
                      },
                    ),
                  ),
                  SizedBox(height: 100.h),
                ],
              ),
            );
          },
        ),
      ),
    );
  }
}

class ListTileButton extends StatelessWidget {
  final IconData icon;
  final String title;
  final Function onTaped;
  ListTileButton(
      {@required this.icon, @required this.title, @required this.onTaped});

  @override
  Widget build(BuildContext context) {
    return ListTile(
      leading: Icon(
        icon,
        size: 90.sp,
      ),
      title: Text(
        title,
        style: Theme.of(context).textTheme.bodyText2.copyWith(fontSize: 50.sp),
      ),
      onTap: onTaped,
    );
  }
}
