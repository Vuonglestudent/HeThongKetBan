using MakeFriendSolution.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.EF.Configurations
{
    public class SearchFeatureConfig : IEntityTypeConfiguration<SearchFeature>
    {
        public void Configure(EntityTypeBuilder<SearchFeature> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.User).WithMany(x => x.SearchFeatures)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.FeatureDetail).WithMany(x => x.SearchFeatures)
                .HasForeignKey(x => x.FeatureDetailId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Feature).WithMany(x => x.SearchFeatures)
                .HasForeignKey(x => x.FeatureId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
