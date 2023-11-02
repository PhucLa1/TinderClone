using Microsoft.EntityFrameworkCore;
using JWTAuthencation.Models;
using JWTAuthencation.Models.ViewModel;

namespace JWTAuthencation.Data
{
    public class JWTAuthencationContext : DbContext
    {
        public JWTAuthencationContext(
            DbContextOptions<JWTAuthencationContext> options):base(options)
        {}
        public virtual DbSet<Block> Block { get; set; } = null!;
        public virtual DbSet<Call> Call { get; set; } = null!;
        public virtual DbSet<CallStatus> CallStatus { get; set; } = null!;
        public virtual DbSet<Languages> Languages { get; set; } = null!;
        public virtual DbSet<Like> Like { get; set; } = null!;
        public virtual DbSet<Mess> Mess { get; set; } = null!;
        public virtual DbSet<Passion> Passion { get; set; } = null!;
        public virtual DbSet<Permission> Permission { get; set; } = null!;
        public virtual DbSet<Photo> Photo{ get; set; } = null!;
        public virtual DbSet<Setting> Setting { get; set; } = null!;
        public virtual DbSet<SuggestedQuestion> SuggestedQuestion { get; set; } = null!;
        public virtual DbSet<Unlike> Unlike { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        public virtual DbSet<UsersLanguages> UsersLanguages { get; set; } = null!;
        public virtual DbSet<UsersPassion> UsersPassion { get; set; } = null!;
        public virtual DbSet<SexsualOrientation> SexsualOrientation { get; set; } = null!;
        public virtual DbSet<Communication> Communication { get; set; } = null!;
        public virtual DbSet<LoveLanguage> LoveLanguage { get; set; } = null!;
        public virtual DbSet<Pet> Pet { get; set; } = null!;
        public virtual DbSet<Alcolhol> Alcolhol { get; set; } = null!;
        public virtual DbSet<Diet> Diet { get; set; } = null!;
        public virtual DbSet<VacxinCovid> VacxinCovid { get; set; } = null!;
        public virtual DbSet<Zodiac> Zodiac { get; set; } = null!;
        public virtual DbSet<Personality> Personality { get; set; } = null!;
        public virtual DbSet<Smoke> Smoke { get; set; } = null!;
        public virtual DbSet<SocialMedia> SocialMedia { get; set; } = null!;
        public virtual DbSet<Education> Education { get; set; } = null!;
        public virtual DbSet<PurposeDate> PurposeDate { get; set; } = null!;
        public virtual DbSet<FutureFamily> FutureFamily { get; set; } = null!;
        public virtual DbSet<Workout> Workout { get; set; } = null!;
        public virtual DbSet<SleepHabit> SleepHabit { get; set; } = null!;
        public virtual DbSet<Admin> Admin { get; set; } = null!;
        //Procedure
        public virtual DbSet<UserInfo> GetUserProfile { get; set; } = null!;
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>()
            .HasKey(u => u.ID);
        }


    }
}
