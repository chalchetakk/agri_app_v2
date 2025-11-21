using Microsoft.EntityFrameworkCore;
using agriApp.Entities.Auth;

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
