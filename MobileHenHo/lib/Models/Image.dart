class Image {
  int _id;
  String _userId;
  String _title;
  int _numberOfLikes;
  bool _liked;
  String _imagePath;
  bool _hasImage;
  String _createdAt;

  Image(Map<String, dynamic> response) {
    _id = response['id'];
    _userId = response['userId'];
    _title = response['title'];
    _numberOfLikes = response['numberOfLikes'];
    _liked = response['liked'];
    _imagePath = response['imagePath'];
    _hasImage = response['hasImage'];
    _createdAt = response['createdAt'];
  }

  String get getImagePath {
    return _imagePath;
  }
}
