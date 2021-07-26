import 'package:flutter/material.dart';

const kLogoTextStyle = TextStyle(
    fontSize: 70.0,
    letterSpacing: 1.2,
    fontWeight: FontWeight.w800,
    color: Colors.white);

const kRuleTextStyle = TextStyle(
  color: Colors.white,
  fontWeight: FontWeight.w500,
);

const kTroubleLoginTextStyle = TextStyle(
  color: Colors.white,
  fontSize: 20.0,
  fontWeight: FontWeight.w600,
);

const kTextFieldDecoration = InputDecoration(
  hintText: 'Enter your ...',
  contentPadding: EdgeInsets.symmetric(vertical: 10.0, horizontal: 20.0),
  border: OutlineInputBorder(
    borderRadius: BorderRadius.all(Radius.circular(32.0)),
  ),
  enabledBorder: OutlineInputBorder(
    borderSide: BorderSide(color: Colors.blueAccent, width: 1.0),
    borderRadius: BorderRadius.all(Radius.circular(32.0)),
  ),
  focusedBorder: OutlineInputBorder(
    borderSide: BorderSide(color: Colors.blueAccent, width: 2.0),
    borderRadius: BorderRadius.all(Radius.circular(32.0)),
  ),
);

const kNameTextStyle = TextStyle(
  letterSpacing: 1.1,
  fontSize: 25.0,
  fontWeight: FontWeight.w400,
);

const kFunctionTextStyle = TextStyle(
  color: Colors.blueGrey,
  fontWeight: FontWeight.w600,
);

const colorApp = LinearGradient(colors: [
  Color.fromRGBO(251, 195, 182, 1.0),
  Color.fromRGBO(219, 138, 172, 1.0),
  Color.fromRGBO(174, 61, 158, 1.0),
]);

const kTitleProfileTextStyle = TextStyle(
  fontWeight: FontWeight.bold,
  fontSize: 18.0,
);

final TextStyle headerStyle = TextStyle(
  color: Colors.grey.shade800,
  fontWeight: FontWeight.bold,
  fontSize: 20.0,
);

Container buildDivider() {
  return Container(
    margin: const EdgeInsets.symmetric(
      horizontal: 8.0,
    ),
    width: double.infinity,
    height: 1.0,
    color: Colors.grey.shade300,
  );
}
