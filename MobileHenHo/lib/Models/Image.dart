class Image {
  int _id;
  String _userId;
  String _title;
  int _numberOfLikes;
  bool _liked;
  String _imagePath;
  bool _hasImage;
  String _createdAt;

  void setImage(Map<String, dynamic> response) {
    _id = response['id'];
    _userId = response['userId'];
    _title = response['title'];
    _numberOfLikes = response['numberOfLikes'];
    _liked = response['liked'];
    _imagePath = response['imagePath'];
    _hasImage = response['hasImage'];
    _createdAt = response['createdAt'];
  }

  List<Image> setPaths(List<String> paths) {
    List<Image> lstImage = [];
    int count = 0;
    for (String path in paths) {
      Image imgNew = new Image();
      imgNew.setPath(path);
      imgNew.setId(count);
      count++;
      lstImage.add(imgNew);
    }

    return lstImage;
  }

  void setPath(String path) {
    _imagePath = path;
  }

  void setId(int id) {
    _id = id;
  }

  String get getImagePath {
    return _imagePath;
  }

  int get getImageId {
    return _id;
  }
}
