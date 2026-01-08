using Microsoft.EntityFrameworkCore;
using agriApp.Entities.Auth;
using agriApp.Entities.Stakeholders;
using agriApp.Entities.Market;
using agriApp.Entities.Lots;
using agriApp.Data.Configurations;
using agriApp.Entities.Auctions;
using agriApp.Entities.Geography;
using agriApp.Entities.Lookups;
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
public DbSet<Mandi> Mandis { get; set; }
public DbSet<OfficialRole> OfficialRoles { get; set; }
public DbSet<MandiOfficial> MandiOfficials { get; set; }

public DbSet<PreRegisteredLot> PreRegisteredLots { get; set; }
public DbSet<ArrivedLot> ArrivedLots { get; set; }
public DbSet<LiveAuctionLot> LiveAuctionLots { get; set; }

public DbSet<BuyerInterestLot> BuyerInterestLots { get; set; }

public DbSet<Auction> Auctions { get; set; }
public DbSet<Anchor> Anchors { get; set; }

public DbSet<AnchorFarmer> AnchorFarmers { get; set; }

public DbSet<GeoState> GeoStates { get; set; }
public DbSet<GeoDistrict> GeoDistricts { get; set; }
public DbSet<GeoTaluka> GeoTalukas { get; set; }

public DbSet<MandiCategory> MandiCategories { get; set; }
        // ---------------------------
        // Model configuration loader
        // ---------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Load all configuration classes automatically
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgriDbContext).Assembly);
// modelBuilder.ApplyConfiguration(new BuyerInterestLotConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
