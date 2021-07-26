enum RelationshipType { Khong_co_gi, Dang_tim_hieu, Dang_yeu, Da_ket_hon }

class IRelationship {
  int id;
  String fromId;
  String toId;
  String fromName;
  String toName;
  String fromAvatar;
  String toAvatar;
  DateTime createdAt;
  DateTime updatedAt;
  RelationshipType relationshipType;

  IRelationship({
    this.id,
    this.fromId,
    this.toId,
    this.relationshipType,
    this.createdAt,
    this.updatedAt,
    this.fromName,
    this.toName,
    this.toAvatar,
    this.fromAvatar,
  });
}
