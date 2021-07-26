class INotification {
  final int id;
  final String fullName;
  final String fromId;
  final String content;
  final String toId;
  final String type;
  final DateTime createdAt;
  final String avatar;

  INotification({
    this.id,
    this.fullName,
    this.fromId,
    this.content,
    this.toId,
    this.type,
    this.createdAt,
    this.avatar,
  });
}
