import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/UserProvider.dart';

class CarouselProfile extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    if (Provider.of<UserProvider>(context).isAddImages == false) {
      return Positioned.fill(
        bottom: 60.h,
        top: 1300.h,
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
            items: List.generate(quotes.length, (i) {
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
            }),
          ),
        ),
      );
    } else {
      return Positioned.fill(
        bottom: 20.h,
        top: 1620.h,
        child: Container(
          color: Colors.grey.shade100,
          child: Center(
            child: Text(
              "Gi??? n??t TH??M ???NH ????? h???y ch???c n??ng",
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
  new Quotes("B???n b?? chung s??? th??ch", "G???i ?? nh???ng b???n b?? c?? s??? th??ch chung"),
  new Quotes("B???n b?? g???n ????y", "T??m ki???m b???n b?? c?? v??? tr?? g???n b???n"),
  new Quotes("Tr?? chuy???n kh??ng gi???i h???n",
      "Nh???n tin v?? g???i ??i???n v???i b???n b?? m???i l??c, m???i n??i"),
  new Quotes("C??c c??u truy???n hay",
      "?????c v?? vi???t c??c c??u truy???n v??? ch??nh m??nh l??n ???ng d???ng"),
];
