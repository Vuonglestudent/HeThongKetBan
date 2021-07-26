import 'package:flutter/material.dart';
import 'package:material_floating_search_bar/material_floating_search_bar.dart';
import 'package:provider/provider.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Models/User.dart';
import 'package:tinder_clone/Screens/ListImage_Screen.dart';
import 'package:tinder_clone/Screens/ProfileDetail_Screen.dart';
import 'package:tinder_clone/Screens/Zinger_Screen.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:tinder_clone/Widgets/Notification.dart';

class FriendsScreen extends StatefulWidget {
  @override
  _FriendsScreenState createState() => _FriendsScreenState();
}

class _FriendsScreenState extends State<FriendsScreen> {
  bool getFriendList = false;
  FloatingSearchBarController controller;
  UserProvider _userProvider;
  List<String> filteredSearchHistory;
  Future<List<String>> filterSearchTerms({
    @required String filter,
  }) async {
    if (filter != null && filter.isNotEmpty && filter != '') {
      getFriendList = false;
      getFriendList = await _userProvider.getSearchFriendByName(filter);
      return _userProvider.filterFriendList
          .map((e) => e.fullName)
          .toList()
          .toList();
    } else {
      return filteredSearchHistory.toList();
    }
  }

  @override
  void initState() {
    super.initState();
    controller = FloatingSearchBarController();
    controller.query = '';
    filteredSearchHistory = [];
  }

  @override
  void dispose() {
    controller.dispose();
    super.dispose();
  }

  Future<bool> getFriends(UserProvider user) async {
    if (!getFriendList && user.friendList.length == 0) {
      getFriendList = await user.getFriendsUser();
    } else {
      Future.delayed(Duration(milliseconds: 1), () => getFriendList = true);
    }
    return getFriendList;
  }

  @override
  Widget build(BuildContext context) {
    _userProvider = Provider.of<UserProvider>(context);
    return Scaffold(
      body: Consumer<UserProvider>(
        builder: (context, user, child) {
          return FutureBuilder<bool>(
            future: getFriends(user),
            builder: (context, AsyncSnapshot<bool> snapshot) {
              Widget child;
              if (snapshot.hasData) {
                child = Scaffold(
                  body: SafeArea(
                    child: FloatingSearchBar(
                      controller: controller,
                      transition: CircularFloatingSearchBarTransition(),
                      physics: BouncingScrollPhysics(),
                      iconColor: Theme.of(context).secondaryHeaderColor,
                      automaticallyImplyBackButton: false,
                      leadingActions: [
                        FloatingSearchBarAction.back(
                          showIfClosed: false,
                        ),
                        FloatingSearchBarAction(
                          showIfClosed: true,
                          child: CircularButton(
                            icon: const Icon(Icons.person),
                            onPressed: () {},
                          ),
                        ),
                      ],
                      title: Text(
                        'Tìm kiếm bạn bè ...',
                        style: TextStyle(fontSize: 50.sp),
                      ),
                      hint: 'Tìm kiếm bạn bè ...',
                      actions: [
                        FloatingSearchBarAction.searchToClear(),
                      ],
                      onQueryChanged: (query) async {
                        await _userProvider.getSearchFriendByName(query);
                      },
                      onSubmitted: (query) {
                        setState(() {
                          filterSearchTerms(filter: query);
                        });
                        controller.close();
                      },
                      body: FloatingSearchBarScrollNotifier(
                        child: SearchResultsListView(),
                      ),
                      builder: (context, transition) {
                        return ClipRRect(
                          borderRadius: BorderRadius.circular(8),
                          child: Material(
                            color: Colors.white,
                            elevation: 4,
                            child: Builder(
                              builder: (context) {
                                if (filteredSearchHistory.isEmpty &&
                                    controller.query.isEmpty) {
                                  return Container(
                                    height: 56,
                                    width: double.infinity,
                                    alignment: Alignment.center,
                                    child: ListTile(
                                      title: Text(
                                        "Lấy toàn bộ danh sách",
                                        maxLines: 1,
                                        overflow: TextOverflow.ellipsis,
                                      ),
                                      leading: const Icon(Icons.search),
                                      onTap: () {
                                        setState(() {
                                          filterSearchTerms(filter: 'All');
                                        });
                                        controller.close();
                                      },
                                    ),
                                  );
                                } else if (_userProvider
                                    .filterFriendList.isEmpty) {
                                  return Container(
                                    height: 30,
                                    width: double.infinity,
                                    alignment: Alignment.center,
                                    child: Text(
                                      'Không tìm thấy bạn bè tương ứng',
                                      maxLines: 1,
                                      overflow: TextOverflow.ellipsis,
                                      style:
                                          Theme.of(context).textTheme.caption,
                                    ),
                                  );
                                } else {
                                  return Column(
                                    mainAxisSize: MainAxisSize.min,
                                    children: _userProvider.filterFriendList
                                        .map(
                                          (term) => ListTile(
                                            title: Text(
                                              term.fullName,
                                              maxLines: 1,
                                              overflow: TextOverflow.ellipsis,
                                            ),
                                            leading: const Icon(Icons.search),
                                            onTap: () {
                                              setState(() {
                                                filterSearchTerms(
                                                    filter: term.fullName);
                                              });
                                              controller.close();
                                            },
                                          ),
                                        )
                                        .toList(),
                                  );
                                }
                              },
                            ),
                          ),
                        );
                      },
                    ),
                  ),
                );
              } else if (!getFriendList) {
                child = SearchMatching(
                    atCenter: true, chng: true, title: "Lấy danh sách bạn bè");
              }
              return child != null
                  ? child
                  : SearchMatching(
                      atCenter: true,
                      chng: true,
                      title: "Lấy danh sách bạn bè",
                    );
            },
          );
        },
      ),
    );
  }
}

class SearchResultsListView extends StatefulWidget {
  @override
  _SearchResultsListViewState createState() => _SearchResultsListViewState();
}

class _SearchResultsListViewState extends State<SearchResultsListView> {
  UserProvider _userProvider;
  List<UsersSimilar> _list;
  ScrollController scrollController;
  bool isLoad = false;
  void setListShow() {
    setState(() {
      _list = [];
      _list = _userProvider.filterFriendList.length > 0
          ? _userProvider.filterFriendList
          : _userProvider.friendList;
    });
  }

  @override
  void initState() {
    super.initState();
    scrollController = new ScrollController()..addListener(_scrollListener);
  }

  @override
  void dispose() {
    scrollController.removeListener(_scrollListener);
    super.dispose();
  }

  void _scrollListener() {
    print(scrollController.position.extentAfter);
    if (scrollController.position.extentAfter < 50) {
      setState(() {
        isLoad = true;
        _userProvider.getFriendsUser();
      });
    } else {
      setState(() {
        isLoad = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    _userProvider = Provider.of<UserProvider>(context);
    final fsb = FloatingSearchBar.of(context);
    setListShow();
    return ListView(
      controller: scrollController,
      padding: EdgeInsets.only(top: fsb.height + fsb.margins.vertical),
      children: [
        ListBody(
          children: List.generate(_list.length, (index) {
            return Consumer<AuthenticationProvider>(
              builder: (context, user, child) {
                return Consumer<UserProvider>(
                  builder: (context, profile, child) {
                    return GestureDetector(
                      onTap: () {},
                      child: Column(
                        children: <Widget>[
                          SizedBox(
                            height: 15.h,
                          ),
                          Container(
                            height: 230.h,
                            margin: EdgeInsets.symmetric(horizontal: 40.w),
                            child: Row(
                              children: <Widget>[
                                Container(
                                  height: 200.h,
                                  width: 200.h,
                                  decoration: BoxDecoration(
                                    borderRadius: BorderRadius.circular(120.sp),
                                  ),
                                  child: ClipRRect(
                                    borderRadius: BorderRadius.circular(120.sp),
                                    child: Image(
                                      fit: BoxFit.cover,
                                      image:
                                          NetworkImage(_list[index].avatarPath),
                                    ),
                                  ),
                                ),
                                SizedBox(
                                  width: 25.w,
                                ),
                                Expanded(
                                    child: Padding(
                                  padding: EdgeInsets.only(left: 15.w),
                                  child: Column(
                                    mainAxisAlignment: MainAxisAlignment.start,
                                    crossAxisAlignment:
                                        CrossAxisAlignment.start,
                                    children: <Widget>[
                                      SizedBox(
                                        height: 30.h,
                                      ),
                                      Text(
                                        _list[index].fullName,
                                        style: TextStyle(
                                            fontSize: 50.sp,
                                            fontWeight: FontWeight.w700),
                                      ),
                                      SizedBox(
                                        height: 10.h,
                                      ),
                                      Expanded(
                                        child: Row(
                                          mainAxisAlignment:
                                              MainAxisAlignment.spaceBetween,
                                          crossAxisAlignment:
                                              CrossAxisAlignment.start,
                                          children: [
                                            Expanded(
                                              child: RaisedButton(
                                                color: Theme.of(context)
                                                    .accentColor,
                                                child: Text("Hồ Sơ",
                                                    style: TextStyle(
                                                      color: Colors.white,
                                                      fontWeight:
                                                          FontWeight.bold,
                                                    )),
                                                onPressed: () {
                                                  Navigator.push(
                                                    context,
                                                    MaterialPageRoute(
                                                      builder: (context) =>
                                                          ProfileDetails(
                                                        authenticationUserId:
                                                            user.getUserInfo.id,
                                                        userId: _list[index].id,
                                                      ),
                                                    ),
                                                  );
                                                },
                                              ),
                                            ),
                                            SizedBox(
                                              width: 20.w,
                                            ),
                                            Expanded(
                                                child: RaisedButton(
                                              color: Theme.of(context)
                                                  .secondaryHeaderColor,
                                              child: Text(
                                                "Hình Ảnh",
                                                style: TextStyle(
                                                  color: Colors.white,
                                                  fontWeight: FontWeight.bold,
                                                ),
                                              ),
                                              onPressed: () async {
                                                await profile
                                                    .getNewImageSeenUserSimilar(
                                                        _list[index].id);
                                                if (profile
                                                        .getImageSeenUserSimilar
                                                        .length >
                                                    0) {
                                                  Navigator.push(
                                                    context,
                                                    MaterialPageRoute(
                                                      builder: (BuildContext
                                                              context) =>
                                                          ListImageScreen(
                                                        userId: profile
                                                            .usersSimilar[index]
                                                            .id,
                                                      ),
                                                    ),
                                                  );
                                                } else {
                                                  notificationWidget(
                                                    context,
                                                    typeNotication:
                                                        AlertType.info,
                                                    title: "Hình ảnh",
                                                    desc:
                                                        "Hiện tại người dùng chưa đăng bất kì ảnh nào.",
                                                    textButton: "OK",
                                                  ).show();
                                                }
                                              },
                                            ))
                                          ],
                                        ),
                                      ),
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
                );
              },
            );
          }),
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
    );
  }
}
