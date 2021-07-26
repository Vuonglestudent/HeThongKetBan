import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:photo_view/photo_view.dart';
import 'package:photo_view/photo_view_gallery.dart';
import 'package:tinder_clone/Models/Image.dart' as img;

class FullScreenImage extends StatelessWidget {
  final List<img.Image> galleryItems;
  final String imageUrl;
  final int tag;

  const FullScreenImage({Key key, this.imageUrl, this.tag, this.galleryItems})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
        child: PhotoViewGallery.builder(
      pageController: PageController(initialPage: tag),
      scrollPhysics: BouncingScrollPhysics(),
      builder: (BuildContext context, int index) {
        return PhotoViewGalleryPageOptions(
          imageProvider: this.galleryItems == null
              ? NetworkImage(this.imageUrl)
              : NetworkImage(galleryItems[index].getImagePath),
          initialScale: PhotoViewComputedScale.contained,
          heroAttributes: this.galleryItems == null
              ? PhotoViewHeroAttributes(tag: 0)
              : PhotoViewHeroAttributes(tag: galleryItems[index].getImageId),
        );
      },
      itemCount: this.galleryItems == null ? 1 : galleryItems.length,
      loadingBuilder: (context, event) => Center(
        child: Container(
          width: 20.0,
          height: 20.0,
          child: CircularProgressIndicator(
            value: event == null
                ? 0
                : event.cumulativeBytesLoaded / event.expectedTotalBytes,
          ),
        ),
      ),
    ));
  }
}

// return Scaffold(
//   backgroundColor: Colors.black87,
//   body: GestureDetector(
//     child: Center(
//       child: Hero(
//         tag: tag,
//         child: CachedNetworkImage(
//           width: MediaQuery.of(context).size.width,
//           fit: BoxFit.contain,
//           imageUrl: imageUrl,
//         ),
//       ),
//     ),
//     onTap: () {
//       Navigator.pop(context);
//     },
//   ),
// );
