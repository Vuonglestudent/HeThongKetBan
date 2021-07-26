using MakeFriendSolution.Models;
using MakeFriendSolution.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System;

namespace MakeFriendSolution.EF
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            var adminId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C96D");
            var vuongId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C96E");
            var hieuId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C96F");
            var dinhId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C961");
            var datId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C962");
            var sonId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C963");
            var ducId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C964");
            var tienId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C965");
            var nhungId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C966");
            var nhung2Id = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C967");
            var diemId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C968");
            var hanId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C969");
            var maiId = new Guid("EC826AF8-0310-48CF-8A14-DA11BDB1C977");
            builder.Entity<SimilarityFeature>().HasData(
                new SimilarityFeature()
                {
                    UpdatedAt = DateTime.Now,
                    Id = 1
                }
            );

            builder.Entity<Feature>().HasData(
                new Feature()
                {
                    Id = 1,
                    IsCalculated = true,
                    IsSearchFeature = true,
                    Name = "Dáng người",
                    WeightRate = 0.9
                },
                new Feature()
                {
                    Id = 2,
                    IsCalculated = true,
                    IsSearchFeature = true,
                    Name = "Tính cách",
                    WeightRate = 0.95
                },
                new Feature()
                {
                    Id = 3,
                    IsCalculated = true,
                    IsSearchFeature = true,
                    Name = "Phong cách",
                    WeightRate = 0.85
                },
                new Feature()
                {
                    Id = 4,
                    IsCalculated = true,
                    IsSearchFeature = true,
                    Name = "Lối sống",
                    WeightRate = 0.83
                },
                new Feature()
                {
                    Id = 5,
                    IsCalculated = true,
                    IsSearchFeature = false,
                    Name = "Xem phim",
                    WeightRate = 0.8
                },
                new Feature()
                {
                    Id = 6,
                    IsCalculated = true,
                    IsSearchFeature = false,
                    Name = "Đọc sách",
                    WeightRate = 0.75
                },
                new Feature()
                {
                    Id = 7,
                    IsCalculated = true,
                    IsSearchFeature = false,
                    Name = "Máy tính",
                    WeightRate = 0.68
                },
                new Feature()
                {
                    Id = 8,
                    IsCalculated = true,
                    IsSearchFeature = false,
                    Name = "Cây cảnh",
                    WeightRate = 0.64
                },
                new Feature()
                {
                    Id = 9,
                    IsCalculated = true,
                    IsSearchFeature = false,
                    Name = "Mua sắm",
                    WeightRate = 0.87
                },
                new Feature()
                {
                    Id = 10,
                    IsCalculated = true,
                    IsSearchFeature = false,
                    Name = "Thú cưng",
                    WeightRate = 0.88
                },
                new Feature()
                {
                    Id = 11,
                    IsCalculated = true,
                    IsSearchFeature = false,
                    Name = "Du lịch",
                    WeightRate = 0.8
                },
                new Feature()
                {
                    Id = 12,
                    IsCalculated = true,
                    IsSearchFeature = false,
                    Name = "Thiện nguyện",
                    WeightRate = 0.64
                },
                new Feature()
                {
                    Id = 13,
                    IsCalculated = true,
                    IsSearchFeature = false,
                    Name = "Cắm trại",
                    WeightRate = 0.75
                },
                new Feature()
                {
                    Id = 14,
                    IsCalculated = true,
                    IsSearchFeature = false,
                    Name = "Chơi game",
                    WeightRate = 0.76
                },
                new Feature()
                {
                    Id = 15,
                    IsCalculated = true,
                    IsSearchFeature = false,
                    Name = "Thể thao",
                    WeightRate = 0.86
                }
            );

            builder.Entity<FeatureDetail>().HasData(
                //Dáng người Id = 1, max = 5
                new FeatureDetail()
                {
                    Id = 1,
                    FeatureId = 1,
                    Content = "Nhỏ nhắn",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 2,
                    FeatureId = 1,
                    Content = "Mảnh mai",
                    Weight = -1
                },
                new FeatureDetail()
                {
                    Id = 3,
                    FeatureId = 1,
                    Content = "Cân đối",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 4,
                    FeatureId = 1,
                    Content = "Mũm mĩm",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 5,
                    FeatureId = 1,
                    Content = "Cao lớn",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 6,
                    FeatureId = 1,
                    Content = "Vạm vỡ",
                    Weight = 6
                },
                //Tính cách Id = 2, max = 12
                new FeatureDetail()
                {
                    Id = 7,
                    FeatureId = 2,
                    Content = "Lạnh lùng",
                    Weight = -3
                },
                new FeatureDetail()
                {
                    Id = 8,
                    FeatureId = 2,
                    Content = "Thật thà",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 9,
                    FeatureId = 2,
                    Content = "Kín đáo",
                    Weight = -1
                },
                new FeatureDetail()
                {
                    Id = 10,
                    FeatureId = 2,
                    Content = "Tự tin",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 11,
                    FeatureId = 2,
                    Content = "Lịch sự",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 12,
                    FeatureId = 2,
                    Content = "Vui vẻ",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 13,
                    FeatureId = 2,
                    Content = "Ngốc nghếch",
                    Weight = 4
                }
                //Phong cách Id = 3, max = 20
                ,
                new FeatureDetail()
                {
                    Id = 14,
                    FeatureId = 3,
                    Content = "Sang trọng",
                    Weight = -4
                },
                new FeatureDetail()
                {
                    Id = 15,
                    FeatureId = 3,
                    Content = "Thành đạt",
                    Weight = -3
                },
                new FeatureDetail()
                {
                    Id = 16,
                    FeatureId = 3,
                    Content = "Quyến rũ",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 17,
                    FeatureId = 3,
                    Content = "Thể thao",
                    Weight = -1
                },
                new FeatureDetail()
                {
                    Id = 18,
                    FeatureId = 3,
                    Content = "Thời trang",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 19,
                    FeatureId = 3,
                    Content = "Sành điệu",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 20,
                    FeatureId = 3,
                    Content = "Giản dị",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 21,
                    FeatureId = 3,
                    Content = "Tối giản",
                    Weight = 4
                }

                //Lối sống Id = 4, max = 25
                ,
                new FeatureDetail()
                {
                    Id = 22,
                    FeatureId = 4,
                    Content = "Độc lập",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 23,
                    FeatureId = 4,
                    Content = "Tự do",
                    Weight = -1
                },
                new FeatureDetail()
                {
                    Id = 24,
                    FeatureId = 4,
                    Content = "Lạc quan",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 25,
                    FeatureId = 4,
                    Content = "Hướng nội",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 26,
                    FeatureId = 4,
                    Content = "Khuôn phép",
                    Weight = 5
                }
                //Đọc sách Id = 5, max = 29
                ,
                new FeatureDetail()
                {
                    Id = 27,
                    FeatureId = 5,
                    Content = "Không thích",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 28,
                    FeatureId = 5,
                    Content = "Bình thường",
                    Weight = -1, 
                },
                new FeatureDetail()
                {
                    Id = 29,
                    FeatureId = 5,
                    Content = "Thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 30,
                    FeatureId = 5,
                    Content = "Rất thích",
                    Weight = 2
                }
                //Xem phim Id = 6, max = 33
                ,
                new FeatureDetail()
                {
                    Id = 31,
                    FeatureId = 6,
                    Content = "Không thích",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 32,
                    FeatureId = 6,
                    Content = "Bình thường",
                    Weight = -1,
                },
                new FeatureDetail()
                {
                    Id = 33,
                    FeatureId = 6,
                    Content = "Thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 34,
                    FeatureId = 6,
                    Content = "Rất thích",
                    Weight = 2
                }
                //Máy tính Id = 7, max = 37
                ,
                new FeatureDetail()
                {
                    Id = 35,
                    FeatureId = 7,
                    Content = "Không thích",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 36,
                    FeatureId = 7,
                    Content = "Bình thường",
                    Weight = -1,
                },
                new FeatureDetail()
                {
                    Id = 37,
                    FeatureId = 7,
                    Content = "Thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 38,
                    FeatureId = 7,
                    Content = "Rất thích",
                    Weight = 2
                }
                //Cây cảnh Id = 8, max = 41
                ,
                new FeatureDetail()
                {
                    Id = 39,
                    FeatureId = 8,
                    Content = "Không thích",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 40,
                    FeatureId = 8,
                    Content = "Bình thường",
                    Weight = -1,
                },
                new FeatureDetail()
                {
                    Id = 41,
                    FeatureId = 8,
                    Content = "Thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 42,
                    FeatureId = 8,
                    Content = "Rất thích",
                    Weight = 2
                }
                //Mua sắm Id = 9, max = 45
                ,
                new FeatureDetail()
                {
                    Id = 43,
                    FeatureId = 9,
                    Content = "Không thích",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 44,
                    FeatureId = 9,
                    Content = "Bình thường",
                    Weight = -1,
                },
                new FeatureDetail()
                {
                    Id = 45,
                    FeatureId = 9,
                    Content = "Thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 46,
                    FeatureId = 9,
                    Content = "Rất thích",
                    Weight = 2
                }
                //Thú cưng Id = 10, max = 49
                ,
                new FeatureDetail()
                {
                    Id = 47,
                    FeatureId = 10,
                    Content = "Không thích",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 48,
                    FeatureId = 10,
                    Content = "Bình thường",
                    Weight = -1,
                },
                new FeatureDetail()
                {
                    Id = 49,
                    FeatureId = 10,
                    Content = "Thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 50,
                    FeatureId = 10,
                    Content = "Rất thích",
                    Weight = 2
                }
                //Du lịch Id = 11, max = 53
                ,
                new FeatureDetail()
                {
                    Id = 51,
                    FeatureId = 11,
                    Content = "Không thích",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 52,
                    FeatureId = 11,
                    Content = "Bình thường",
                    Weight = -1,
                },
                new FeatureDetail()
                {
                    Id = 53,
                    FeatureId = 11,
                    Content = "Thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 54,
                    FeatureId = 11,
                    Content = "Rất thích",
                    Weight = 2
                }
                //Du lịch Id = 12, max = 57
                ,
                new FeatureDetail()
                {
                    Id = 55,
                    FeatureId = 12,
                    Content = "Không thích",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 56,
                    FeatureId = 12,
                    Content = "Bình thường",
                    Weight = -1,
                },
                new FeatureDetail()
                {
                    Id = 57,
                    FeatureId = 12,
                    Content = "Thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 58,
                    FeatureId = 12,
                    Content = "Rất thích",
                    Weight = 2
                }
                //Du lịch Id = 13, max = 61
                ,
                new FeatureDetail()
                {
                    Id = 59,
                    FeatureId = 13,
                    Content = "Không thích",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 60,
                    FeatureId = 13,
                    Content = "Bình thường",
                    Weight = -1,
                },
                new FeatureDetail()
                {
                    Id = 61,
                    FeatureId = 13,
                    Content = "Thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 62,
                    FeatureId = 13,
                    Content = "Rất thích",
                    Weight = 2
                }
                //Du lịch Id = 14, max = 65
                ,
                new FeatureDetail()
                {
                    Id = 63,
                    FeatureId = 14,
                    Content = "Không thích",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 64,
                    FeatureId = 14,
                    Content = "Bình thường",
                    Weight = -1,
                },
                new FeatureDetail()
                {
                    Id = 65,
                    FeatureId = 14,
                    Content = "Thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 66,
                    FeatureId = 14,
                    Content = "Rất thích",
                    Weight = 2
                }
                //Du lịch Id = 15, max = 69
                ,
                new FeatureDetail()
                {
                    Id = 67,
                    FeatureId = 15,
                    Content = "Không thích",
                    Weight = -2
                },
                new FeatureDetail()
                {
                    Id = 68,
                    FeatureId = 15,
                    Content = "Bình thường",
                    Weight = -1,
                },
                new FeatureDetail()
                {
                    Id = 69,
                    FeatureId = 15,
                    Content = "Thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 70,
                    FeatureId = 15,
                    Content = "Rất thích",
                    Weight = 2
                }
            );






































            builder.Entity<AppUser>().HasData(
                //Tam
                new AppUser()
                {
                    Id = adminId,
                    UserName = "Admin",
                    FullName = "Nguyễn Thành Tâm",
                    Email = "tam@gmail.com",
                    PassWord = "admin",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "tam.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nam,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nữ,
                    Job = EJob.Công_nghệ_thông_tin,

                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //Nhung2
                new AppUser()
                {
                    Id = nhung2Id,
                    UserName = "nhung2",
                    FullName = "Nguyễn Huyền Nhung",
                    Email = "nhung@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0369875463",
                    Role = ERole.User,
                    Status = EUserStatus.Active,
                    AvatarPath = "nhung2.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nữ,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,

                    FindPeople = EGender.Nam,
                    Job = EJob.Tài_chính_ngân_hàng,
                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //Vuong
                new AppUser()
                {
                    Id = vuongId,
                    UserName = "vuong",
                    FullName = "Nguyên Vương",
                    Email = "vuong@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "vuong.jpg",
                    Location = ELocation.Hà_Nội,
                    Gender = EGender.Nam,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nữ,
                    Job = EJob.Chủ_doanh_nghiệp,
                    Summary = "Tôi là Vương, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //hieu
                new AppUser()
                {
                    Id = hieuId,
                    UserName = "hieu",
                    FullName = "Võ Minh Hiếu",
                    Email = "hieu@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "hieu.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nam,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nữ,
                    Job = EJob.Giáo_viên_giảng_viên,

                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //Tien
                new AppUser()
                {
                    Id = tienId,
                    UserName = "tien",
                    FullName = "Lê Minh Tiến",
                    Email = "tien@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "tien.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nam,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nữ,
                    Job = EJob.Kỹ_sư,
                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //Dinh
                new AppUser()
                {
                    Id = dinhId,
                    UserName = "Dinh",
                    FullName = "Lê Kim Đỉnh",
                    Email = "dinh@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "dinh.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nam,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nữ,
                    Job = EJob.Lao_động_tự_do,
                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //Dat
                new AppUser()
                {
                    Id = datId,
                    UserName = "Dat",
                    FullName = "Hồ Quốc Đạt",
                    Email = "dat@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "dat.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nam,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nữ,
                    Job = EJob.Marketing,
                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //Son
                new AppUser()
                {
                    Id = sonId,
                    UserName = "Son",
                    FullName = "Phan Sơn",
                    Email = "son@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "son.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nam,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nữ,
                    Job = EJob.Máy_tính,
                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //Duc
                new AppUser()
                {
                    Id = ducId,
                    UserName = "Duc",
                    FullName = "Trí Đức",
                    Email = "duc@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "duc.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nam,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nữ,
                    Job = EJob.Nghề_nghiệp_khác,
                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //Nhung
                new AppUser()
                {
                    Id = nhungId,
                    UserName = "GiaNhung",
                    FullName = "Gia Nhung",
                    Email = "nhung1@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "nhung.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nữ,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nam,
                    Job = EJob.Nhà_hàng_khách_sạn,
                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //Diem
                new AppUser()
                {
                    Id = diemId,
                    UserName = "Diem",
                    FullName = "Kiều Diễm",
                    Email = "diem@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "diem.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nữ,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nam,
                    Job = EJob.Thiết_kế_tạo_mẫu,
                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //Han
                new AppUser()
                {
                    Id = hanId,
                    UserName = "Han",
                    FullName = "Gia Hân",
                    Email = "han@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "han.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nữ,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nam,
                    Job = EJob.Văn_phòng,
                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                },
                //Mai
                new AppUser()
                {
                    Id = maiId,
                    UserName = "Mai",
                    FullName = "Xuân Maiii",
                    Email = "mai@gmail.com",
                    PassWord = "1111",
                    PhoneNumber = "0396925225",
                    Role = ERole.Admin,
                    Status = EUserStatus.Active,
                    AvatarPath = "mai.jpg",
                    Location = ELocation.TP_HCM,
                    Gender = EGender.Nữ,
                    CreatedAt = new DateTime(2020, 09, 07),
                    Dob = new DateTime(1999, 01, 14),
                    Height = 170,
                    Weight = 65,
                    FindPeople = EGender.Nam,
                    Job = EJob.Vận_động_viên,
                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                }
            );
        }
    }
}