using MakeFriendSolution.EF.Configurations;
using MakeFriendSolution.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeFriendSolution.EF
{
    public class MakeFriendDbContext : DbContext
    {
        public MakeFriendDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HaveMessageConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new ImageConfig());
            modelBuilder.ApplyConfiguration(new LikeImageConfig());
            modelBuilder.ApplyConfiguration(new FollowConfig());
            modelBuilder.ApplyConfiguration(new FavoriteConfig());
            modelBuilder.ApplyConfiguration(new BlockUserConfig());
            modelBuilder.ApplyConfiguration(new AccessConfig());
            modelBuilder.ApplyConfiguration(new SimilarityFeatureConfig());
            modelBuilder.ApplyConfiguration(new SimilarityScoreConfog());
            modelBuilder.ApplyConfiguration(new SearchFeatureConfig());

            modelBuilder.ApplyConfiguration(new FeatureConfig());
            modelBuilder.ApplyConfiguration(new FeatureDetailConfig());
            modelBuilder.ApplyConfiguration(new ImageScoreConfig());
            modelBuilder.ApplyConfiguration(new NotificationConfig());
            modelBuilder.Seed();
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<ThumbnailImage> ThumbnailImages { get; set; }
        public DbSet<HaveMessage> HaveMessages { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<LikeImage> LikeImages { get; set; }
        public DbSet<BlockUser> BlockUsers { get; set; }
        public DbSet<Access> Accesses { get; set; }
        public DbSet<SimilarityFeature> SimilariryFeatures { get; set; }
        public DbSet<SimilarityScore> SimilarityScores { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureDetail> FeatureDetails { get; set; }
        public DbSet<UserFeature> UserFeatures { get; set; }
        public DbSet<SearchFeature> SearchFeatures { get; set; }
        public DbSet<ImageScore> ImageScores { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}