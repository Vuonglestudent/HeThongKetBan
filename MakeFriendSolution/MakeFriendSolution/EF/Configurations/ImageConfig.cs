using MakeFriendSolution.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.EF.Configurations
{
    public class ImageConfig : IEntityTypeConfiguration<ThumbnailImage>
    {
        public void Configure(EntityTypeBuilder<ThumbnailImage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.ImagePath).IsRequired();
            builder.Property(x => x.Title).IsRequired().HasDefaultValue("Image title");
            builder.HasOne(x => x.User).WithMany(x => x.ThumbnailImages).HasForeignKey(x => x.UserId);
            builder.Property(x => x.NumberOflikes).HasDefaultValue(0);
        }
    }
}