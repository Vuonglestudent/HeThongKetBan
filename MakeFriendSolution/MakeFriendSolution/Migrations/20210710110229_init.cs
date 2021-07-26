using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MakeFriendSolution.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accesses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    AuthorizeCount = table.Column<int>(nullable: false, defaultValue: 0),
                    UnauthorizeCount = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    WeightRate = table.Column<double>(nullable: false, defaultValue: 1.0),
                    IsCalculated = table.Column<bool>(nullable: false),
                    IsSearchFeature = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageScores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    AutoFilter = table.Column<bool>(nullable: false),
                    Drawings = table.Column<double>(nullable: false, defaultValue: 0.0),
                    Hentai = table.Column<double>(nullable: false, defaultValue: 0.0),
                    Neutral = table.Column<double>(nullable: false, defaultValue: 0.0),
                    Porn = table.Column<double>(nullable: false, defaultValue: 0.0),
                    Sexy = table.Column<double>(nullable: false, defaultValue: 0.0),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageScores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FromId = table.Column<Guid>(nullable: false),
                    ToId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimilariryFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2021, 7, 10, 18, 2, 28, 813, DateTimeKind.Local).AddTicks(8189))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimilariryFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 200, nullable: false),
                    UserName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PassWord = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    Role = table.Column<int>(nullable: false),
                    TypeAccount = table.Column<int>(nullable: false, defaultValue: 0),
                    FullName = table.Column<string>(maxLength: 200, nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    AvatarPath = table.Column<string>(nullable: true),
                    Location = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2021, 7, 10, 18, 2, 28, 787, DateTimeKind.Local).AddTicks(9589)),
                    NumberOfFiends = table.Column<int>(nullable: false, defaultValue: 0),
                    NumberOfLikes = table.Column<int>(nullable: false, defaultValue: 0),
                    NumberOfImages = table.Column<int>(nullable: false, defaultValue: 0),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    IsUpdatePosition = table.Column<bool>(nullable: false),
                    IsInfoUpdated = table.Column<bool>(nullable: false, defaultValue: false),
                    PasswordForgottenCode = table.Column<string>(nullable: true, defaultValue: ""),
                    PasswordForgottenPeriod = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    NumberOfPasswordConfirmations = table.Column<int>(nullable: false, defaultValue: 0),
                    Title = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    Weight = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Dob = table.Column<DateTime>(nullable: false),
                    Job = table.Column<int>(nullable: false),
                    FindPeople = table.Column<int>(nullable: false),
                    FindAgeGroup = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: false),
                    Weight = table.Column<int>(nullable: false, defaultValue: 1),
                    FeatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureDetails_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlockUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    IsLock = table.Column<bool>(nullable: false),
                    FromUserId = table.Column<Guid>(nullable: false),
                    ToUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockUsers_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BlockUsers_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    FromUserId = table.Column<Guid>(nullable: false),
                    ToUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Content = table.Column<string>(maxLength: 400, nullable: false),
                    Vote = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    FromUserId = table.Column<Guid>(nullable: false),
                    ToUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follows_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Follows_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HaveMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(maxLength: 5000, nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    SentAt = table.Column<DateTime>(nullable: false),
                    SenderId = table.Column<Guid>(nullable: false),
                    ReceiverId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HaveMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HaveMessages_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HaveMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FromId = table.Column<Guid>(nullable: false),
                    ToId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    RelationShipType = table.Column<int>(nullable: false),
                    HasRelationship = table.Column<bool>(nullable: false),
                    IsAccept = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relationships_Users_FromId",
                        column: x => x.FromId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Relationships_Users_ToId",
                        column: x => x.ToId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SimilarityScores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FromUserId = table.Column<Guid>(nullable: false),
                    ToUserId = table.Column<Guid>(nullable: false),
                    Score = table.Column<double>(nullable: false, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimilarityScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimilarityScores_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ThumbnailImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false, defaultValue: "Image title"),
                    ImagePath = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    NumberOflikes = table.Column<int>(nullable: false, defaultValue: 0),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThumbnailImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThumbnailImages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    FeatureDetailId = table.Column<int>(nullable: false),
                    FeatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchFeatures_FeatureDetails_FeatureDetailId",
                        column: x => x.FeatureDetailId,
                        principalTable: "FeatureDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SearchFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SearchFeatures_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    FeatureDetailId = table.Column<int>(nullable: false),
                    FeatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFeatures_FeatureDetails_FeatureDetailId",
                        column: x => x.FeatureDetailId,
                        principalTable: "FeatureDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFeatures_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LikeImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ImageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikeImages_ThumbnailImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "ThumbnailImages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LikeImages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Features",
                columns: new[] { "Id", "IsCalculated", "IsSearchFeature", "Name", "WeightRate" },
                values: new object[,]
                {
                    { 2, true, true, "Tính cách", 0.94999999999999996 },
                    { 14, true, false, "Chơi game", 0.76000000000000001 },
                    { 13, true, false, "Cắm trại", 0.75 },
                    { 12, true, false, "Thiện nguyện", 0.64000000000000001 },
                    { 11, true, false, "Du lịch", 0.80000000000000004 },
                    { 10, true, false, "Thú cưng", 0.88 },
                    { 9, true, false, "Mua sắm", 0.87 },
                    { 8, true, false, "Cây cảnh", 0.64000000000000001 },
                    { 7, true, false, "Máy tính", 0.68000000000000005 },
                    { 6, true, false, "Đọc sách", 0.75 },
                    { 5, true, false, "Xem phim", 0.80000000000000004 },
                    { 4, true, true, "Lối sống", 0.82999999999999996 },
                    { 3, true, true, "Phong cách", 0.84999999999999998 },
                    { 1, true, true, "Dáng người", 0.90000000000000002 },
                    { 15, true, false, "Thể thao", 0.85999999999999999 }
                });

            migrationBuilder.InsertData(
                table: "SimilariryFeatures",
                columns: new[] { "Id", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2021, 7, 10, 18, 2, 28, 824, DateTimeKind.Local).AddTicks(9217) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarPath", "CreatedAt", "Dob", "Email", "FindAgeGroup", "FindPeople", "FullName", "Gender", "Height", "IsInfoUpdated", "IsUpdatePosition", "Job", "Latitude", "Location", "Longitude", "PassWord", "PhoneNumber", "Role", "Status", "Summary", "Title", "UserName", "Weight" },
                values: new object[,]
                {
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c969"), "han.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "han@gmail.com", 0, 1, "Gia Hân", 0, 170, true, false, 18, 0.0, 37, 0.0, "1111", "0396925225", 0, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "Han", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c968"), "diem.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "diem@gmail.com", 0, 1, "Kiều Diễm", 0, 170, true, false, 22, 0.0, 37, 0.0, "1111", "0396925225", 0, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "Diem", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c966"), "nhung.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "nhung1@gmail.com", 0, 1, "Gia Nhung", 0, 170, true, false, 6, 0.0, 37, 0.0, "1111", "0396925225", 0, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "GiaNhung", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c964"), "duc.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "duc@gmail.com", 0, 0, "Trí Đức", 1, 170, true, false, 27, 0.0, 37, 0.0, "1111", "0396925225", 0, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "Duc", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c963"), "son.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "son@gmail.com", 0, 0, "Phan Sơn", 1, 170, true, false, 8, 0.0, 37, 0.0, "1111", "0396925225", 0, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "Son", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c962"), "dat.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "dat@gmail.com", 0, 0, "Hồ Quốc Đạt", 1, 170, true, false, 13, 0.0, 37, 0.0, "1111", "0396925225", 0, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "Dat", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c961"), "dinh.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "dinh@gmail.com", 0, 0, "Lê Kim Đỉnh", 1, 170, true, false, 12, 0.0, 37, 0.0, "1111", "0396925225", 0, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "Dinh", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c965"), "tien.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "tien@gmail.com", 0, 0, "Lê Minh Tiến", 1, 170, true, false, 10, 0.0, 37, 0.0, "1111", "0396925225", 0, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "tien", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c96f"), "hieu.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "hieu@gmail.com", 0, 0, "Võ Minh Hiếu", 1, 170, true, false, 4, 0.0, 37, 0.0, "1111", "0396925225", 0, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "hieu", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c96e"), "vuong.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "vuong@gmail.com", 0, 0, "Nguyên Vương", 1, 170, true, false, 1, 0.0, 38, 0.0, "1111", "0396925225", 0, 0, "Tôi là Vương, rất vui khi được làm quen với bạn", "Thông tin của tôi", "vuong", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c967"), "nhung2.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "nhung@gmail.com", 0, 1, "Nguyễn Huyền Nhung", 0, 170, true, false, 21, 0.0, 37, 0.0, "1111", "0369875463", 1, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "nhung2", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c977"), "mai.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "mai@gmail.com", 0, 1, "Xuân Maiii", 0, 170, true, false, 24, 0.0, 37, 0.0, "1111", "0396925225", 0, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "Mai", 65 },
                    { new Guid("ec826af8-0310-48cf-8a14-da11bdb1c96d"), "tam.jpg", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "tam@gmail.com", 0, 0, "Nguyễn Thành Tâm", 1, 170, true, false, 7, 0.0, 37, 0.0, "admin", "0396925225", 0, 0, "Tôi là Tâm, rất vui khi được làm quen với bạn", "Thông tin của tôi", "Admin", 65 }
                });

            migrationBuilder.InsertData(
                table: "FeatureDetails",
                columns: new[] { "Id", "Content", "FeatureId", "Weight" },
                values: new object[,]
                {
                    { 1, "Nhỏ nhắn", 1, -2 },
                    { 38, "Rất thích", 7, 2 },
                    { 39, "Không thích", 8, -2 },
                    { 40, "Bình thường", 8, -1 },
                    { 41, "Thích", 8, 1 },
                    { 42, "Rất thích", 8, 2 },
                    { 43, "Không thích", 9, -2 },
                    { 44, "Bình thường", 9, -1 },
                    { 45, "Thích", 9, 1 },
                    { 46, "Rất thích", 9, 2 },
                    { 47, "Không thích", 10, -2 },
                    { 48, "Bình thường", 10, -1 },
                    { 49, "Thích", 10, 1 },
                    { 50, "Rất thích", 10, 2 },
                    { 51, "Không thích", 11, -2 },
                    { 52, "Bình thường", 11, -1 },
                    { 53, "Thích", 11, 1 },
                    { 54, "Rất thích", 11, 2 },
                    { 68, "Bình thường", 15, -1 },
                    { 67, "Không thích", 15, -2 },
                    { 66, "Rất thích", 14, 2 },
                    { 65, "Thích", 14, 1 },
                    { 64, "Bình thường", 14, -1 },
                    { 63, "Không thích", 14, -2 },
                    { 37, "Thích", 7, 1 },
                    { 62, "Rất thích", 13, 2 },
                    { 60, "Bình thường", 13, -1 },
                    { 59, "Không thích", 13, -2 },
                    { 58, "Rất thích", 12, 2 },
                    { 57, "Thích", 12, 1 },
                    { 56, "Bình thường", 12, -1 },
                    { 55, "Không thích", 12, -2 },
                    { 61, "Thích", 13, 1 },
                    { 36, "Bình thường", 7, -1 },
                    { 35, "Không thích", 7, -2 },
                    { 34, "Rất thích", 6, 2 },
                    { 15, "Thành đạt", 3, -3 },
                    { 14, "Sang trọng", 3, -4 },
                    { 13, "Ngốc nghếch", 2, 4 },
                    { 12, "Vui vẻ", 2, 3 },
                    { 11, "Lịch sự", 2, 2 },
                    { 10, "Tự tin", 2, 1 },
                    { 16, "Quyến rũ", 3, -2 },
                    { 9, "Kín đáo", 2, -1 },
                    { 7, "Lạnh lùng", 2, -3 },
                    { 6, "Vạm vỡ", 1, 6 },
                    { 5, "Cao lớn", 1, 3 },
                    { 4, "Mũm mĩm", 1, 2 },
                    { 3, "Cân đối", 1, 1 },
                    { 2, "Mảnh mai", 1, -1 },
                    { 8, "Thật thà", 2, -2 },
                    { 69, "Thích", 15, 1 },
                    { 17, "Thể thao", 3, -1 },
                    { 19, "Sành điệu", 3, 2 },
                    { 33, "Thích", 6, 1 },
                    { 32, "Bình thường", 6, -1 },
                    { 31, "Không thích", 6, -2 },
                    { 30, "Rất thích", 5, 2 },
                    { 29, "Thích", 5, 1 },
                    { 28, "Bình thường", 5, -1 },
                    { 18, "Thời trang", 3, 1 },
                    { 27, "Không thích", 5, -2 },
                    { 25, "Hướng nội", 4, 2 },
                    { 24, "Lạc quan", 4, 1 },
                    { 23, "Tự do", 4, -1 },
                    { 22, "Độc lập", 4, -2 },
                    { 21, "Tối giản", 3, 4 },
                    { 20, "Giản dị", 3, 3 },
                    { 26, "Khuôn phép", 4, 5 },
                    { 70, "Rất thích", 15, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockUsers_FromUserId",
                table: "BlockUsers",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockUsers_ToUserId",
                table: "BlockUsers",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_FromUserId",
                table: "Favorites",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ToUserId",
                table: "Favorites",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureDetails_FeatureId",
                table: "FeatureDetails",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FromUserId",
                table: "Follows",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_ToUserId",
                table: "Follows",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HaveMessages_ReceiverId",
                table: "HaveMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_HaveMessages_SenderId",
                table: "HaveMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeImages_ImageId",
                table: "LikeImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeImages_UserId",
                table: "LikeImages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_FromId",
                table: "Relationships",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_ToId",
                table: "Relationships",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchFeatures_FeatureDetailId",
                table: "SearchFeatures",
                column: "FeatureDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchFeatures_FeatureId",
                table: "SearchFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchFeatures_UserId",
                table: "SearchFeatures",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarityScores_FromUserId",
                table: "SimilarityScores",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ThumbnailImages_UserId",
                table: "ThumbnailImages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFeatures_FeatureDetailId",
                table: "UserFeatures",
                column: "FeatureDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFeatures_FeatureId",
                table: "UserFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFeatures_UserId",
                table: "UserFeatures",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accesses");

            migrationBuilder.DropTable(
                name: "BlockUsers");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Follows");

            migrationBuilder.DropTable(
                name: "HaveMessages");

            migrationBuilder.DropTable(
                name: "ImageScores");

            migrationBuilder.DropTable(
                name: "LikeImages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Relationships");

            migrationBuilder.DropTable(
                name: "SearchFeatures");

            migrationBuilder.DropTable(
                name: "SimilariryFeatures");

            migrationBuilder.DropTable(
                name: "SimilarityScores");

            migrationBuilder.DropTable(
                name: "UserFeatures");

            migrationBuilder.DropTable(
                name: "ThumbnailImages");

            migrationBuilder.DropTable(
                name: "FeatureDetails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Features");
        }
    }
}
