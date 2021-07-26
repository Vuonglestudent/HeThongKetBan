import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/NotificationsProvider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Screens/Zinger_Screen.dart';
import 'package:tinder_clone/contants.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class BlockUsersScreen extends StatefulWidget {
  @override
  _BlockUsersScreenState createState() => _BlockUsersScreenState();
}

class _BlockUsersScreenState extends State<BlockUsersScreen> {
  bool isLoadData = false;
  Future<bool> getBlockList(AuthenticationProvider authen) async {
    if (!isLoadData) {
      isLoadData = await authen.getBlockList();
    }
    return isLoadData;
  }

  @override
  Widget build(BuildContext context) {
    var notification = Provider.of<NotificationsProvider>(context);

    return Scaffold(
      backgroundColor: Colors.grey.shade200,
      appBar: AppBar(
        flexibleSpace: Container(
          decoration: BoxDecoration(
            gradient: colorApp,
          ),
        ),
        title: Text(
          'DANH SÁCH CHẶN',
        ),
      ),
      body: Container(
        child: Consumer<AuthenticationProvider>(
          builder: (context, authen, child) {
            return FutureBuilder(
              future: getBlockList(authen),
              builder: (context, AsyncSnapshot<bool> snapshot) {
                Widget child;
                if (snapshot.hasData) {
                  child = Container(
                    padding:
                        EdgeInsets.symmetric(vertical: 50.h, horizontal: 100.w),
                    child: SafeArea(
                      child: ListView(
                        children: [
                          ListBody(
                            children:
                                List.generate(authen.blockList.length, (index) {
                              return Consumer<UserProvider>(
                                builder: (context, user, child) {
                                  return Card(
                                    child: Column(
                                      children: <Widget>[
                                        SizedBox(
                                          height: 15.h,
                                        ),
                                        Container(
                                          height: 230.h,
                                          margin: EdgeInsets.symmetric(
                                              horizontal: 40.w),
                                          child: Row(
                                            children: <Widget>[
                                              Container(
                                                height: 200.h,
                                                width: 200.h,
                                                decoration: BoxDecoration(
                                                  borderRadius:
                                                      BorderRadius.circular(
                                                          120.sp),
                                                ),
                                                child: ClipRRect(
                                                  borderRadius:
                                                      BorderRadius.circular(
                                                          120.sp),
                                                  child: Image(
                                                    fit: BoxFit.cover,
                                                    image: NetworkImage(authen
                                                        .blockList[index]
                                                        .avatarPath),
                                                  ),
                                                ),
                                              ),
                                              SizedBox(
                                                width: 25.w,
                                              ),
                                              Expanded(
                                                  child: Padding(
                                                padding:
                                                    EdgeInsets.only(left: 40.w),
                                                child: Column(
                                                  mainAxisAlignment:
                                                      MainAxisAlignment.start,
                                                  crossAxisAlignment:
                                                      CrossAxisAlignment.start,
                                                  children: <Widget>[
                                                    SizedBox(
                                                      height: 30.h,
                                                    ),
                                                    Text(
                                                      authen.blockList[index]
                                                          .fullName,
                                                      maxLines: 1,
                                                      overflow:
                                                          TextOverflow.ellipsis,
                                                      style: TextStyle(
                                                          fontSize: 55.sp,
                                                          fontWeight:
                                                              FontWeight.w700),
                                                    ),
                                                    SizedBox(
                                                      height: 10.h,
                                                    ),
                                                    Expanded(
                                                      child: Row(
                                                        mainAxisAlignment:
                                                            MainAxisAlignment
                                                                .spaceBetween,
                                                        crossAxisAlignment:
                                                            CrossAxisAlignment
                                                                .start,
                                                        children: [
                                                          Expanded(
                                                            child:
                                                                MaterialButton(
                                                              color: Theme.of(
                                                                      context)
                                                                  .primaryColor,
                                                              child: Row(
                                                                mainAxisAlignment:
                                                                    MainAxisAlignment
                                                                        .center,
                                                                crossAxisAlignment:
                                                                    CrossAxisAlignment
                                                                        .end,
                                                                children: [
                                                                  Icon(
                                                                    Icons
                                                                        .lock_open,
                                                                    color: Theme.of(
                                                                            context)
                                                                        .accentColor,
                                                                  ),
                                                                  SizedBox(
                                                                      width:
                                                                          15.w),
                                                                  Text(
                                                                    "Bỏ Chặn",
                                                                    style:
                                                                        TextStyle(
                                                                      color: Colors
                                                                          .white,
                                                                      fontWeight:
                                                                          FontWeight
                                                                              .bold,
                                                                      fontSize:
                                                                          45.sp,
                                                                    ),
                                                                  ),
                                                                ],
                                                              ),
                                                              onPressed: () {
                                                                authen
                                                                    .openBlockUser(
                                                                  context,
                                                                  user,
                                                                  notification,
                                                                  index,
                                                                );
                                                              },
                                                            ),
                                                          ),
                                                        ],
                                                      ),
                                                    ),
                                                  ],
                                                ),
                                              ))
                                            ],
                                          ),
                                        ),
                                      ],
                                    ),
                                  );
                                },
                              );
                            }),
                          ),
                        ],
                      ),
                    ),
                  );
                } else if (!isLoadData) {
                  child = SearchMatching(
                    atCenter: true,
                    chng: true,
                    title: "Lấy danh sách bị chặn...",
                  );
                }
                return child;
              },
            );
          },
        ),
      ),
    );
  }
}
