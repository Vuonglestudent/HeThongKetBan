import 'package:flutter/material.dart';
import 'package:tinder_clone/Models/Zinger_clone_icons.dart';
import 'package:tinder_clone/Screens/Messages_Screen.dart';
import 'package:tinder_clone/Screens/Profile_Screen.dart';
import 'package:tinder_clone/Screens/Zinger_Screen.dart';

class HomePage extends StatefulWidget {
  static const String id = 'HomePage';
  @override
  _HomePageState createState() => _HomePageState();
}

class _HomePageState extends State<HomePage>
    with SingleTickerProviderStateMixin {
  TabController _tabcontroller;
  @override
  void initState() {
    super.initState();
    _tabcontroller = new TabController(length: 3, vsync: this);
    _tabcontroller.addListener(() {
      setState(() {});
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          backgroundColor: Colors.white,
          automaticallyImplyLeading: false,
          elevation: 1,
          title: Padding(
            padding: EdgeInsets.only(bottom: 15.0),
            child: TabBar(
                indicatorColor: Colors.transparent,
                controller: _tabcontroller,
                tabs: [
                  IconRouterButton(
                    tabcontroller: _tabcontroller,
                    iconData: Zinger_clone.iconfinder_icons_user2_1564535,
                    index: 0,
                  ),
                  SafeArea(
                    child: Container(
                      padding: EdgeInsets.all(20.0),
                      child: Center(
                        child: Image(
                          image: AssetImage('assets/images/Z-logo.png'),
                          color: _tabcontroller.index == 1
                              ? Theme.of(context).primaryColor
                              : Colors.grey,
                          width: 45.0,
                        ),
                      ),
                    ),
                  ),
                  IconRouterButton(
                    tabcontroller: _tabcontroller,
                    iconData: Zinger_clone.iconfinder_message_01_186393,
                    index: 2,
                  ),
                ]),
          ),
        ),
        body: TabBarView(
          physics: NeverScrollableScrollPhysics(),
          controller: _tabcontroller,
          children: <Widget>[
            ProfileScreen(),
            ZingerScreen(),
            MessagesScreen(),
          ],
        ));
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
            size: 35.0,
          ),
        ),
      ),
    );
  }
}

