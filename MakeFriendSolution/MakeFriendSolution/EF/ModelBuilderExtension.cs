using MakeFriendSolution.Models;
using MakeFriendSolution.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 2,
                    IsCalculated = true,
                    
                    IsSearchFeature = true,
                    Name = "Học vấn",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 3,
                    IsCalculated = true,
                    
                    Name = "Phong cách sống",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 5,
                    IsCalculated = true,
                    
                    Name = "Tôn giáo",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 6,
                    IsCalculated = true,
                    
                    Name = "Thể loại phim ưa thích",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 7,
                    IsCalculated = true,
                    
                    Name = "Nhạc ưa thích",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 8,
                    IsCalculated = true,
                    
                    Name = "Bầu không khí ưa thích",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 9,
                    IsCalculated = true,
                    
                    Name = "Đi mua sắm",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 10,
                    IsCalculated = true,
                    
                    Name = "Đi du lịch",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 11,
                    IsCalculated = true,
                    
                    Name = "Chơi game",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 12,
                    IsCalculated = true,
                    
                    Name = "Nấu ăn",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 13,
                    IsCalculated = true,
                    
                    Name = "Công nghệ",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 14,
                    IsCalculated = true,
                    
                    Name = "Thú cưng",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 15,
                    IsCalculated = true,
                    
                    Name = "Chơi thể thao",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 16,
                    IsCalculated = true,
                    
                    Name = "Hút thuốc",
                    WeightRate = 1
                },
                new Feature()
                {
                    Id = 17,
                    IsCalculated = true,
                    
                    Name = "Uống rượu bia",
                    WeightRate = 1
                }
            );

            builder.Entity<FeatureDetail>().HasData(
                //Dáng người Id = 1, max = 6
                new FeatureDetail()
                {
                    Id = 1,
                    FeatureId = 1,
                    Content = "Nhỏ nhắn",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 2,
                    FeatureId = 1,
                    Content = "Mảnh mai",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 3,
                    FeatureId = 1,
                    Content = "Cân đối",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 4,
                    FeatureId = 1,
                    Content = "Mũm mĩm",
                    Weight = 4
                },
                new FeatureDetail()
                {
                    Id = 5,
                    FeatureId = 1,
                    Content = "Cao lớn",
                    Weight = 5
                },
                new FeatureDetail()
                {
                    Id = 6,
                    FeatureId = 1,
                    Content = "Vạm vỡ",
                    Weight = 6
                },
                //Học vấn Id = 2, max = 12
                new FeatureDetail()
                {
                    Id = 7,
                    FeatureId = 2,
                    Content = "Phổ thông",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 8,
                    FeatureId = 2,
                    Content = "Trung cấp",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 9,
                    FeatureId = 2,
                    Content = "Cao đẳng",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 10,
                    FeatureId = 2,
                    Content = "Đại học",
                    Weight = 4
                },
                new FeatureDetail()
                {
                    Id = 11,
                    FeatureId = 2,
                    Content = "Cao học",
                    Weight = 5
                },
                new FeatureDetail()
                {
                    Id = 12,
                    FeatureId = 2,
                    Content = "Trên cao học",
                    Weight = 6
                }
                //Cách sống Id = 3, max = 19
                ,
                new FeatureDetail()
                {
                    Id = 13,
                    FeatureId = 3,
                    Content = "An nhàn",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 14,
                    FeatureId = 3,
                    Content = "Giản dị",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 15,
                    FeatureId = 3,
                    Content = "Lạc quan",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 16,
                    FeatureId = 3,
                    Content = "Lành mạnh",
                    Weight = 4
                },
                new FeatureDetail()
                {
                    Id = 17,
                    FeatureId = 3,
                    Content = "Năng động",
                    Weight = 5
                },
                new FeatureDetail()
                {
                    Id = 18,
                    FeatureId = 3,
                    Content = "Tình cảm",
                    Weight = 6
                },
                new FeatureDetail()
                {
                    Id = 19,
                    FeatureId = 3,
                    Content = "Tự do",
                    Weight = 7
                }
                //Tôn giáo Id = 5, max = 29
                ,
                new FeatureDetail()
                {
                    Id = 27,
                    FeatureId = 5,
                    Content = "Không có đạo",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 28,
                    FeatureId = 5,
                    Content = "Thiên Chúa giáo",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 29,
                    FeatureId = 5,
                    Content = "Phật giáo",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 100,
                    FeatureId = 5,
                    Content = "Tin Lành",
                    Weight = 4
                },
                new FeatureDetail()
                {
                    Id = 101,
                    FeatureId = 5,
                    Content = "Đạo khác",
                    Weight = 5
                }
                //Thể loại phim Id = 6, max = 38
                ,
                new FeatureDetail()
                {
                    Id = 30,
                    FeatureId = 6,
                    Content = "Hành động",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 31,
                    FeatureId = 6,
                    Content = "Khoa học viễn tưởng",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 32,
                    FeatureId = 6,
                    Content = "Chiến tranh",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 33,
                    FeatureId = 6,
                    Content = "Chiến tranh",
                    Weight = 4
                },
                new FeatureDetail()
                {
                    Id = 34,
                    FeatureId = 6,
                    Content = "Cổ trang",
                    Weight = 5
                },
                new FeatureDetail()
                {
                    Id = 35,
                    FeatureId = 6,
                    Content = "Hài hước",
                    Weight = 6
                },
                new FeatureDetail()
                {
                    Id = 36,
                    FeatureId = 6,
                    Content = "Kinh dị",
                    Weight = 7
                },
                new FeatureDetail()
                {
                    Id = 37,
                    FeatureId = 6,
                    Content = "Lãng mạn",
                    Weight = 8
                },
                new FeatureDetail()
                {
                    Id = 38,
                    FeatureId = 6,
                    Content = "Hoạt hình",
                    Weight = 9
                }
                //Nhạc yêu thích Id = 7, max = 43
                ,
                new FeatureDetail()
                {
                    Id = 39,
                    FeatureId = 7,
                    Content = "Nhạc trẻ",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 40,
                    FeatureId = 7,
                    Content = "Pop",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 41,
                    FeatureId = 7,
                    Content = "Dance",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 42,
                    FeatureId = 7,
                    Content = "Rap - Hip hop",
                    Weight = 4
                },
                new FeatureDetail()
                {
                    Id = 43,
                    FeatureId = 7,
                    Content = "Nhạc Bolero",
                    Weight = 5
                }
                //Bầu không khí Id = 8, max = 48
                ,
                new FeatureDetail()
                {
                    Id = 44,
                    FeatureId = 8,
                    Content = "Tĩnh lặng",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 45,
                    FeatureId = 8,
                    Content = "Êm đềm",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 46,
                    FeatureId = 8,
                    Content = "Bình yên",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 47,
                    FeatureId = 8,
                    Content = "Vui tươi",
                    Weight = 4
                },
                new FeatureDetail()
                {
                    Id = 48,
                    FeatureId = 8,
                    Content = "Náo nhiệt",
                    Weight = 5
                }
                //Đi mua sắm Id = 9, max = 51
                ,
                new FeatureDetail()
                {
                    Id = 49,
                    FeatureId = 9,
                    Content = "Ít khi đi",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 50,
                    FeatureId = 9,
                    Content = "Thỉnh thoảng",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 51,
                    FeatureId = 9,
                    Content = "Thường xuyên",
                    Weight = 3
                }
                //Đi du lịch Id = 10, max = 54
                ,
                new FeatureDetail()
                {
                    Id = 52,
                    FeatureId = 10,
                    Content = "Ít khi đi",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 53,
                    FeatureId = 10,
                    Content = "Thỉnh thoảng",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 54,
                    FeatureId = 10,
                    Content = "Thường xuyên",
                    Weight = 3
                }
                //Chơi game Id = 11, max = 59
                ,
                new FeatureDetail()
                {
                    Id = 56,
                    FeatureId = 11,
                    Content = "Không chơi game",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 57,
                    FeatureId = 11,
                    Content = "Thỉnh thoảng",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 58,
                    FeatureId = 11,
                    Content = "Thường xuyên",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 59,
                    FeatureId = 11,
                    Content = "Nghiện game",
                    Weight = 4
                }
                // Nấu ăn Id = 12, max = 63
                ,
                new FeatureDetail()
                {
                    Id = 60,
                    FeatureId = 12,
                    Content = "Không nấu ăn",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 61,
                    FeatureId = 12,
                    Content = "Ít nấu ăn",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 62,
                    FeatureId = 12,
                    Content = "Thỉnh thoảng",
                    Weight = 3
                },
                new FeatureDetail()
                {
                    Id = 63,
                    FeatureId = 12,
                    Content = "Thường xuyên",
                    Weight = 4
                }
                //Công nghệ Id = 13, max = 66
                ,
                new FeatureDetail()
                {
                    Id = 64,
                    FeatureId = 13,
                    Content = "Bình thường",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 65,
                    FeatureId = 13,
                    Content = "Chỉ theo dõi",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 66,
                    FeatureId = 13,
                    Content = "Tính đồ công nghệ",
                    Weight = 3
                }
                //Thú cưng Id = 14, max = 69
                ,
                new FeatureDetail()
                {
                    Id = 67,
                    FeatureId = 14,
                    Content = "Không thích",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 68,
                    FeatureId = 14,
                    Content = "Nuôi cho vui",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 69,
                    FeatureId = 14,
                    Content = "Thích thú cưng",
                    Weight = 3
                }
                // Chơi thể thao Id = 15, max = 72
                ,
                new FeatureDetail()
                {
                    Id = 70,
                    FeatureId = 15,
                    Content = "Ít khi chơi",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 71,
                    FeatureId = 15,
                    Content = "Thỉnh thoảng",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 72,
                    FeatureId = 15,
                    Content = "Thường xuyên",
                    Weight = 3
                }
                //Hút thuốc Id = 16, max = 75
                ,
                new FeatureDetail()
                {
                    Id = 73,
                    FeatureId = 16,
                    Content = "Không hút thuốc",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 74,
                    FeatureId = 16,
                    Content = "Hút xã giao",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 75,
                    FeatureId = 16,
                    Content = "Hút nhiều",
                    Weight = 3
                }
                //Uống rượu bia Id = 17, max = 78
                ,
                new FeatureDetail()
                {
                    Id = 76,
                    FeatureId = 17,
                    Content = "Không uống",
                    Weight = 1
                },
                new FeatureDetail()
                {
                    Id = 77,
                    FeatureId = 17,
                    Content = "Uống xã giao",
                    Weight = 2
                },
                new FeatureDetail()
                {
                    Id = 78,
                    FeatureId = 17,
                    Content = "Uống nhiều",
                    Weight = 3
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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,

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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,
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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,
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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,

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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,
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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,
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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,
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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,
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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,
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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,
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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,
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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,
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
                    Job = EJob.IT_Công_Nghệ_Thông_Tin,
                    Summary = "Tôi là Tâm, rất vui khi được làm quen với bạn",
                    Title = "Thông tin của tôi",
                    IsInfoUpdated = true
                }
            );
        }
    }
}