using MakeFriendSolution.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace MakeFriendSolution.EF.Configurations
{
    public class SimilarityFeatureConfig : IEntityTypeConfiguration<SimilarityFeature>
    {
        public void Configure(EntityTypeBuilder<SimilarityFeature> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UpdatedAt).HasDefaultValue(DateTime.Now);
        }
    }
}
