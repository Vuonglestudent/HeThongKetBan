import 'dart:async';
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/MessageProvider.dart';
import 'package:tinder_clone/Screens/InPersonChat_Screen.dart';
import 'package:tinder_clone/Screens/Zinger_Screen.dart';
import 'package:tinder_clone/Widgets/ErrorData.dart';

class MessagesScreen extends StatefulWidget {
  @override
  _MessagesScreenState createState() => _MessagesScreenState();
}

class _MessagesScreenState extends State<MessagesScreen> {
  TextEditingController _fieldController;
  MessageProvider message;
  bool loadData = false;

  Future<bool> loadDataMessage(BuildContext context) async {
    if (loadData == false && message.friendList.length == 0) {
      loadData = await message.getFriendList();
      print(loadData);
    } else {
      Future.delayed(Duration(seconds: 1), () => loadData = true);
    }
    return loadData;
  }

  @override
  void initState() {
    super.initState();
    _fieldController = new TextEditingController();
  }

  @override
  void dispose() {
    message.isExistScreenMessage = false;
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    message = Provider.of<MessageProvider>(context);
    message.isExistScreenMessage = true;

    return Container(
      child: Consumer<MessageProvider>(
        builder: (context, message, child) {
          return FutureBuilder<bool>(
            future: loadDataMessage(context),
            builder: (context, AsyncSnapshot<bool> snapshot) {
              Widget child;
              if (snapshot.hasData) {
                child = ListView(
                  children: <Widget>[
                    SizedBox(
                      height: 30.h,
                    ),
                    //Search friends
                    Container(
                      padding: EdgeInsets.symmetric(horizontal: 10.w),
                      child: TextField(
                        controller: _fieldController,
                        cursorRadius: Radius.circular(10.0.sp),
                        cursorColor: Theme.of(context).primaryColor,
                        style: TextStyle(fontSize: 50.sp),
                        onChanged: (value) {
                          message.searchFriendMessage(value);
                        },
                        decoration: InputDecoration(
                          hintText: "Tìm kiếm bạn bè",
                          border: InputBorder.none,
                          prefixIcon: Icon(
                            Icons.search,
                            color: Theme.of(context).primaryColor,
                          ),
                          contentPadding: EdgeInsets.only(
                            left: 35.w,
                            top: 40.h,
                            bottom: 30.h,
                          ),
                        ),
                      ),
                    ),
                    Divider(
                      height: 1.0,
                      color: Theme.of(context).accentColor,
                      indent: 120.w,
                      endIndent: 30.w,
                    ),
                    //<end>
                    // SizedBox(
                    //   height: 30.h,
                    // ),
                    //Friend List <start>
                    // Padding(
                    //   padding: EdgeInsets.symmetric(horizontal: 30.w),
                    //   child: Column(
                    //     children: <Widget>[
                    //       Align(
                    //         alignment: Alignment.topLeft,
                    //         child: Text(
                    //           "Users Follower",
                    //           style: TextStyle(
                    //               fontSize: 45.sp,
                    //               fontWeight: FontWeight.w600,
                    //               color:
                    //                   Theme.of(context).secondaryHeaderColor),
                    //         ),
                    //       ),
                    //       SizedBox(
                    //         height: 30.h,
                    //       ),
                    //     ],
                    //   ),
                    // ),
                    // Container(
                    //   height: 250.h,
                    //   child: ListView.builder(
                    //       itemCount: 10,
                    //       scrollDirection: Axis.horizontal,
                    //       itemBuilder: (context, index) {
                    //         return Center(
                    //           child: Container(
                    //             margin: EdgeInsets.symmetric(horizontal: 20.w),
                    //             width: 150.h,
                    //             height: 150.h,
                    //             decoration: BoxDecoration(
                    //                 borderRadius: BorderRadius.circular(90),
                    //                 color: Colors.blueGrey.shade50),
                    //           ),
                    //         );
                    //       }),
                    // ),
                    // Divider(
                    //   height: 1,
                    //   thickness: 2,
                    //   color: Colors.blueGrey.shade50,
                    // ),
                    //<end>

                    SizedBox(
                      height: 20.h,
                    ),
                    //List Message Friend <start>
                    Padding(
                      padding: EdgeInsets.symmetric(horizontal: 30.w),
                      child: Align(
                        alignment: Alignment.topLeft,
                        child: Text(
                          "Messages",
                          style: TextStyle(
                              fontSize: 45.sp,
                              fontWeight: FontWeight.w600,
                              color: Theme.of(context).secondaryHeaderColor),
                        ),
                      ),
                    ),
                    SizedBox(
                      height: 20.h,
                    ),
                    ListBody(
                      children:
                          List.generate(message.friendList.length, (index) {
                        return GestureDetector(
                          onTap: () {
                            Navigator.push(
                              context,
                              MaterialPageRoute(
                                builder: (context) => InPersonChatScreen(
                                  user: message.friendList[index],
                                ),
                              ),
                            ).then((value) => message.saveUserWhenExistChat());
                          },
                          child: Column(
                            children: <Widget>[
                              Container(
                                height: 230.h,
                                margin: EdgeInsets.symmetric(horizontal: 40.w),
                                child: Row(
                                  children: <Widget>[
                                    Container(
                                      height: 180.h,
                                      width: 180.h,
                                      decoration: BoxDecoration(
                                        borderRadius:
                                            BorderRadius.circular(120.sp),
                                      ),
                                      child: ClipRRect(
                                        borderRadius:
                                            BorderRadius.circular(120.sp),
                                        child: Image(
                                            fit: BoxFit.cover,
                                            height: 80.h,
                                            width: 80.h,
                                            image: NetworkImage(message
                                                .friendList[index]
                                                .userInfo
                                                .avatarPath)),
                                      ),
                                    ),
                                    SizedBox(
                                      width: 25.w,
                                    ),
                                    Expanded(
                                        child: Column(
                                      mainAxisAlignment:
                                          MainAxisAlignment.center,
                                      crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                      children: <Widget>[
                                        SizedBox(
                                          height: 55.h,
                                        ),
                                        Text(
                                          message.friendList[index].userInfo
                                              .fullName,
                                          style: TextStyle(
                                              fontSize: 50.sp,
                                              fontWeight: FontWeight.w700),
                                        ),
                                        SizedBox(
                                          height: 15.h,
                                        ),
                                        Expanded(
                                          child: Row(
                                            mainAxisAlignment:
                                                MainAxisAlignment.spaceBetween,
                                            crossAxisAlignment:
                                                CrossAxisAlignment.start,
                                            children: [
                                              Container(
                                                width: 450.w,
                                                child: Text(
                                                  message.friendList[index]
                                                      .firstMessage,
                                                  maxLines: 1,
                                                  overflow:
                                                      TextOverflow.ellipsis,
                                                  style: TextStyle(
                                                      color: message
                                                              .friendList[index]
                                                              .firstSeenMessage
                                                          ? Colors.black87
                                                          : Colors
                                                              .grey.shade500,
                                                      fontSize: 45.sp,
                                                      fontWeight: message
                                                              .friendList[index]
                                                              .firstSeenMessage
                                                          ? FontWeight.w600
                                                          : FontWeight.w400),
                                                ),
                                              ),
                                              Text(
                                                message.friendList[index]
                                                    .firstSendAt,
                                                overflow: TextOverflow.ellipsis,
                                                style: TextStyle(
                                                    color: Colors.grey.shade500,
                                                    fontSize: 45.sp,
                                                    fontStyle: FontStyle.italic,
                                                    fontWeight:
                                                        FontWeight.w400),
                                              )
                                            ],
                                          ),
                                        ),
                                      ],
                                    ))
                                  ],
                                ),
                              ),
                              Divider(height: 1, indent: 300.w, endIndent: 20.w)
                            ],
                          ),
                        );
                      }),
                    )
                    //<end>
                  ],
                );
              } else if (snapshot.hasError) {
                child = ErrorData(
                  titleError: "Try again later",
                );
              } else if (!loadData) {
                child = SearchMatching(
                  atCenter: true,
                  chng: true,
                  title: "Lấy danh sách nhắn tin...",
                );
              }
              return child != null
                  ? child
                  : SearchMatching(
                      atCenter: true,
                      chng: true,
                      title: "Lấy danh sách nhắn tin...",
                    );
            },
          );
        },
      ),
    );
  }
}

//                    Expanded(
//                     child: Column(
//                     children: <Widget>[
//                        SizedBox(
//                         height: ScreenUtil().setHeight(100.0),
//                       ),
//                        Image(
//                           width: ScreenUtil().setWidth(600),
//                           height: ScreenUtil().setHeight(400),
//                           fit: BoxFit.cover,
//                           image: new AssetImage('assets/images/sorry.png')),
//                        SizedBox(
//                         height: ScreenUtil().setHeight(50.0),
//                       ),
//                        Text(
//                         "Check back later",
//                         style: new TextStyle(
//                             wordSpacing: 1.5,
//                             fontSize: ScreenUtil().setSp(75.0),
//                             fontWeight: FontWeight.w400),
//                       ),
//                        SizedBox(
//                         height: ScreenUtil().setWidth(20.0),
//                       ),
//                        Padding(
//                         padding: EdgeInsets.symmetric(
//                             horizontal: ScreenUtil().setWidth(60.0)),
//                         child: new Text(
//                             "We couldn't find any social activity for your matches. Try again later",
//                             textAlign: TextAlign.center,
//                             style: new TextStyle(
//                                 fontSize: ScreenUtil().setSp(40.0),
//                                 fontWeight: FontWeight.w300,
//                                 color: Colors.grey.shade600)),
//                       )
//                     ],
//                   ),)
