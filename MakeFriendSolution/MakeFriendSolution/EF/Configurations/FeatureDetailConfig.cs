using MakeFriendSolution.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MakeFriendSolution.EF.Configurations
{
    public class FeatureDetailConfig : IEntityTypeConfiguration<FeatureDetail>
    {
        public void Configure(EntityTypeBuilder<FeatureDetail> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Weight).HasDefaultValue(1);

            builder.HasOne(x => x.Feature).WithMany(x => x.FeatureDetails).HasForeignKey(x => x.FeatureId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}