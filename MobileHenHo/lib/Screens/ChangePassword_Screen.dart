import 'package:flutter/material.dart';
import 'package:modal_progress_hud/modal_progress_hud.dart';
import 'package:provider/provider.dart';
import 'package:tinder_clone/Models/Providers/AuthenticationProvider.dart';
import 'package:tinder_clone/Models/Providers/Loading.dart';
import 'package:tinder_clone/Widgets/SaveAndCanceSetting.dart';
import 'package:tinder_clone/contants.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class ChangePasswordScreen extends StatefulWidget {
  @override
  _ChangePasswordScreenState createState() => _ChangePasswordScreenState();
}

class _ChangePasswordScreenState extends State<ChangePasswordScreen> {
  bool isUpdate;
  bool isCheckNewPass;
  bool isCheckInfo;
  TextEditingController oldPass;
  TextEditingController newPass;
  TextEditingController againPass;
  @override
  void initState() {
    super.initState();
    oldPass = new TextEditingController();
    newPass = new TextEditingController();
    againPass = new TextEditingController();
  }

  @override
  Widget build(BuildContext context) {
    var authen = Provider.of<AuthenticationProvider>(context);
    return ModalProgressHUD(
      inAsyncCall: Provider.of<Loading>(context).loading,
      child: Scaffold(
        backgroundColor: Colors.grey.shade200,
        appBar: AppBar(
          flexibleSpace: Container(
            decoration: BoxDecoration(
              gradient: colorApp,
            ),
          ),
          title: Text(
            'ĐỔI MẬT KHẨU',
          ),
        ),
        body: Container(
          child: SingleChildScrollView(
            padding: EdgeInsets.all(40.h),
            child: Column(children: [
              isUpdate != null
                  ? isUpdate
                      ? Row(
                          children: [
                            Container(
                              padding: EdgeInsets.only(left: 50.w, right: 20.w),
                              child: Text(
                                "Cập nhật thành công",
                                style: TextStyle(
                                  color: Colors.grey,
                                  fontStyle: FontStyle.italic,
                                ),
                              ),
                            ),
                            Icon(
                              Icons.info_outline,
                              color: Colors.green,
                            )
                          ],
                        )
                      : Row(
                          children: [
                            Container(
                              padding: EdgeInsets.only(left: 50.w, right: 20.w),
                              child: Text(
                                "Mật khẩu không chính xác",
                                style: TextStyle(
                                  color: Colors.grey,
                                  fontStyle: FontStyle.italic,
                                ),
                              ),
                            ),
                            Icon(
                              Icons.dangerous,
                              color: Colors.red,
                            )
                          ],
                        )
                  : Container(),
              isCheckInfo != null
                  ? isCheckInfo
                      ? Row(
                          children: [
                            Container(
                              padding: EdgeInsets.only(left: 50.w, right: 20.w),
                              child: Text(
                                "Vui lòng điền đầy đủ thông tin",
                                style: TextStyle(
                                  color: Colors.grey,
                                  fontStyle: FontStyle.italic,
                                ),
                              ),
                            ),
                            Icon(
                              Icons.dangerous,
                              color: Colors.red,
                            )
                          ],
                        )
                      : Row(
                          children: [
                            Container(
                              padding: EdgeInsets.only(left: 50.w, right: 20.w),
                              child: Text(
                                "Xác nhận mật khẩu không chính xác",
                                style: TextStyle(
                                  color: Colors.grey,
                                  fontStyle: FontStyle.italic,
                                ),
                              ),
                            ),
                            Icon(
                              Icons.dangerous,
                              color: Colors.red,
                            )
                          ],
                        )
                  : Container(),
              Card(
                elevation: 0.5,
                margin: EdgeInsets.symmetric(
                  vertical: 10.h,
                  horizontal: 0,
                ),
                child: Column(
                  children: [
                    FieldChangePassword(
                      title: "Mật khẩu cũ",
                      myControler: oldPass,
                      onChanged: (value) {
                        setState(() {
                          isUpdate = null;
                          isCheckInfo = null;
                        });
                      },
                    ),
                    FieldChangePassword(
                      title: "Mật khẩu mới",
                      myControler: newPass,
                      onChanged: (value) {
                        setState(() {
                          isUpdate = null;
                          isCheckInfo = null;
                        });
                      },
                    ),
                    FieldChangePassword(
                      title: "Xác nhận mật khẩu mới",
                      myControler: againPass,
                      isCheck: isCheckNewPass,
                      onChanged: (value) {
                        if (againPass.text == newPass.text) {
                          setState(() {
                            isCheckNewPass = true;
                          });
                        } else {
                          setState(() {
                            isCheckNewPass = false;
                          });
                        }
                        setState(() {
                          isUpdate = null;
                          isCheckInfo = null;
                        });
                      },
                    ),
                  ],
                ),
              ),
              SaveAndCanceSetting(
                onPressSave: () async {
                  if (oldPass.text == '' &&
                      newPass.text == '' &&
                      againPass.text == '') {
                    setState(() {
                      isCheckInfo = true;
                    });
                    return;
                  }

                  if (newPass.text != againPass.text) {
                    setState(() {
                      isCheckInfo = false;
                    });
                    return;
                  }

                  Provider.of<Loading>(context, listen: false).setLoading();
                  var reponse = await authen.changePassword(
                      oldPass.text, newPass.text, againPass.text);
                  Provider.of<Loading>(context, listen: false).setLoading();
                  isUpdate = reponse;
                },
                onPRessCance: () {
                  setState(() {
                    oldPass = new TextEditingController();
                    newPass = new TextEditingController();
                    againPass = new TextEditingController();
                    isCheckNewPass = null;
                    isCheckInfo = null;
                  });
                },
              ),
            ]),
          ),
        ),
      ),
    );
  }
}

class FieldChangePassword extends StatefulWidget {
  final String title;
  final Function onChanged;
  final bool isCheck;
  final TextEditingController myControler;

  FieldChangePassword({
    this.myControler,
    this.title,
    this.onChanged,
    this.isCheck,
  });

  @override
  _FieldChangePasswordState createState() => _FieldChangePasswordState();
}

class _FieldChangePasswordState extends State<FieldChangePassword> {
  bool isNotSeen = true;

  @override
  Widget build(BuildContext context) {
    return TextField(
      obscureText: isNotSeen,
      controller: widget.myControler,
      onChanged: widget.onChanged,
      decoration: InputDecoration(
        suffixIcon: Container(
          width: 200.w,
          child: Row(
            mainAxisAlignment: MainAxisAlignment.end,
            children: [
              widget.title == "Xác nhận mật khẩu mới"
                  ? widget.isCheck != null
                      ? widget.isCheck
                          ? Icon(
                              Icons.beenhere_outlined,
                              color: Colors.green,
                              size: 50.sp,
                            )
                          : Icon(
                              Icons.cancel_outlined,
                              color: Colors.red,
                              size: 50.sp,
                            )
                      : Container()
                  : Container(),
              IconButton(
                padding: EdgeInsets.zero,
                onPressed: () {
                  setState(() {
                    isNotSeen = !isNotSeen;
                  });
                },
                icon: Icon(isNotSeen ? Icons.visibility_off : Icons.visibility),
              ),
            ],
          ),
        ),
        contentPadding: EdgeInsets.all(40.h),
        hintText: widget.title,
        focusedBorder: OutlineInputBorder(
            borderSide: BorderSide(
          style: BorderStyle.solid,
          color: Theme.of(context).accentColor,
          width: 2.h,
        )),
        enabledBorder: UnderlineInputBorder(
          borderSide: BorderSide(
            style: BorderStyle.solid,
            color: Colors.grey.shade300,
            width: 2.h,
          ),
        ),
      ),
      textAlign: TextAlign.start,
      style: TextStyle(color: Colors.grey),
    );
  }
}
