class Internet {
  Internet({this.urlAPI}) {
    urlMain = urlMain + urlAPI;
  }
  //String urlMain = 'http://10.0.2.2:5100/api/v1';
  String urlMain = 'https://hieuit.tech:5201/api/v1';

  final String urlAPI;
}
