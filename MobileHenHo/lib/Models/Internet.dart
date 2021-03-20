class Internet {
  Internet({this.urlAPI}) {
    urlMain = urlMain + urlAPI;
  }
  String urlMain = 'http://10.0.2.2:5100/api/v1';
  //String urlMain = 'http://hieuit.tech:5400';
  //String urlMain = 'http://hieuit.tech:5100';

  final String urlAPI;
}
