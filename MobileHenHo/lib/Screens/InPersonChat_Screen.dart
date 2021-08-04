import 'package:bubble/bubble.dart';
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:multi_image_picker/multi_image_picker.dart';
import 'package:provider/provider.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:tinder_clone/Models/ChatFriend.dart';
import 'package:tinder_clone/Models/Image.dart' as img;
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/MessageProvider.dart';
import 'package:tinder_clone/Models/User.dart';
import 'package:tinder_clone/Screens/CallVideo_Screen.dart';
import 'package:tinder_clone/Screens/Zinger_Screen.dart';
import 'package:tinder_clone/Services/SignalRService.dart';
import 'package:tinder_clone/Widgets/ErrorData.dart';
import 'package:tinder_clone/Widgets/FullScreenImage.dart';
import '../Models/Providers/NotificationsProvider.dart';
import '../Models/Providers/UserProvider.dart';
import 'ProfileDetail_Screen.dart';

class InPersonChatScreen extends StatefulWidget {
  final GlobalKey _menuKey = new GlobalKey();
  final ChatFriend user;
  final UsersSimilar usersSimilar;
  InPersonChatScreen({this.user, this.usersSimilar});
  @override
  _InPersonChatScreenState createState() => _InPersonChatScreenState();
}

enum WhyFarther { harder, smarter, selfStarter, tradingCharter }

class _InPersonChatScreenState extends State<InPersonChatScreen> {
  TextEditingController _messageController;
  AuthenticationProvider authentication;
  NotificationsProvider notification;
  UserProvider userProfile;
  MessageProvider messages;

  bool loadData = false;
  String contentChat = '';
  bool isImage = false;

  Widget _buildGridViewChoiceImage(List<Asset> images) {
    if (images != null)
      return Container(
        height: 200.h,
        child: Center(
          child: ListView(
            scrollDirection: Axis.horizontal,
            children: List.generate(images.length, (index) {
              Asset asset = images[index];
              return MaterialButton(
                padding: EdgeInsets.zero,
                onPressed: () {
                  userProfile.removeImage(index);
                  if (userProfile.getImages.length == 0) {
                    isImage = false;
                  }
                },
                child: ClipRRect(
                  borderRadius: BorderRadius.circular(8.0),
                  child: AssetThumb(
                    asset: asset,
                    width: 200,
                    height: 200,
                  ),
                ),
              );
            }),
          ),
        ),
      );
    else
      return Container(color: Colors.white);
  }

  Widget _buildGridViewShowImage(List<String> images) {
    List<img.Image> imagePaths = img.Image().setPaths(images);
    if (images != null)
      return Container(
        height: images.length < 3
            ? (3 - images.length) * 300.h
            : images.length % 3 == 0
                ? (images.length ~/ 3) * 210.h
                : ((images.length ~/ 3) + 1) * 210.h,
        child: GridView.count(
          crossAxisCount: images.length < 3 ? images.length : 3,
          crossAxisSpacing: 2.sp,
          mainAxisSpacing: 2.sp,
          padding: EdgeInsets.all(2.sp),
          physics: const NeverScrollableScrollPhysics(),
          children: List.generate(images.length, (index) {
            return GestureDetector(
              onTap: () {
                Navigator.push(context, MaterialPageRoute(builder: (_) {
                  return FullScreenImage(
                    imageUrl: images[index],
                    tag: index,
                    galleryItems: imagePaths,
                  );
                }));
              },
              child: Hero(
                tag: images[index],
                child: Container(
                  child: Image(
                    fit: BoxFit.cover,
                    image: NetworkImage(
                      images[index],
                    ),
                  ),
                ),
              ),
            );
          }),
        ),
      );
    else
      return Container(color: Colors.white);
  }

  Future<bool> getDataMessages(
      MessageProvider messages, ChatFriend user) async {
    if (loadData == false) {
      loadData = await messages.getDataMessagesUser(
          user != null ? user.userInfo.id : widget.usersSimilar.id, 1);
    }
    return loadData;
  }

  bool checkData() {
    if (messages.friendList.isNotEmpty && messages.friendList.length > 0) {
      return true;
    } else
      return false;
  }

  void loadDataMessage() async {
    await messages.getFriendList();
  }

  @override
  void initState() {
    super.initState();
    _messageController = new TextEditingController();
  }

  @override
  Widget build(BuildContext context) {
    authentication = Provider.of<AuthenticationProvider>(context);
    notification = Provider.of<NotificationsProvider>(context);
    userProfile = Provider.of<UserProvider>(context);
    messages = Provider.of<MessageProvider>(context);

    void _showPopupMenu() async {
      await showMenu(
        context: context,
        position: RelativeRect.fromLTRB(1, 55, 0, 0),
        items: [
          PopupMenuItem(
            child: TextButton(
              onPressed: () {
                print('click view Info');
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) => ProfileDetails(
                      authenticationUserId: authentication.getUserInfo.id,
                      userId: widget.user != null
                          ? widget.user.userInfo.id
                          : widget.usersSimilar.id,
                    ),
                  ),
                );
              },
              child: Row(
                children: [
                  Icon(
                    Icons.person,
                    color: Colors.grey.shade600,
                  ),
                  SizedBox(
                    width: 20.w,
                  ),
                  Text("Trang cá nhân",
                      style: TextStyle(
                        color: Colors.grey.shade600,
                      )),
                ],
              ),
            ),
          ),
          PopupMenuItem(
            child: TextButton(
              onPressed: () async {
                await Alert(
                  context: context,
                  type: AlertType.none,
                  title: 'Chặn người dùng',
                  style: AlertStyle(
                    titleStyle: TextStyle(
                      fontWeight: FontWeight.w700,
                      fontSize: 70.sp,
                    ),
                    descStyle: TextStyle(fontSize: 55.sp),
                  ),
                  desc:
                      'Bạn và người dùng sẽ không thể thấy thông tin của nhau. Bạn có muốn chặn!',
                  buttons: [
                    DialogButton(
                      child: Text(
                        "Hủy",
                        style: TextStyle(color: Colors.white, fontSize: 60.sp),
                      ),
                      onPressed: () {
                        Navigator.pop(context, false);
                      },
                      radius: BorderRadius.circular(0.0),
                      color: Theme.of(context).accentColor,
                    ),
                    DialogButton(
                      child: Text(
                        "Đồng ý",
                        style: TextStyle(color: Colors.white, fontSize: 60.sp),
                      ),
                      onPressed: () {
                        Navigator.pop(context, true);
                      },
                      radius: BorderRadius.circular(0.0),
                      color: Theme.of(context).secondaryHeaderColor,
                    ),
                  ],
                ).show().then((value) async {
                  if (value) {
                    await userProfile
                        .blockUser(userProfile.getUserProfile.id)
                        .then((r) {
                      print(r);
                      if (r) {
                        notification.showNotification(
                          userProfile.getUserProfile.fullName,
                          "đã bị chặn thành công",
                          userProfile.getUserProfile.id,
                        );
                        Navigator.pop(context);
                      } else {
                        notification.showNotification(
                          userProfile.getUserProfile.fullName,
                          "không thể bị chặn lúc này",
                          userProfile.getUserProfile.id,
                        );
                      }
                    });
                  }
                });
              },
              child: Row(
                children: [
                  Icon(
                    Icons.highlight_remove,
                    color: Colors.grey.shade600,
                  ),
                  SizedBox(
                    width: 20.w,
                  ),
                  Text(
                    "Chặn người dùng",
                    style: TextStyle(
                      color: Colors.grey.shade600,
                    ),
                  ),
                ],
              ),
            ),
          ),
        ],
        elevation: 8.0,
      );
    }

    if (authentication.signalR.connectionId == null) {
      authentication.signalR.startConnectHub(context);
    }
    return Scaffold(
      backgroundColor: Colors.white,
      appBar: AppBar(
        automaticallyImplyLeading: false,
        backgroundColor: Colors.white,
        titleSpacing: 0.0,
        actions: <Widget>[
          ShaderMask(
              child: Row(
                children: [
                  IconButton(
                      icon: Icon(Icons.phone),
                      onPressed: () {
                        print('click phone');
                        authentication.signalR
                            .getUserById(widget.user != null
                                ? widget.user.userInfo.id
                                : widget.usersSimilar.id)
                            .then((value) {
                          print(value);
                          if (value != null) {
                            Navigator.push(
                              context,
                              MaterialPageRoute(
                                builder: (BuildContext context) =>
                                    CallVideoScreen(
                                  myFriend: widget.user.userInfo,
                                  callType: CallType.voiceCall,
                                  isAccept: false,
                                  context: context,
                                ),
                              ),
                            );
                          } else {
                            print("Người dùng chưa onl");
                          }
                        });
                      }),
                  IconButton(
                      icon: Icon(Icons.video_call),
                      onPressed: () {
                        print('click video call');
                        authentication.signalR
                            .getUserById(widget.user != null
                                ? widget.user.userInfo.id
                                : widget.usersSimilar.id)
                            .then((value) {
                          print(value);
                          if (value != null) {
                            Navigator.push(
                              context,
                              MaterialPageRoute(
                                  builder: (BuildContext context) =>
                                      CallVideoScreen(
                                        myFriend: widget.user.userInfo,
                                        callType: CallType.videoCall,
                                        isAccept: false,
                                        context: context,
                                      )),
                            );
                          } else {
                            print("Người dùng chưa onl");
                          }
                        });
                      }),
                  IconButton(
                      icon: Icon(Icons.more_vert),
                      onPressed: () {
                        _showPopupMenu();
                      }),
                ],
              ),
              blendMode: BlendMode.srcATop,
              shaderCallback: (Rect bounds) {
                return LinearGradient(
                    colors: [
                      Theme.of(context).primaryColor,
                      Theme.of(context).accentColor
                    ],
                    begin: Alignment.topRight,
                    end: Alignment.bottomLeft,
                    stops: [0.0, 1.0]).createShader(bounds);
              }),
        ],
        leading: ShaderMask(
            child: IconButton(
                icon: Icon(Icons.arrow_back),
                onPressed: () {
                  Navigator.pop(context);
                }),
            shaderCallback: (Rect bounds) {
              return LinearGradient(
                  colors: [
                    Theme.of(context).primaryColor,
                    Theme.of(context).accentColor
                  ],
                  begin: Alignment.topRight,
                  end: Alignment.bottomLeft,
                  stops: [0.0, 1.0]).createShader(bounds);
            }),
        title: Row(
          mainAxisAlignment: MainAxisAlignment.start,
          mainAxisSize: MainAxisSize.min,
          children: [
            ClipRRect(
              borderRadius: BorderRadius.circular(120.sp),
              child: Image.network(
                widget.user == null
                    ? widget.usersSimilar.avatarPath
                    : widget.user.userInfo.avatarPath,
                fit: BoxFit.cover,
                height: 100.h,
                width: 100.h,
              ),
            ),
            SizedBox(width: 20.w),
            Expanded(
              child: Container(
                padding: EdgeInsets.fromLTRB(
                    ScreenUtil().setWidth(10.0), 0.0, 0.0, 0.0),
                margin: EdgeInsets.fromLTRB(0.0, 0.0, 0.0, 0.0),
                child: Text(
                  widget.user == null
                      ? widget.usersSimilar.fullName
                      : widget.user.userInfo.fullName,
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                  style: TextStyle(
                    fontSize: 45.sp,
                    color: Colors.grey.shade600,
                  ),
                ),
              ),
            )
          ],
        ),
      ),
      body: Column(
        children: <Widget>[
          Expanded(
            flex: 12,
            child: Consumer<MessageProvider>(
              builder: (context, messages, child) {
                return Padding(
                  padding: EdgeInsets.only(
                    left: 20.w,
                    right: 20.w,
                  ),
                  child: FutureBuilder(
                    future: getDataMessages(messages, widget.user),
                    builder: (context, AsyncSnapshot<bool> snapshot) {
                      Widget child;
                      if (snapshot.hasData &&
                          messages.userChatCurrent != null) {
                        child = ListView.builder(
                          itemCount:
                              messages.userChatCurrent.messagesUser.length,
                          reverse: true,
                          itemBuilder: (context, index) {
                            return messages.userChatCurrent.messagesUser[index]
                                        .senderId ==
                                    Provider.of<AuthenticationProvider>(context)
                                        .getUserInfo
                                        .id
                                ? Bubble(
                                    margin: BubbleEdges.only(
                                        top: (index <
                                                    messages
                                                            .userChatCurrent
                                                            .messagesUser
                                                            .length -
                                                        1 &&
                                                messages
                                                        .userChatCurrent
                                                        .messagesUser[index + 1]
                                                        .senderId !=
                                                    messages.userChatCurrent
                                                        .userInfo.id)
                                            ? 5.h
                                            : 20.h,
                                        left: 300.w,
                                        bottom: index == 0 ? 10.h : 10.h),
                                    nip: (index <
                                                messages.userChatCurrent
                                                        .messagesUser.length -
                                                    2 &&
                                            messages
                                                    .userChatCurrent
                                                    .messagesUser[index + 1]
                                                    .senderId !=
                                                messages.userChatCurrent
                                                    .userInfo.id)
                                        ? BubbleNip.no
                                        : BubbleNip.rightBottom,
                                    nipRadius: 11.w,
                                    color: Colors.blue.shade300,
                                    style: BubbleStyle(
                                      radius: Radius.circular(40.w),
                                    ),
                                    nipHeight: 23.h,
                                    nipWidth: 23.w,
                                    alignment: Alignment.centerRight,
                                    elevation: 0.4,
                                    padding: messages
                                                .userChatCurrent
                                                .messagesUser[index]
                                                .messageType !=
                                            "image"
                                        ? BubbleEdges.symmetric(
                                            horizontal: 20.sp, vertical: 15.sp)
                                        : BubbleEdges.all(0.sp),
                                    child: messages
                                                .userChatCurrent
                                                .messagesUser[index]
                                                .messageType !=
                                            "image"
                                        ? Text(
                                            messages.userChatCurrent
                                                .messagesUser[index].content,
                                            style: TextStyle(
                                                color: Colors.white,
                                                fontSize: 43.sp,
                                                fontWeight: FontWeight.w400),
                                          )
                                        : _buildGridViewShowImage(
                                            messages.userChatCurrent
                                                .messagesUser[index].filePaths,
                                          ),
                                  )
                                : Bubble(
                                    margin: BubbleEdges.only(
                                        top: (index <
                                                    messages
                                                            .userChatCurrent
                                                            .messagesUser
                                                            .length -
                                                        2 &&
                                                messages
                                                        .userChatCurrent
                                                        .messagesUser[index + 1]
                                                        .receiverId ==
                                                    messages.userChatCurrent
                                                        .userInfo.id)
                                            ? 20.h
                                            : 5.h,
                                        right: 300.w),
                                    nip: (index <
                                                messages.userChatCurrent
                                                        .messagesUser.length -
                                                    2 &&
                                            messages
                                                    .userChatCurrent
                                                    .messagesUser[index + 1]
                                                    .receiverId ==
                                                messages.userChatCurrent
                                                    .userInfo.id)
                                        ? BubbleNip.no
                                        : BubbleNip.leftBottom,
                                    color: Colors.blueGrey.shade50,
                                    nipHeight: 23.h,
                                    nipWidth: 23.w,
                                    nipRadius: 11.w,
                                    style: BubbleStyle(
                                      radius: Radius.circular(40.w),
                                    ),
                                    alignment: Alignment.centerLeft,
                                    padding: messages
                                                .userChatCurrent
                                                .messagesUser[index]
                                                .messageType !=
                                            "image"
                                        ? BubbleEdges.symmetric(
                                            horizontal: 20.sp, vertical: 15.sp)
                                        : BubbleEdges.all(0.sp),
                                    elevation: 0.4,
                                    child: messages
                                                .userChatCurrent
                                                .messagesUser[index]
                                                .messageType !=
                                            "image"
                                        ? Text(
                                            messages.userChatCurrent
                                                .messagesUser[index].content,
                                            style: TextStyle(
                                                color: Colors.black87,
                                                fontSize: 43.sp,
                                                fontWeight: FontWeight.w400),
                                          )
                                        : _buildGridViewShowImage(
                                            messages.userChatCurrent
                                                .messagesUser[index].filePaths,
                                          ),
                                  );
                          },
                        );
                      } else if (snapshot.hasError) {
                        child = ErrorData(
                          titleError:
                              "We couldn't find history messages. Try again later",
                        );
                      } else if (!loadData ||
                          messages.userChatCurrent == null) {
                        child = SearchMatching(
                          atCenter: true,
                          chng: true,
                          title: "Load Messages User...",
                        );
                      }
                      return child;
                    },
                  ),
                );
              },
            ),
          ),
          Container(
            padding: EdgeInsets.only(top: 25.h, bottom: 15.h),
            height: isImage ? 400.h : 170.h,
            decoration: BoxDecoration(
                color: Colors.transparent,
                borderRadius: BorderRadius.circular(120.sp)),
            child: Consumer<MessageProvider>(
              builder: (context, message, child) {
                return Padding(
                  padding:
                      EdgeInsets.symmetric(horizontal: 10.w, vertical: 0.h),
                  child: Row(
                    children: <Widget>[
                      Expanded(
                        flex: 8,
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            isImage
                                ? _buildGridViewChoiceImage(
                                    userProfile.getImages)
                                : Container(),
                            SizedBox(height: 10.w),
                            Container(
                              height: 120.h,
                              decoration: BoxDecoration(
                                  color: Colors.white,
                                  boxShadow: [
                                    BoxShadow(
                                        offset: Offset(0.0, 0.0),
                                        color: Colors.grey)
                                  ],
                                  borderRadius: BorderRadius.circular(50.0)),
                              child: Align(
                                alignment: Alignment.center,
                                child: TextField(
                                  onChanged: (value) {
                                    contentChat = value;
                                  },
                                  controller: _messageController,
                                  cursorColor: Theme.of(context).primaryColor,
                                  decoration: InputDecoration(
                                      hintText: "Type a message",
                                      border: InputBorder.none,
                                      enabled: !isImage,
                                      suffixIcon: Padding(
                                        padding: EdgeInsets.only(right: 20.w),
                                        child: Row(
                                          mainAxisSize: MainAxisSize.min,
                                          children: <Widget>[
                                            IconButton(
                                              padding: EdgeInsets.all(0.sp),
                                              constraints: BoxConstraints(),
                                              icon: Icon(
                                                Icons.attachment,
                                                color: Colors.grey,
                                              ),
                                              onPressed: () {},
                                            ),
                                            SizedBox(
                                              width: 15.w,
                                            ),
                                            IconButton(
                                              padding: EdgeInsets.all(0.sp),
                                              constraints: BoxConstraints(),
                                              icon: Icon(
                                                Icons.camera_alt,
                                                color: Colors.grey,
                                              ),
                                              onPressed: () async {
                                                await Provider.of<UserProvider>(
                                                        context,
                                                        listen: false)
                                                    .loadAssets();
                                                setState(() {
                                                  isImage = true;
                                                });
                                              },
                                            ),
                                          ],
                                        ),
                                      ),
                                      prefixIcon: Icon(
                                        Icons.sentiment_satisfied,
                                        size: 70.sp,
                                        color: Colors.grey,
                                      )),
                                ),
                              ),
                            )
                          ],
                        ),
                      ),
                      SizedBox(
                        width: 10.w,
                      ),
                      Expanded(
                          flex: 1,
                          child: Container(
                              height: double.infinity,
                              decoration: BoxDecoration(
                                  color: Colors.blue.shade300,
                                  borderRadius: BorderRadius.circular(120.sp)),
                              child: MaterialButton(
                                padding: EdgeInsets.zero,
                                onPressed: () {
                                  setState(() {
                                    message.sendMessage(
                                      messages.userChatCurrent.userInfo.id,
                                      isImage ? 'file' : contentChat,
                                      isImage
                                          ? Provider.of<UserProvider>(context,
                                                  listen: false)
                                              .getImages
                                          : [],
                                    );
                                    setState(() {
                                      contentChat = '';
                                      isImage = false;
                                      Provider.of<UserProvider>(context,
                                              listen: false)
                                          .cancelAddImages();
                                    });

                                    _messageController.clear();
                                  });
                                },
                                child: Center(
                                  child: Icon(
                                    Icons.send,
                                    size: 70.sp,
                                    color: Colors.white,
                                  ),
                                ),
                              )))
                    ],
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}
