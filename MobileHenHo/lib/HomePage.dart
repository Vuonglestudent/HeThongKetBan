import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/MessageProvider.dart';
import 'package:tinder_clone/Models/Providers/NotificationsProvider.dart';
import 'package:tinder_clone/Models/User.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:tinder_clone/Screens/AccpetCallerScreen.dart';
import 'package:tinder_clone/Screens/Friends_Screen.dart';
import 'package:tinder_clone/Screens/InPersonChat_Screen.dart';
import 'package:tinder_clone/Screens/LoginGmail_Screen.dart';
import 'package:tinder_clone/Screens/Messages_Screen.dart';
import 'package:tinder_clone/Screens/Notifications_Screen.dart';
import 'package:tinder_clone/Screens/Profile_Screen.dart';
import 'package:tinder_clone/Screens/Zinger_Screen.dart';
import 'package:tinder_clone/main.dart';

class HomePage extends StatefulWidget {
  static const String id = 'HomePage';
  final int tabIndex;
  final bool searchByLocation;
  HomePage({this.searchByLocation = false, this.tabIndex});
  @override
  _HomePageState createState() => _HomePageState();
}

class _HomePageState extends State<HomePage>
    with SingleTickerProviderStateMixin {
  TabController _tabcontroller;
  MessageProvider messages;
  AuthenticationProvider authentication;
  @override
  void initState() {
    super.initState();
    _configureDidReceiveLocalNotificationSubject();
    _configureSelectNotificationSubject();
    _tabcontroller = new TabController(length: 5, vsync: this);
    _tabcontroller.index = widget.tabIndex ?? 0;
    _tabcontroller.addListener(() {
      setState(() {});
    });
  }

  void _configureDidReceiveLocalNotificationSubject() {
    print('_configureDidReceiveLocalNotificationSubject');
    didReceiveLocalNotificationSubject.stream
        .listen((ReceivedNotification receivedNotification) async {
      await showDialog(
        context: context,
        builder: (BuildContext context) => CupertinoAlertDialog(
          title: receivedNotification.title != null
              ? Text(receivedNotification.title)
              : null,
          content: receivedNotification.body != null
              ? Text(receivedNotification.body)
              : null,
          actions: <Widget>[
            CupertinoDialogAction(
              isDefaultAction: true,
              onPressed: () async {
                Navigator.of(context, rootNavigator: true).pop();
                await Navigator.pushNamed(
                  context,
                  HomePage.id,
                );
              },
              child: const Text('Ok'),
            )
          ],
        ),
      );
    });
  }

  void _configureSelectNotificationSubject() {
    print('_configureSelectNotificationSubject');
    selectNotificationSubject.stream.listen((String payload) async {
      var str = payload.split(RegExp("_"));
      if (str[0] == 'chat') {
        UsersSimilar user =
            new UsersSimilar(id: str[1], fullName: str[2], avatarPath: str[3]);
        await Navigator.push(
          context,
          MaterialPageRoute(
            builder: (BuildContext context) => InPersonChatScreen(
              usersSimilar: user,
            ),
          ),
        ).then((value) {
          selectNotificationSubject.add('');
          messages.saveUserWhenExistChat();
        });
      }
    });
  }

  @override
  void dispose() {
    //didReceiveLocalNotificationSubject.close();
    //selectNotificationSubject.close();
    // authentication.signalR.callerSub.close();
    // authentication.signalR.messageSub.close();
    // authentication.signalR.notificationSub.close();
    _tabcontroller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    authentication = Provider.of<AuthenticationProvider>(context);
    messages = Provider.of<MessageProvider>(context);
    var notification = Provider.of<NotificationsProvider>(context);

    if (authentication.getUserInfo == null) {
      Future.delayed(Duration(seconds: 1), () {
        Navigator.pushReplacementNamed(context, LoginGmailScreen.id);
      });
      return Scaffold(
        body: SearchMatching(
          atCenter: true,
          chng: true,
          title: "Logout System ...",
        ),
      );
    } else {
      if (authentication.signalR.connectionId == null) {
        authentication.signalR.startConnectHub(context);
        authentication.determinePosition();
      }

      authentication.signalR.messageSub.listen((value) {
        if (value.idMessage != authentication.signalR.idMessageCurrent ||
            value.senderId != authentication.signalR.senderId ||
            value.receiverId != authentication.signalR.recevierId) {
          messages.listenMessageSignalRInChat(
              authentication.getUserInfo.id, value);
          authentication.signalR.idMessageCurrent = value.idMessage;
          authentication.signalR.senderId = value.senderId;
          authentication.signalR.recevierId = value.receiverId;
          if (messages.userChatCurrent != null) {
            if (value.senderId != messages.userChatCurrent.userInfo.id &&
                value.senderId != authentication.getUserInfo.id) {
              NotificationsProvider().showNotification(
                  value.fullName, 'đã nhắn tin: ${value.content}', '');
              return;
            }
          }
        }
      });

      authentication.signalR.notificationSub.listen((value) {
        if (value != null) {
          notification.listenNotificationHub(
              value, _tabcontroller.index != 3 ? true : false);
          authentication.signalR.notificationSub.add(null);
        }
      });

      authentication.signalR.callerSub.distinct((userBefore, userAfter) {
        if (userBefore == null)
          return true;
        else
          return false;
      }).listen((user) {
        setState(() {
          if (user != null && !authentication.isCalling) {
            print('caller: ' + user.userName);
            authentication.signalR.callerSub.add(null);
            Navigator.push(
              context,
              MaterialPageRoute(
                builder: (BuildContext context) => AccpectCallerScreen(
                  caller: user,
                ),
              ),
            ).then((value) {
              authentication.isCalling = false;
            });
          }
        });
      });

      return WillPopScope(
        onWillPop: () async {
          final value = await showDialog<bool>(
              context: context,
              builder: (context) {
                return AlertDialog(
                  content: Text('Bạn có muốn đăng xuất khỏi ứng dụng?'),
                  actions: <Widget>[
                    FlatButton(
                      child: Text('Không'),
                      onPressed: () {
                        Navigator.of(context).pop(false);
                      },
                    ),
                    FlatButton(
                      child: Text('Đồng ý'),
                      onPressed: () {
                        authentication.logoutSystem(context);
                        Navigator.of(context).pop(true);
                      },
                    ),
                  ],
                );
              });

          return value == true;
        },
        child: Scaffold(
            appBar: AppBar(
              backgroundColor: Colors.white,
              automaticallyImplyLeading: false,
              elevation: 1,
              title: Padding(
                padding: EdgeInsets.only(bottom: 40.h),
                child: TabBar(
                    isScrollable: true,
                    indicatorColor: Colors.transparent,
                    controller: _tabcontroller,
                    tabs: [
                      SafeArea(
                        child: Container(
                          child: Center(
                            child: Image(
                              image: AssetImage('assets/images/Z-logo.png'),
                              color: _tabcontroller.index == 0
                                  ? Theme.of(context).primaryColor
                                  : Colors.grey,
                              width: 140.w,
                            ),
                          ),
                        ),
                      ),
                      IconRouterButton(
                        tabcontroller: _tabcontroller,
                        iconData: Icons.people,
                        index: 1,
                      ),
                      IconRouterButton(
                        tabcontroller: _tabcontroller,
                        iconData: Icons.message,
                        index: 2,
                      ),
                      IconRouterButton(
                        tabcontroller: _tabcontroller,
                        iconData: Icons.circle_notifications,
                        index: 3,
                      ),
                      IconRouterButton(
                        tabcontroller: _tabcontroller,
                        iconData: Icons.account_circle,
                        index: 4,
                      ),
                    ]),
              ),
            ),
            body: TabBarView(
              physics: NeverScrollableScrollPhysics(),
              controller: _tabcontroller,
              children: <Widget>[
                ZingerScreen(
                  searchByLocation: widget.searchByLocation,
                ),
                FriendsScreen(),
                MessagesScreen(),
                NotificationsScreen(),
                ProfileScreen(),
              ],
            )),
      );
    }
  }
}

class IconRouterButton extends StatelessWidget {
  final TabController tabcontroller;
  final IconData iconData;
  final int index;
  IconRouterButton({
    @required this.tabcontroller,
    @required this.iconData,
    @required this.index,
  });
  @override
  Widget build(BuildContext context) {
    return SafeArea(
      child: Container(
        padding: EdgeInsets.all(20.0),
        child: Center(
          child: Icon(
            iconData,
            color: tabcontroller.index == index
                ? Theme.of(context).primaryColor
                : Colors.grey,
            size: 120.w,
          ),
        ),
      ),
    );
  }
}
