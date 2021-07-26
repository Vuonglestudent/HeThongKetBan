import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_tindercard/flutter_tindercard.dart';
import 'package:intl/intl.dart';
import 'package:modal_progress_hud/modal_progress_hud.dart';
import 'package:page_transition/page_transition.dart';
import 'package:provider/provider.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:tinder_clone/HomePage.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Screens/ListImage_Screen.dart';
import 'package:tinder_clone/Screens/ProfileDetail_Screen.dart';
import 'package:tinder_clone/Screens/SettingLocation_Screen.dart';
import 'package:tinder_clone/Widgets/Notification.dart';

class ZingerScreen extends StatefulWidget {
  final bool searchByLocation;
  ZingerScreen({@required this.searchByLocation});
  @override
  _ZingerScreenState createState() => _ZingerScreenState();
}

class _ZingerScreenState extends State<ZingerScreen>
    with SingleTickerProviderStateMixin {
  bool chng = true;
  bool atCenter = true;
  bool getUsersSimilar = false;
  CardController _cardController;
  bool isRefresh = false;
  int indexUser = 0;
  Future<bool> getUserSimilar(BuildContext context) async {
    if (!widget.searchByLocation) {
      if (getUsersSimilar == false) {
        getUsersSimilar =
            await Provider.of<UserProvider>(context).getUsersSimilar(
          Provider.of<AuthenticationProvider>(context).getUserInfo.id,
        );
      }
    } else {
      if (getUsersSimilar == false) {
        getUsersSimilar = await Provider.of<UserProvider>(context).findAround();
      }
    }
    return getUsersSimilar;
  }

  calculateAge(DateTime birthDate) {
    DateTime currentDate = DateTime.now();
    int age = currentDate.year - birthDate.year;
    int month1 = currentDate.month;
    int month2 = birthDate.month;
    if (month2 > month1) {
      age--;
    } else if (month1 == month2) {
      int day1 = currentDate.day;
      int day2 = birthDate.day;
      if (day2 > day1) {
        age--;
      }
    }
    return age;
  }

  @override
  Widget build(BuildContext context) {
    var authen = Provider.of<AuthenticationProvider>(context);
    return ModalProgressHUD(
      inAsyncCall: Provider.of<Loading>(context).loading,
      child: Consumer<UserProvider>(
        builder: (context, userProfiles, child) {
          return FutureBuilder<bool>(
            future: getUserSimilar(context),
            builder: (context, AsyncSnapshot<bool> snapshot) {
              Widget child;
              if (snapshot.hasData) {
                child = Stack(
                  children: <Widget>[
                    AnimatedContainer(
                      duration: Duration(milliseconds: 600),
                      curve: Curves.fastLinearToSlowEaseIn,
                      color: !atCenter
                          ? chng
                              ? Colors.pinkAccent.shade200
                              : Colors.tealAccent.shade200
                          : Colors.blue.shade50,
                    ),
                    Container(
                      margin: EdgeInsets.only(top: 20.sp),
                      child: Row(
                        crossAxisAlignment: CrossAxisAlignment.center,
                        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                        children: [
                          ButtonStyleFindFriends(
                            title: "Gợi ý tương đồng",
                            color: widget.searchByLocation
                                ? Colors.white
                                : Theme.of(context).secondaryHeaderColor,
                            onPressed: () {
                              Navigator.pushReplacement(
                                context,
                                PageRouteBuilder(
                                  pageBuilder:
                                      (context, animation1, animation2) =>
                                          HomePage(),
                                  transitionDuration: Duration(seconds: 0),
                                ),
                              );
                            },
                          ),
                          ButtonStyleFindFriends(
                            title: "Bạn bè gần đây",
                            color: widget.searchByLocation
                                ? Theme.of(context).secondaryHeaderColor
                                : Colors.white,
                            onPressed: () async {
                              if (authen.isSaveLocation)
                                Navigator.pushReplacement(
                                  context,
                                  PageRouteBuilder(
                                    pageBuilder:
                                        (context, animation1, animation2) =>
                                            HomePage(searchByLocation: true),
                                    transitionDuration: Duration(seconds: 0),
                                  ),
                                );
                              else {
                                notificationWidget(
                                  context,
                                  title: "Tìm kiếm theo vị trí",
                                  desc: "Tìm kiếm thất bại",
                                  textButton: "Hủy",
                                  typeNotication: AlertType.error,
                                ).show();
                              }
                            },
                          ),
                        ],
                      ),
                    ),
                    Positioned(
                      bottom: 0.h,
                      child: new Container(
                        height: 220.w,
                        width: MediaQuery.of(context).size.width,
                        child: new Row(
                          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                          children: <Widget>[
                            //Icon refresh
                            MaterialButton(
                              shape: CircleBorder(),
                              padding: EdgeInsets.all(0.sp),
                              minWidth: 0.sp,
                              onPressed: () {
                                print(widget.searchByLocation);
                                userProfiles
                                    .refreshUserPage(widget.searchByLocation);
                                Navigator.pushReplacement(
                                  context,
                                  PageRouteBuilder(
                                    pageBuilder:
                                        (context, animation1, animation2) =>
                                            HomePage(
                                                searchByLocation:
                                                    widget.searchByLocation),
                                    transitionDuration: Duration(seconds: 0),
                                  ),
                                );
                              },
                              child: new Container(
                                padding: EdgeInsets.all(15.w),
                                height: 80.h,
                                width: 80.h,
                                decoration: new BoxDecoration(
                                    boxShadow: [
                                      new BoxShadow(
                                          offset: new Offset(0.0, 0.0),
                                          color: Colors.grey),
                                      new BoxShadow(
                                          offset: new Offset(1.0, 1.0),
                                          color: Colors.grey,
                                          blurRadius: 5.0),
                                      new BoxShadow(
                                          offset: new Offset(-1.0, -1.0),
                                          color: Colors.white,
                                          blurRadius: 10.0)
                                    ],
                                    color: Colors.white,
                                    borderRadius: BorderRadius.circular(60.sp)),
                                child: new ShaderMask(
                                    child: new Image(
                                        image: new AssetImage(
                                            'assets/images/round.png')),
                                    blendMode: BlendMode.srcATop,
                                    shaderCallback: (Rect bounds) {
                                      return LinearGradient(
                                              colors: [
                                                Colors.amber.shade700,
                                                Colors.amber.shade400
                                              ],
                                              begin: Alignment.topRight,
                                              end: Alignment.bottomLeft,
                                              stops: [0.0, 1.0])
                                          .createShader(bounds);
                                    }),
                              ),
                            ),
                            //Icon Seen Image
                            MaterialButton(
                              shape: CircleBorder(),
                              padding: EdgeInsets.all(0.sp),
                              minWidth: 0.sp,
                              onPressed: userProfiles.usersSimilar.length == 0
                                  ? () {}
                                  : () async {
                                      await userProfiles
                                          .getNewImageSeenUserSimilar(
                                              userProfiles
                                                  .usersSimilar[indexUser].id);
                                      if (userProfiles
                                              .getImageSeenUserSimilar.length >
                                          0) {
                                        Navigator.push(
                                          context,
                                          MaterialPageRoute(
                                            builder: (BuildContext context) =>
                                                ListImageScreen(
                                              userId: userProfiles
                                                  .usersSimilar[indexUser].id,
                                            ),
                                          ),
                                        );
                                      } else {
                                        notificationWidget(
                                          context,
                                          typeNotication: AlertType.info,
                                          title: "Hình ảnh",
                                          desc:
                                              "Hiện tại người dùng chưa đăng bất kì ảnh nào.",
                                          textButton: "OK",
                                        ).show();
                                      }
                                    },
                              child: Container(
                                padding: EdgeInsets.all(25.sp),
                                height: 110.h,
                                width: 110.h,
                                decoration: new BoxDecoration(
                                    boxShadow: [
                                      new BoxShadow(
                                          offset: new Offset(0.0, 0.0),
                                          color: Colors.grey),
                                      new BoxShadow(
                                          offset: new Offset(1.0, 1.0),
                                          color: Colors.grey,
                                          blurRadius: 5.0),
                                      new BoxShadow(
                                          offset: new Offset(-1.0, -1.0),
                                          color: Colors.white,
                                          blurRadius: 10.0)
                                    ],
                                    color: Colors.white,
                                    borderRadius: BorderRadius.circular(60.sp)),
                                child: new ShaderMask(
                                    child: Icon(
                                      Icons.camera_alt,
                                      size: 65.h,
                                    ),
                                    blendMode: BlendMode.srcATop,
                                    shaderCallback: (Rect bounds) {
                                      return LinearGradient(
                                              colors: [
                                                Theme.of(context).accentColor,
                                                Theme.of(context).primaryColor
                                              ],
                                              begin: Alignment.topRight,
                                              end: Alignment.bottomLeft,
                                              stops: [0.0, 1.0])
                                          .createShader(bounds);
                                    }),
                              ),
                            ),
                            //Icon Follow
                            MaterialButton(
                              shape: CircleBorder(),
                              padding: EdgeInsets.all(0.sp),
                              minWidth: 0.sp,
                              onPressed: userProfiles.usersSimilar.length == 0
                                  ? () {}
                                  : () {
                                      userProfiles
                                          .setFollowedZingerPage(indexUser);
                                    },
                              child: Container(
                                height: 80.h,
                                width: 80.h,
                                decoration: new BoxDecoration(
                                    boxShadow: [
                                      new BoxShadow(
                                          offset: new Offset(0.0, 0.0),
                                          color: Colors.grey),
                                      new BoxShadow(
                                          offset: new Offset(1.0, 1.0),
                                          color: Colors.grey,
                                          blurRadius: 5.0),
                                      new BoxShadow(
                                          offset: new Offset(-1.0, -1.0),
                                          color: Colors.white,
                                          blurRadius: 10.0)
                                    ],
                                    color: Colors.white,
                                    borderRadius: BorderRadius.circular(60.sp)),
                                child: new ShaderMask(
                                    child: new Icon(
                                      indexUser <
                                                  userProfiles
                                                      .usersSimilar.length &&
                                              indexUser >= 0
                                          ? userProfiles.usersSimilar[indexUser]
                                                  .followed
                                              ? Icons.star
                                              : Icons.star_border
                                          : Icons.star_border,
                                      size: 65.h,
                                    ),
                                    blendMode: BlendMode.srcATop,
                                    shaderCallback: (Rect bounds) {
                                      return LinearGradient(
                                              colors: [
                                                Colors.blue.shade600,
                                                Colors.blue.shade300
                                              ],
                                              begin: Alignment.topRight,
                                              end: Alignment.bottomLeft,
                                              stops: [0.0, 1.0])
                                          .createShader(bounds);
                                    }),
                              ),
                            ),
                            //Icon Favorite
                            MaterialButton(
                              shape: CircleBorder(),
                              padding: EdgeInsets.all(0.sp),
                              minWidth: 0.sp,
                              onPressed: userProfiles.usersSimilar.length == 0
                                  ? () {}
                                  : () {
                                      userProfiles
                                          .setFavoritedZingerPage(indexUser);
                                    },
                              child: Container(
                                padding: EdgeInsets.all(25.sp),
                                height: 110.h,
                                width: 110.h,
                                decoration: new BoxDecoration(
                                    boxShadow: [
                                      new BoxShadow(
                                          offset: new Offset(0.0, 0.0),
                                          color: Colors.grey),
                                      new BoxShadow(
                                          offset: new Offset(1.0, 1.0),
                                          color: Colors.grey,
                                          blurRadius: 5.0),
                                      new BoxShadow(
                                          offset: new Offset(-1.0, -1.0),
                                          color: Colors.white,
                                          blurRadius: 10.0)
                                    ],
                                    color: Colors.white,
                                    borderRadius: BorderRadius.circular(60.sp)),
                                child: new ShaderMask(
                                    child: new Icon(
                                      indexUser <
                                                  userProfiles
                                                      .usersSimilar.length &&
                                              indexUser >= 0
                                          ? userProfiles.usersSimilar[indexUser]
                                                  .favorited
                                              ? Icons.favorite
                                              : Icons.favorite_border
                                          : Icons.favorite_border,
                                      size: 65.h,
                                    ),
                                    blendMode: BlendMode.srcATop,
                                    shaderCallback: (Rect bounds) {
                                      return LinearGradient(
                                              colors: [
                                                Colors.red.shade700,
                                                Colors.red.shade200
                                              ],
                                              begin: Alignment.topRight,
                                              end: Alignment.bottomLeft,
                                              stops: [0.0, 1.0])
                                          .createShader(bounds);
                                    }),
                              ),
                            ),
                            //Icon Seen Profile
                            MaterialButton(
                              shape: CircleBorder(),
                              padding: EdgeInsets.all(0.sp),
                              minWidth: 0.sp,
                              onPressed: userProfiles.usersSimilar.length == 0
                                  ? () {}
                                  : () {
                                      // if (userProfiles.usersSimilar.length < 8) {
                                      //   widget.searchByLocation
                                      //       ? userProfiles.indexUserLocation =
                                      //           userProfiles.indexUserLocation +
                                      //               indexUser
                                      //       : userProfiles.indexUserZinger =
                                      //           userProfiles.indexUserZinger +
                                      //               indexUser;
                                      // } else {
                                      //   widget.searchByLocation
                                      //       ? userProfiles.indexUserLocation =
                                      //           indexUser
                                      //       : userProfiles.indexUserZinger =
                                      //           indexUser;
                                      // }
                                      Navigator.of(context)
                                          .push(MaterialPageRoute(
                                              builder: (BuildContext context) =>
                                                  ProfileDetails(
                                                      authenticationUserId:
                                                          Provider.of<AuthenticationProvider>(
                                                                  context)
                                                              .getUserInfo
                                                              .id,
                                                      userId: userProfiles
                                                          .usersSimilar[
                                                              indexUser]
                                                          .id)))
                                          .then(
                                            (value) =>
                                                Navigator.pushReplacement(
                                              context,
                                              PageRouteBuilder(
                                                pageBuilder: (context,
                                                        animation1,
                                                        animation2) =>
                                                    HomePage(
                                                        searchByLocation: widget
                                                            .searchByLocation),
                                                transitionDuration:
                                                    Duration(seconds: 0),
                                              ),
                                            ),
                                          );
                                    },
                              child: Container(
                                padding: EdgeInsets.all(10.sp),
                                height: 80.h,
                                width: 80.h,
                                decoration: new BoxDecoration(
                                    boxShadow: [
                                      new BoxShadow(
                                          offset: new Offset(0.0, 0.0),
                                          color: Colors.grey),
                                      new BoxShadow(
                                          offset: new Offset(1.0, 1.0),
                                          color: Colors.grey,
                                          blurRadius: 5.0),
                                      new BoxShadow(
                                          offset: new Offset(-1.0, -1.0),
                                          color: Colors.white,
                                          blurRadius: 10.0)
                                    ],
                                    color: Colors.white,
                                    borderRadius: BorderRadius.circular(60.sp)),
                                child: new ShaderMask(
                                    child: new Image(
                                        image: new AssetImage(
                                            'assets/images/blank-avatar.png')),
                                    blendMode: BlendMode.srcATop,
                                    shaderCallback: (Rect bounds) {
                                      return LinearGradient(
                                              colors: [
                                                Colors.purple.shade500,
                                                Colors.purple.shade200
                                              ],
                                              begin: Alignment.topRight,
                                              end: Alignment.bottomLeft,
                                              stops: [0.0, 1.0])
                                          .createShader(bounds);
                                    }),
                              ),
                            )
                          ],
                        ),
                      ),
                    ),
                    Container(
                      margin: userProfiles.usersSimilar.length == 0
                          ? EdgeInsets.only(top: 320.h)
                          : EdgeInsets.only(top: 130.h),
                      height: 1600.h,
                      child: userProfiles.usersSimilar.length == 0
                          ? Align(
                              alignment: Alignment.bottomCenter,
                              child: widget.searchByLocation
                                  ? Column(
                                      mainAxisAlignment:
                                          MainAxisAlignment.start,
                                      children: [
                                        Container(
                                          width: 800.w,
                                          child: Card(
                                            child: Container(
                                              padding: EdgeInsets.all(32.sp),
                                              child: Column(
                                                mainAxisAlignment:
                                                    MainAxisAlignment.end,
                                                crossAxisAlignment:
                                                    CrossAxisAlignment.end,
                                                children: [
                                                  Column(
                                                    mainAxisAlignment:
                                                        MainAxisAlignment.start,
                                                    crossAxisAlignment:
                                                        CrossAxisAlignment
                                                            .start,
                                                    children: [
                                                      Text(
                                                        "Gợi ý",
                                                        style: TextStyle(
                                                          fontSize: 60.sp,
                                                          fontWeight:
                                                              FontWeight.bold,
                                                        ),
                                                      ),
                                                      SizedBox(height: 15.h),
                                                      Text(
                                                        "Cài đặt bộ lọc để có thể tìm thấy người dùng mới.",
                                                        textAlign:
                                                            TextAlign.justify,
                                                      ),
                                                    ],
                                                  ),
                                                  ButtonCaiDatLocation(),
                                                ],
                                              ),
                                            ),
                                          ),
                                        ),
                                        SizedBox(height: 100.h),
                                        SearchMatching(
                                          atCenter: atCenter,
                                          chng: chng,
                                          title: "Đã hết người gần đây",
                                        ),
                                      ],
                                    )
                                  : Container(
                                      child: Column(
                                        mainAxisAlignment:
                                            MainAxisAlignment.start,
                                        children: [
                                          SizedBox(height: 280.h),
                                          SearchMatching(
                                            atCenter: atCenter,
                                            chng: chng,
                                            title: "Đã hết người tương đồng",
                                          ),
                                        ],
                                      ),
                                    ),
                            )
                          : Align(
                              alignment: Alignment.bottomCenter,
                              child: new TinderSwapCard(
                                animDuration: 800,
                                orientation: AmassOrientation.TOP,
                                totalNum: userProfiles.usersSimilar.length,
                                stackNum: 3,
                                swipeEdge: 10.0,
                                maxWidth:
                                    MediaQuery.of(context).size.width - 10.0,
                                maxHeight:
                                    MediaQuery.of(context).size.height * 0.7,
                                minWidth:
                                    MediaQuery.of(context).size.width - 50.0,
                                minHeight:
                                    MediaQuery.of(context).size.height * 0.64,
                                cardBuilder: (context, index) {
                                  Widget child;
                                  if (userProfiles.usersSimilar.length != 0) {
                                    if (index >= 0 &&
                                        index <=
                                            userProfiles.usersSimilar.length -
                                                1 &&
                                        getUsersSimilar) {
                                      final date = userProfiles
                                          .usersSimilar[index].dob
                                          .toString()
                                          .split(("T"))[0]
                                          .toString();
                                      child = UserSimilarCard(
                                          widget.searchByLocation,
                                          userProfiles
                                              .usersSimilar[index].avatarPath,
                                          userProfiles
                                              .usersSimilar[index].fullName,
                                          calculateAge(DateFormat("yyyy-MM-dd")
                                                  .parse(date))
                                              .toString(),
                                          widget.searchByLocation
                                              ? userProfiles
                                                  .usersSimilar[index].distance
                                                  .toStringAsFixed(1)
                                              : userProfiles
                                                  .usersSimilar[index].point
                                                  .toStringAsFixed(2),
                                          userProfiles.usersSimilar[index]
                                              .numberOfFollowers
                                              .toString(),
                                          userProfiles.usersSimilar[index]
                                              .numberOfFavoritors
                                              .toString());
                                    }
                                  }
                                  return child;
                                },
                                cardController: _cardController,
                                swipeUpdateCallback: (DragUpdateDetails details,
                                    Alignment align) {
                                  /// Get swiping card's alignment
                                  if (align.x < 0) {
                                    //Card is LEFT swiping
                                    //print("Left align " + align.x.toString());
                                    setState(() {
                                      if (align.x < -1) atCenter = false;
                                      chng = true;
                                    });
                                  } else if (align.x > 0) {
                                    //Card is RIGHT swiping
                                    //print("right align " + align.x.toString());
                                    setState(() {
                                      if (align.x > 1) atCenter = false;
                                      chng = false;
                                    });
                                  }
                                },
                                swipeCompleteCallback:
                                    (CardSwipeOrientation orientation,
                                        int index) async {
                                  /// Get orientation & index of swiped card!
                                  setState(() {
                                    atCenter = true;
                                  });
                                  indexUser = index + 1;
                                  if (userProfiles.usersSimilar.length < 8) {
                                    widget.searchByLocation
                                        ? userProfiles.indexUserLocation =
                                            userProfiles.indexUserLocation +
                                                indexUser
                                        : userProfiles.indexUserZinger =
                                            userProfiles.indexUserZinger +
                                                indexUser;
                                  } else {
                                    widget.searchByLocation
                                        ? userProfiles.indexUserLocation =
                                            indexUser
                                        : userProfiles.indexUserZinger =
                                            indexUser;
                                  }
                                  if (index ==
                                      userProfiles.usersSimilar.length - 1) {
                                    userProfiles
                                        .nextUserPage(widget.searchByLocation);
                                    await Navigator.pushReplacement(
                                      context,
                                      PageRouteBuilder(
                                        pageBuilder: (context, animation1,
                                                animation2) =>
                                            HomePage(
                                                searchByLocation:
                                                    widget.searchByLocation),
                                        transitionDuration:
                                            Duration(seconds: 0),
                                      ),
                                    );
                                  }
                                },
                              ),
                            ),
                    ),
                  ],
                );
              } else if (snapshot.hasError) {
                child = Stack(
                  children: [
                    AnimatedContainer(
                      duration: Duration(milliseconds: 600),
                      curve: Curves.fastLinearToSlowEaseIn,
                      color: !atCenter
                          ? chng
                              ? Colors.pinkAccent.shade200
                              : Colors.tealAccent.shade200
                          : Colors.blue.shade50,
                      child: new Center(
                          child: Column(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: <Widget>[
                          new SizedBox(
                            height: ScreenUtil().setHeight(550.0),
                          ),
                          new ClipRRect(
                            borderRadius: BorderRadius.circular(100.0),
                            child: new Image(
                                width: ScreenUtil().setWidth(400),
                                height: ScreenUtil().setWidth(400),
                                fit: BoxFit.cover,
                                image: new AssetImage(
                                    'assets/images/abhishekProfile.JPG')),
                          ),
                          new SizedBox(
                            height: ScreenUtil().setHeight(40.0),
                          ),
                          new Padding(
                            padding: EdgeInsets.symmetric(
                                horizontal: ScreenUtil().setWidth(60.0)),
                            child: new Text(
                                "There is no one new around you ...",
                                textAlign: TextAlign.center,
                                style: new TextStyle(
                                    wordSpacing: 1.2,
                                    fontSize: ScreenUtil().setSp(55.0),
                                    fontWeight: FontWeight.w300,
                                    color: Colors.grey.shade600)),
                          )
                        ],
                      )),
                    ),
                  ],
                );
              } else if (!getUsersSimilar) {
                child = SearchMatching(
                  atCenter: atCenter,
                  chng: chng,
                  title: widget.searchByLocation
                      ? "Tìm kiếm lân cận..."
                      : "Tìm kiếm tương đồng...",
                );
              }
              return child != null
                  ? child
                  : SearchMatching(
                      atCenter: atCenter,
                      chng: chng,
                      title: widget.searchByLocation
                          ? "Tìm kiếm lân cận..."
                          : "Tìm kiếm tương đồng...",
                    );
            },
          );
        },
      ),
    );
  }

  List<String> welcomeImages = [
    "assets/welcome0.png",
    "assets/welcome1.png",
    "assets/welcome2.png",
    "assets/welcome2.png",
    "assets/welcome1.png",
    "assets/welcome1.png"
  ];
  double abs(double x) {
    if (x < 0) return x * -1;
    return x;
  }
}

class ButtonCaiDatLocation extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Container(
      width: 300.w,
      height: 110.w,
      color: Theme.of(context).secondaryHeaderColor,
      child: MaterialButton(
        padding: EdgeInsets.only(left: 20.sp),
        onPressed: () {
          Navigator.push(
            context,
            PageTransition(
              type: PageTransitionType.rightToLeft,
              child: SettingLocationScreen(),
            ),
          ).then((value) => Navigator.pushReplacement(
                context,
                PageRouteBuilder(
                  pageBuilder: (context, animation1, animation2) =>
                      HomePage(searchByLocation: true),
                  transitionDuration: Duration(seconds: 0),
                ),
              ));
        },
        child: Row(
          mainAxisAlignment: MainAxisAlignment.center,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            Text(
              "CÀI ĐẶT",
              style: TextStyle(
                fontSize: 45.sp,
                color: Colors.white,
                fontWeight: FontWeight.bold,
              ),
            ),
            Icon(
              Icons.arrow_right,
              size: 70.sp,
              color: Colors.white,
            ),
          ],
        ),
      ),
    );
  }
}

class ButtonStyleFindFriends extends StatelessWidget {
  final String title;
  final Function onPressed;
  final Color color;
  ButtonStyleFindFriends({this.title, this.onPressed, this.color});
  @override
  Widget build(BuildContext context) {
    return Container(
      child: MaterialButton(
        elevation: 2.0,
        child: Text(
          title,
          style: Theme.of(context)
              .textTheme
              .bodyText2
              .copyWith(color: Colors.grey),
        ),
        height: 110.h,
        minWidth: 475.w,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(30.sp),
          side: BorderSide(color: color),
        ),
        onPressed: onPressed,
        color: Colors.white,
      ),
    );
  }
}

class SearchMatching extends StatelessWidget {
  const SearchMatching(
      {Key key,
      @required this.atCenter,
      @required this.chng,
      @required this.title})
      : super(key: key);

  final bool atCenter;
  final bool chng;
  final String title;

  @override
  Widget build(BuildContext context) {
    return Stack(children: [
      AnimatedContainer(
        duration: Duration(milliseconds: 600),
        curve: Curves.fastLinearToSlowEaseIn,
        color: !atCenter
            ? chng
                ? Colors.pinkAccent.shade200
                : Colors.tealAccent.shade200
            : Colors.blue.shade50,
        child: new Center(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: <Widget>[
              new CircularProgressIndicator(),
              new SizedBox(
                height: ScreenUtil().setHeight(30.0),
              ),
              new Text(
                title,
                style: new TextStyle(
                    fontSize: ScreenUtil().setSp(60.0),
                    fontWeight: FontWeight.w200,
                    color: Colors.grey.shade600),
              )
            ],
          ),
        ),
      )
    ]);
  }
}

class UserSimilarCard extends StatelessWidget {
  final bool isAround;
  final String imagePath,
      userName,
      userOld,
      userPrecent,
      numberFollow,
      numberFavorite;
  UserSimilarCard(this.isAround, this.imagePath, this.userName, this.userOld,
      this.userPrecent, this.numberFollow, this.numberFavorite);
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 16.sp),
      child: ClipRRect(
        borderRadius: BorderRadius.all(Radius.circular(30.sp)),
        child: Stack(
          children: <Widget>[
            Container(
              width: 1000.w,
              height: 1300.h,
              child: Card(
                child: Image.network(
                  imagePath,
                  fit: BoxFit.cover,
                ),
              ),
            ),
            Positioned(
              left: 0.h,
              bottom: 0.h,
              width: 1000.w,
              height: 150.h,
              child: Container(
                decoration: BoxDecoration(
                    gradient: LinearGradient(
                        begin: Alignment.bottomCenter,
                        end: Alignment.topCenter,
                        colors: [Colors.black, Colors.black12])),
              ),
            ),
            Positioned(
              left: 30.w,
              bottom: 30.h,
              child: Container(
                width: MediaQuery.of(context).size.width - 50,
                child: Row(
                  mainAxisSize: MainAxisSize.max,
                  mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                  children: <Widget>[
                    Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: <Widget>[
                        Container(
                          width: 400.w,
                          child: Text(
                            userName,
                            overflow: TextOverflow.ellipsis,
                            maxLines: 1,
                            style: TextStyle(
                                color: Colors.white,
                                fontSize: 40.sp,
                                fontWeight: FontWeight.w700,
                                letterSpacing: 1),
                          ),
                        ),
                        Text(
                          "$userOld tuổi",
                          style: TextStyle(
                              color: Colors.white,
                              fontSize: 30.sp,
                              fontWeight: FontWeight.normal),
                        ),
                      ],
                    ),
                    NumberFollowAndFavoriteUser(
                      number: numberFollow,
                      icon: Icons.star,
                      colors: [Colors.blue.shade600, Colors.blue.shade300],
                    ),
                    NumberFollowAndFavoriteUser(
                      number: numberFavorite,
                      icon: Icons.favorite,
                      colors: [Colors.red.shade700, Colors.red.shade200],
                    ),
                    Container(
                        width: 140.w,
                        height: 40.h,
                        padding: EdgeInsets.symmetric(
                            horizontal: 10.w, vertical: 5.h),
                        decoration: BoxDecoration(
                            color: Colors.white,
                            shape: BoxShape.rectangle,
                            borderRadius:
                                BorderRadius.all(Radius.circular(10))),
                        child: Text(
                          isAround ? "$userPrecent km" : "$userPrecent%",
                          overflow: TextOverflow.ellipsis,
                          maxLines: 1,
                          style:
                              TextStyle(color: Colors.black, fontSize: 30.sp),
                        ))
                  ],
                ),
              ),
            )
          ],
        ),
      ),
    );
  }
}

class NumberFollowAndFavoriteUser extends StatelessWidget {
  final String number;
  final IconData icon;
  final List<Color> colors;

  NumberFollowAndFavoriteUser({this.number, this.icon, this.colors});

  @override
  Widget build(BuildContext context) {
    return Container(
      width: 150.w,
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: [
          Container(
            child: new ShaderMask(
                child: new Icon(
                  icon,
                  size: 60.sp,
                ),
                blendMode: BlendMode.srcATop,
                shaderCallback: (Rect bounds) {
                  return LinearGradient(
                      colors: colors,
                      begin: Alignment.topRight,
                      end: Alignment.bottomLeft,
                      stops: [0.0, 1.0]).createShader(bounds);
                }),
          ),
          Container(
            child: Text(
              number,
              overflow: TextOverflow.ellipsis,
              maxLines: 1,
              style: TextStyle(
                color: Colors.white,
                fontSize: 30.sp,
              ),
            ),
          ),
          SizedBox(width: 10.w)
        ],
      ),
    );
  }
}
