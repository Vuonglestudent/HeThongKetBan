using MakeFriendSolution.Models;
using MakeFriendSolution.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.EF.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(50).IsUnicode(false);
            builder.Property(x => x.PassWord).IsRequired().IsUnicode(false).HasMaxLength(200);
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(200);
            builder.Property(x => x.IsInfoUpdated).HasDefaultValue(0);
            builder.Property(x => x.NumberOfPasswordConfirmations).HasDefaultValue(0);
            builder.Property(x => x.PasswordForgottenPeriod).HasDefaultValue(new DateTime(2000, 1, 1));
            builder.Property(x => x.PasswordForgottenCode).HasDefaultValue("");
            builder.Property(x => x.TypeAccount).HasDefaultValue(ETypeAccount.System);
            builder.Property(x => x.NumberOfFiends).HasDefaultValue(0);
            builder.Property(x => x.NumberOfImages).HasDefaultValue(0);
            builder.Property(x => x.NumberOfLikes).HasDefaultValue(0);
            builder.Property(x => x.UpdatedAt).HasDefaultValue(DateTime.Now);

            builder.HasMany(x => x.SendMessages).WithOne(a => a.Sender).HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.ReceiveMessages).WithOne(a => a.Receiver).HasForeignKey(x => x.ReceiverId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.BeingFollowedBy).WithOne(a => a.ToUser).HasForeignKey(x => x.ToUserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.Followed).WithOne(a => a.FromUser).HasForeignKey(x => x.FromUserId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.BeingFavoritedBy).WithOne(a => a.ToUser).HasForeignKey(x => x.ToUserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.Favorited).WithOne(a => a.FromUser).HasForeignKey(x => x.FromUserId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.BeingFavoritedBy).WithOne(a => a.ToUser).HasForeignKey(x => x.ToUserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.BlockedByUsers).WithOne(a => a.FromUser).HasForeignKey(x => x.FromUserId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}