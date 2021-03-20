import 'package:flutter/material.dart';
import 'package:modal_progress_hud/modal_progress_hud.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Models/Image.dart' as image;
import 'package:tinder_clone/Widgets/FullScreenImage.dart';
import 'package:tinder_clone/contants.dart';

class ListImageScreen extends StatefulWidget {
  @override
  _ListImageScreenState createState() => _ListImageScreenState();
}

class _ListImageScreenState extends State<ListImageScreen> {
  Widget buildGridView(List<image.Image> images) {
    if (images != null)
      return Center(
        child: GridView.count(
          crossAxisCount: 3,
          crossAxisSpacing: 2.0,
          mainAxisSpacing: 2.0,
          children: List.generate(images.length, (index) {
            return GestureDetector(
              onTap: () {
                Navigator.push(context, MaterialPageRoute(builder: (_) {
                  return FullScreenImage(
                    imageUrl: images[index].getImagePath,
                    tag: "generate_a_unique_tag",
                  );
                }));
              },
              child: Hero(
                tag: "generate_a_unique_tag",
                child: Container(
                  child: Image(
                    fit: BoxFit.cover,
                    image: NetworkImage(
                      images[index].getImagePath,
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

  @override
  Widget build(BuildContext context) {
    return ModalProgressHUD(
      inAsyncCall: Provider.of<Loading>(context).loading,
      child: Scaffold(
        appBar: AppBar(
          elevation: 0,
          flexibleSpace: Container(
            decoration: BoxDecoration(
              gradient: colorApp,
            ),
          ),
        ),
        body: Stack(
          children: <Widget>[
            ClipPath(
              child: Column(
                children: <Widget>[
                  Expanded(
                    child: Container(
                      child: buildGridView(
                          Provider.of<UserProvider>(context).getlistImagesUser),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
