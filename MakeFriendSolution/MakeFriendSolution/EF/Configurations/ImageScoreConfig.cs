using MakeFriendSolution.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.EF.Configurations
{
    public class ImageScoreConfig : IEntityTypeConfiguration<ImageScore>
    {
        public void Configure(EntityTypeBuilder<ImageScore> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Neutral).HasDefaultValue(0);
            builder.Property(x => x.Sexy).HasDefaultValue(0);
            builder.Property(x => x.Porn).HasDefaultValue(0);
            builder.Property(x => x.Hentai).HasDefaultValue(0);
            builder.Property(x => x.Drawings).HasDefaultValue(0);
        }
    }
}
