import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/NotificationsProvider.dart';
import 'package:tinder_clone/Screens/ProfileDetail_Screen.dart';
import 'package:tinder_clone/Screens/Zinger_Screen.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class NotificationsScreen extends StatefulWidget {
  static const String id = 'Notifications_Screen';
  @override
  _NotificationsScreenState createState() => _NotificationsScreenState();
}

class _NotificationsScreenState extends State<NotificationsScreen> {
  ScrollController controller;
  bool getNotification = false;
  bool isLoad = false;
  NotificationsProvider notifi;
  Future<bool> getNotifications(NotificationsProvider notifications) async {
    if (!getNotification) {
      getNotification = await notifications.getNotification();
    }
    return getNotification;
  }

  @override
  void initState() {
    super.initState();
    controller = new ScrollController()..addListener(_scrollListener);
  }

  @override
  void dispose() {
    controller.removeListener(_scrollListener);
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    notifi = Provider.of<NotificationsProvider>(context);
    return Scaffold(
      body: Consumer<NotificationsProvider>(
        builder: (context, notifications, child) {
          return FutureBuilder(
            future: getNotifications(notifications),
            builder: (context, AsyncSnapshot<bool> snapshot) {
              Widget child;
              if (snapshot.hasData && getNotification) {
                child = Scrollbar(
                  child: ListView(
                    controller: controller,
                    children: [
                      ListBody(
                        children: List.generate(
                          notifications.listNotifications.length,
                          (index) {
                            var authen =
                                Provider.of<AuthenticationProvider>(context);
                            return GestureDetector(
                              onTap: () {
                                Navigator.push(
                                  context,
                                  MaterialPageRoute(
                                    builder: (context) => ProfileDetails(
                                        authenticationUserId:
                                            authen.getUserInfo.id,
                                        userId: notifications
                                            .listNotifications[index].fromId),
                                  ),
                                );
                              },
                              child: Column(
                                children: <Widget>[
                                  SizedBox(
                                    height: 15.h,
                                  ),
                                  Container(
                                    height: notifications
                                                .listNotifications[index]
                                                .type ==
                                            'relationship'
                                        ? 280.h
                                        : 210.h,
                                    margin:
                                        EdgeInsets.symmetric(horizontal: 40.w),
                                    child: Row(
                                      children: <Widget>[
                                        Container(
                                          height: 160.h,
                                          width: 160.h,
                                          decoration: BoxDecoration(
                                            borderRadius:
                                                BorderRadius.circular(120.sp),
                                          ),
                                          child: ClipRRect(
                                            borderRadius:
                                                BorderRadius.circular(120.sp),
                                            child: Image(
                                              fit: BoxFit.cover,
                                              image: NetworkImage(notifications
                                                  .listNotifications[index]
                                                  .avatar),
                                            ),
                                          ),
                                        ),
                                        SizedBox(
                                          width: 20.w,
                                        ),
                                        Expanded(
                                            child: Padding(
                                          padding: EdgeInsets.only(left: 40.w),
                                          child: Column(
                                            mainAxisAlignment:
                                                MainAxisAlignment.start,
                                            crossAxisAlignment:
                                                CrossAxisAlignment.start,
                                            children: <Widget>[
                                              SizedBox(
                                                height: 30.h,
                                              ),
                                              Row(
                                                mainAxisAlignment:
                                                    MainAxisAlignment
                                                        .spaceBetween,
                                                children: [
                                                  Container(
                                                    width: 430.w,
                                                    child: Text(
                                                      notifications
                                                          .listNotifications[
                                                              index]
                                                          .fullName,
                                                      maxLines: 1,
                                                      overflow:
                                                          TextOverflow.ellipsis,
                                                      style: TextStyle(
                                                        fontSize: 45.sp,
                                                        fontWeight:
                                                            FontWeight.w700,
                                                      ),
                                                    ),
                                                  ),
                                                  Text(
                                                    DateFormat('hh:mm - dd.MMM')
                                                        .format(notifications
                                                            .listNotifications[
                                                                index]
                                                            .createdAt),
                                                  )
                                                ],
                                              ),
                                              SizedBox(
                                                height: 10.h,
                                              ),
                                              Text(
                                                notifications
                                                    .listNotifications[index]
                                                    .content,
                                                style: TextStyle(
                                                    fontSize: 40.sp,
                                                    fontWeight:
                                                        FontWeight.w400),
                                              ),
                                              notifications
                                                          .listNotifications[
                                                              index]
                                                          .type ==
                                                      'relationship'
                                                  ? Expanded(
                                                      child: Row(
                                                        mainAxisAlignment:
                                                            MainAxisAlignment
                                                                .spaceBetween,
                                                        crossAxisAlignment:
                                                            CrossAxisAlignment
                                                                .start,
                                                        children: [
                                                          Expanded(
                                                            child: RaisedButton(
                                                              color: Theme.of(
                                                                      context)
                                                                  .accentColor,
                                                              child: Text(
                                                                  "Đồng ý",
                                                                  style:
                                                                      TextStyle(
                                                                    color: Colors
                                                                        .white,
                                                                    fontWeight:
                                                                        FontWeight
                                                                            .bold,
                                                                  )),
                                                              onPressed: () {
                                                                print("Dong y");
                                                                notifications.onAccept(
                                                                    notifications
                                                                        .listNotifications[
                                                                            index]
                                                                        .fromId,
                                                                    index);
                                                              },
                                                            ),
                                                          ),
                                                          SizedBox(
                                                            width: 20.w,
                                                          ),
                                                          Expanded(
                                                              child:
                                                                  RaisedButton(
                                                            color: Theme.of(
                                                                    context)
                                                                .secondaryHeaderColor,
                                                            child: Text(
                                                              "Từ chối",
                                                              style: TextStyle(
                                                                color: Colors
                                                                    .white,
                                                                fontWeight:
                                                                    FontWeight
                                                                        .bold,
                                                              ),
                                                            ),
                                                            onPressed:
                                                                () async {
                                                              notifications.onDecline(
                                                                  notifications
                                                                      .listNotifications[
                                                                          index]
                                                                      .fromId,
                                                                  index);
                                                            },
                                                          ))
                                                        ],
                                                      ),
                                                    )
                                                  : Expanded(
                                                      child: Container(),
                                                    ),
                                              SizedBox(
                                                height: 10.h,
                                              )
                                            ],
                                          ),
                                        ))
                                      ],
                                    ),
                                  ),
                                  Divider(
                                    color: Theme.of(context).accentColor,
                                    height: 1,
                                    indent: 300.w,
                                    endIndent: 20.w,
                                  ),
                                  Divider(
                                    color: Theme.of(context).accentColor,
                                    height: 1,
                                    indent: 300.w,
                                    endIndent: 20.w,
                                  ),
                                ],
                              ),
                            );
                          },
                        ),
                      ),
                      isLoad
                          ? Container(
                              height: 200.h,
                              width: double.infinity,
                              child: SearchMatching(
                                atCenter: true,
                                chng: true,
                                title: "Tải thêm dữ liệu",
                              ),
                            )
                          : Container(),
                    ],
                  ),
                );
              } else if (!getNotification) {
                child = SearchMatching(
                  atCenter: true,
                  chng: true,
                  title: "Lấy danh sách thông báo ...",
                );
              } else if (snapshot.hasData && !getNotification) {
                child = Container(
                  child: Text("Lỗi rồi"),
                );
              }
              return child != null
                  ? child
                  : SearchMatching(
                      atCenter: true,
                      chng: true,
                      title: "Lấy danh sách thông báo ...",
                    );
            },
          );
        },
      ),
    );
  }

  void _scrollListener() {
    if (controller.position.extentAfter < 100) {
      setState(() {
        isLoad = true;
        notifi.getNotification();
      });
    } else {
      setState(() {
        isLoad = false;
        //items.addAll(new List.generate(42, (index) => 'Inserted $index'));
      });
    }
  }
}
