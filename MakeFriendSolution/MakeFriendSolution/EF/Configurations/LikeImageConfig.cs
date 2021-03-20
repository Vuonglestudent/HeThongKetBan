using MakeFriendSolution.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeFriendSolution.EF.Configurations
{
    public class LikeImageConfig : IEntityTypeConfiguration<LikeImage>
    {
        public void Configure(EntityTypeBuilder<LikeImage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.Image).WithMany(x => x.LikeImages).HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}