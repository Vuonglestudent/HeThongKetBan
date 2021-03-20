import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/HomePage.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Screens/LoginGmail_Screen.dart';
import 'package:tinder_clone/Screens/Login_Screen.dart';
import 'package:tinder_clone/Screens/SplashScreen.dart';
import 'package:tinder_clone/Screens/ListImage_Screen.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (context) => Loading()),
        ChangeNotifierProvider(create: (context) => AuthenticationProvider()),
        ChangeNotifierProvider(create: (context) => UserProvider()),
      ],
      child: MaterialApp(
        debugShowCheckedModeBanner: false,
        title: 'Tinder Clone',
        theme: ThemeData(
            primaryColor: Color.fromRGBO(174, 61, 158, 1.0),
            secondaryHeaderColor: Color.fromRGBO(219, 138, 172, 1.0),
            accentColor: Color.fromRGBO(251, 195, 182, 1.0)),
        initialRoute: SplashScreen.id,
        routes: {
          HomePage.id: (context) => HomePage(),
          SplashScreen.id: (context) => SplashScreen(),
          LoginGmailScreen.id: (context) => LoginGmailScreen(),
          LoginScreen.id: (context) => LoginScreen(),
        },
      ),
    );
  }
}
