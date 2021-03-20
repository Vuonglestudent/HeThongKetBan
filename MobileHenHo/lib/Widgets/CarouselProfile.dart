import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';

class CarouselProfile extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    if (Provider.of<UserProvider>(context).isAddImages == false) {
      return Positioned(
        bottom: 80.h,
        child: Container(
          height: ScreenUtil().setHeight(350),
          width: MediaQuery.of(context).size.width,
          child: CarouselSlider(
            options: CarouselOptions(
              height: 300.h,
              aspectRatio: 16 / 2,
              viewportFraction: 0.8,
              enableInfiniteScroll: true,
              autoPlayAnimationDuration: Duration(milliseconds: 800),
              autoPlayInterval: Duration(seconds: 2),
              autoPlayCurve: Curves.fastOutSlowIn,
              autoPlay: true,
              enlargeCenterPage: true,
              pauseAutoPlayOnTouch: true,
            ),
            items: [0, 1, 2, 3, 4, 5].map((i) {
              return Builder(
                builder: (context) {
                  return Container(
                    width: ScreenUtil().setWidth(900.0),
                    height: ScreenUtil().setHeight(180),
                    margin: EdgeInsets.all(5.0),
                    decoration: BoxDecoration(
                        color: Colors.white,
                        border: Border.all(
                            color: Theme.of(context).primaryColor, width: 2),
                        boxShadow: [
                          BoxShadow(
                              color: Colors.grey,
                              offset: Offset(0.0, 5.0),
                              blurRadius: 10.0)
                        ],
                        borderRadius: BorderRadius.circular(10.0)),
                    child: Center(
                        child: Column(
                      crossAxisAlignment: CrossAxisAlignment.center,
                      mainAxisSize: MainAxisSize.min,
                      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                      children: <Widget>[
                        Text(
                          quotes[i].heading,
                          textAlign: TextAlign.center,
                          style: TextStyle(
                              fontSize: ScreenUtil().setSp(65.0),
                              fontWeight: FontWeight.w700,
                              color: Colors.black54),
                        ),
                        SizedBox(
                          height: ScreenUtil().setHeight(15.0),
                        ),
                        Text(
                          quotes[i].baseline,
                          textAlign: TextAlign.center,
                          style: TextStyle(
                              fontSize: ScreenUtil().setSp(40.0),
                              fontWeight: FontWeight.w300,
                              color: Colors.black54),
                        ),
                      ],
                    )),
                  );
                },
              );
            }).toList(),
          ),
        ),
      );
    } else {
      return Positioned(
        bottom: 20.h,
        left: 70,
        child: Container(
          color: Colors.grey.shade100,
          child: Center(
            child: Text(
              "Keep add images button to cancel function.",
              style:
                  TextStyle(fontStyle: FontStyle.italic, color: Colors.black45),
            ),
          ),
        ),
      );
    }
  }
}

class Quotes {
  final String heading;
  final String baseline;

  Quotes(this.heading, this.baseline);
}

List<Quotes> quotes = [
  new Quotes("GET TINDER GOLD", "See who likes you & more!"),
  new Quotes("Get matches faster", "Boost your profile once a month!"),
  new Quotes(
      "I meant to swipe right", "Get unlimited Rewinds with Tinder Plus!"),
  new Quotes("Stand out with Super Likes",
      "You're 3 times more likely to get a match!"),
  new Quotes("Increase your chances", "Get unlimited likes with tinder Plus!"),
  new Quotes(
      "Swipe around the world!", "Passport to anywhere with Tinder Plus!"),
];
