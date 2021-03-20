import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:modal_progress_hud/modal_progress_hud.dart';
import 'package:multi_image_picker/multi_image_picker.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Screens/ProfileDetail_Screen.dart';
import 'package:tinder_clone/Widgets/UploadApiImagesButton.dart';
import 'package:tinder_clone/contants.dart';
import 'package:tinder_clone/Widgets/AddImagesProfile.dart';
import 'package:tinder_clone/Widgets/IconsProfileExpanded.dart';
import 'package:tinder_clone/Widgets/CarouselProfile.dart';
import 'package:tinder_clone/Widgets/MyClipper.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';

class ProfileScreen extends StatefulWidget {
  static const String id = 'Profile_Screen';
  @override
  _ProfileScreenState createState() => _ProfileScreenState();
}

class _ProfileScreenState extends State<ProfileScreen> {
  @override
  void didChangeDependencies() {
    precacheImage(
        NetworkImage(Provider.of<AuthenticationProvider>(context)
            .getUserInfo
            .avatarPath),
        context);
    super.didChangeDependencies();
  }

  Widget buildGridView(List<Asset> images) {
    if (images != null)
      return Center(
        child: GridView.count(
          crossAxisCount: 3,
          crossAxisSpacing: 2.0,
          mainAxisSpacing: 2.0,
          children: List.generate(images.length, (index) {
            Asset asset = images[index];
            return AssetThumb(
              asset: asset,
              width: 300,
              height: 300,
            );
          }),
        ),
      );
    else
      return Container(color: Colors.white);
  }

  @override
  Widget build(BuildContext context) {
    return Consumer<AuthenticationProvider>(
      builder: (context, authentication, child) {
        return ModalProgressHUD(
          inAsyncCall: Provider.of<Loading>(context).loading,
          child: Stack(
            children: <Widget>[
              Container(
                height: double.infinity,
                width: MediaQuery.of(context).size.width,
                color: Colors.grey.shade100,
              ),
              ClipPath(
                clipBehavior: Clip.antiAlias,
                clipper: MyClipper(),
                child: Container(
                  height: Provider.of<UserProvider>(context).isAddImages == true
                      ? MediaQuery.of(context).size.height * 0.95
                      : MediaQuery.of(context).size.height * 0.725,
                  decoration: BoxDecoration(color: Colors.white, boxShadow: [
                    BoxShadow(
                        color: Colors.grey,
                        offset: Offset(1.0, 10.0),
                        blurRadius: 10.0)
                  ]),
                  child: Column(
                    children: <Widget>[
                      Expanded(
                        flex: 4,
                        child: Container(
                          child: Column(
                            children: <Widget>[
                              SizedBox(height: 50.h),
                              Container(
                                  width: 315.w,
                                  height: 300.h,
                                  child: CircleAvatar(
                                    backgroundImage: NetworkImage(
                                        authentication.getUserInfo.avatarPath),
                                  )),
                              SizedBox(height: 15.h),
                              Text(
                                authentication.getUserInfo.fullName,
                                style: kNameTextStyle,
                              ),
                              SizedBox(
                                height: 25.h,
                              ),
                              Expanded(
                                  child: Row(
                                children: <Widget>[
                                  IconsProfileExpanded(
                                    titleButton: "SETTINGS",
                                    icon: Icons.settings,
                                    onPressed: () {},
                                  ),
                                  AddImagesProfile(),
                                  IconsProfileExpanded(
                                    titleButton: "PROFILES",
                                    icon: Icons.article_outlined,
                                    onPressed: () => Navigator.push(
                                      context,
                                      MaterialPageRoute(
                                        builder: (context) => ProfileDetails(
                                          authenticationUserId:
                                              authentication.getUserInfo.id,
                                          userId: authentication.getUserInfo.id,
                                        ),
                                      ),
                                    ),
                                  ),
                                ],
                              ))
                            ],
                          ),
                        ),
                      ),
                      Expanded(
                        flex: 3,
                        child: Container(
                          child: buildGridView(
                              Provider.of<UserProvider>(context).getImages),
                        ),
                      ),
                      UploadApiImagesButton(
                          checkCreateButton:
                              Provider.of<UserProvider>(context).isAddImages),
                    ],
                  ),
                ),
              ),
              CarouselProfile(),
            ],
          ),
        );
      },
    );
  }
}
