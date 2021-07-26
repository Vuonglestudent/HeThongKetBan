import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';
import 'package:provider/provider.dart';
import 'package:rxdart/rxdart.dart';
import 'package:tinder_clone/HomePage.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Models/Providers/MessageProvider.dart';
import 'package:tinder_clone/Models/Providers/NotificationsProvider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Screens/LoginGmail_Screen.dart';
import 'package:tinder_clone/Screens/Login_Screen.dart';
import 'package:tinder_clone/Screens/SplashScreen.dart';

final FlutterLocalNotificationsPlugin flutterLocalNotificationsPlugin =
    FlutterLocalNotificationsPlugin();

final BehaviorSubject<ReceivedNotification> didReceiveLocalNotificationSubject =
    BehaviorSubject<ReceivedNotification>();

final BehaviorSubject<String> selectNotificationSubject =
    BehaviorSubject<String>();

const MethodChannel platform =
    MethodChannel('dexterx.dev/flutter_local_notifications_example');

class ReceivedNotification {
  ReceivedNotification({
    @required this.id,
    @required this.title,
    @required this.body,
    @required this.payload,
  });

  final int id;
  final String title;
  final String body;
  final String payload;
}

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  final NotificationAppLaunchDetails notificationAppLaunchDetails =
      await flutterLocalNotificationsPlugin.getNotificationAppLaunchDetails();

  String initialRoute = SplashScreen.id;
  if (notificationAppLaunchDetails?.didNotificationLaunchApp ?? false) {
    selectedNotificationPayload = notificationAppLaunchDetails.payload;
    initialRoute = HomePage.id;
  }

  const AndroidInitializationSettings initializationSettingsAndroid =
      AndroidInitializationSettings('@mipmap/zinger_logo');

  final InitializationSettings initializationSettings = InitializationSettings(
    android: initializationSettingsAndroid,
  );
  await flutterLocalNotificationsPlugin.initialize(initializationSettings,
      onSelectNotification: (String payload) async {
    if (payload != null) {
      debugPrint('notification payload: $payload');
    }
    selectedNotificationPayload = payload;
    selectNotificationSubject.add(payload);
  });
  runApp(MyApp(
    initialRoute: initialRoute,
  ));
}

class MyApp extends StatelessWidget {
  final String initialRoute;
  MyApp({this.initialRoute});
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (context) => Loading()),
        ChangeNotifierProvider(create: (context) => AuthenticationProvider()),
        ChangeNotifierProvider(create: (context) => UserProvider()),
        ChangeNotifierProvider(create: (context) => MessageProvider()),
        ChangeNotifierProvider(create: (context) => NotificationsProvider()),
      ],
      child: MaterialApp(
        debugShowCheckedModeBanner: false,
        title: 'Zinger - Kết bạn online',
        theme: ThemeData(
            primaryColor: Color.fromRGBO(174, 61, 158, 1.0),
            secondaryHeaderColor: Color.fromRGBO(219, 138, 172, 1.0),
            accentColor: Color.fromRGBO(251, 195, 182, 1.0)),
        initialRoute: SplashScreen.id,
        routes: {
          HomePage.id: (context) => HomePage(
                searchByLocation: false,
              ),
          SplashScreen.id: (context) => SplashScreen(),
          LoginGmailScreen.id: (context) => LoginGmailScreen(),
          LoginScreen.id: (context) => LoginScreen(),
        },
      ),
    );
  }
}
