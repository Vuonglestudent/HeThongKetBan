using MakeFriendSolution.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.EF.Configurations
{
    public class SimilarityScoreConfog : IEntityTypeConfiguration<SimilarityScore>
    {
        public void Configure(EntityTypeBuilder<SimilarityScore> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Score).HasDefaultValue(0);

            builder.HasOne(x => x.FromUser)
                .WithMany(x => x.SimilarityScores)
                .HasForeignKey(x => x.FromUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.ToUserId).IsRequired();
        }
    }
}
