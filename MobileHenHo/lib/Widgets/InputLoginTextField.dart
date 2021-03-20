import 'package:flutter/material.dart';

class InputLoginTextField extends StatelessWidget {
  final Function onChanged;
  final titleTextFile;
  final bool obscureText;
  InputLoginTextField(
      {@required this.titleTextFile,
      @required this.onChanged,
      @required this.obscureText});
  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsets.all(10),
      decoration: BoxDecoration(
          border: Border(bottom: BorderSide(color: Colors.grey[200]))),
      child: TextField(
        obscureText: obscureText,
        decoration: InputDecoration(
            hintText: titleTextFile,
            hintStyle: TextStyle(color: Colors.grey),
            border: InputBorder.none),
        onChanged: onChanged,
      ),
    );
  }
}
