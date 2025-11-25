using Microsoft.EntityFrameworkCore;
using agriApp.Entities.Auth;
using agriApp.Entities.Stakeholders;
using agriApp.Entities.Market;

namespace agriApp.Data
{
    public class AgriDbContext : DbContext
    {
        public AgriDbContext(DbContextOptions<AgriDbContext> options)
            : base(options)
        {
        }

        // ---------------------------
        // Auth Module Tables
        // ---------------------------

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public DbSet<LoginActivity> LoginActivities { get; set; }
        public DbSet<JwtToken> JwtTokens { get; set; }
public DbSet<Farmer> Farmers { get; set; }
public DbSet<FarmerInterestedCrop> FarmerInterestedCrops { get; set; }
public DbSet<FarmDetails> FarmDetails { get; set; }
public DbSet<Buyer> Buyers { get; set; }
public DbSet<BuyerInterestedCrop> BuyerInterestedCrops { get; set; }
public DbSet<Crop> Crops { get; set; }
public DbSet<Seller> Sellers { get; set; }
public DbSet<SellerInterestedCrop> SellerInterestedCrops { get; set; }



        // ---------------------------
        // Model configuration loader
        // ---------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Load all configuration classes automatically
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgriDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
