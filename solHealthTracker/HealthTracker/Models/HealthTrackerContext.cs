using HealthTracker.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace HealthTracker.Models
{
    public class HealthTrackerContext : DbContext
    {
        public HealthTrackerContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<HealthLog> HealthLogs { get; set; }
        public DbSet<IdealData> IdealDatas { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<MonitorPreference> MonitorPreferences { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UsersDetails { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<OAuthAccessTokenModel> OAuthAccessTokenModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasIndex(d => d.Email)
            .IsUnique();

            modelBuilder.Entity<HealthLog>()
                .HasOne(h => h.HealthLogForPreference)
                .WithMany(p => p.healthLogsOfUser)
                .HasForeignKey(h => h.PreferenceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MonitorPreference>()
                .HasOne(m => m.MonitorPreferenceForCoach)
                .WithMany(c => c.MonitorPreferences)
                .HasForeignKey(m => m.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserPreference>()
                .HasOne(u => u.PreferenceForUser)
                .WithMany(u => u.UserPreferences)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Target>()
                .HasOne(t => t.TargetForUserPreference)
                .WithMany(u => u.TargetsForUserPreference)
                .HasForeignKey(t => t.PreferenceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Suggestion>()
                .HasOne(s => s.SuggestionByCoach)
                .WithMany(c => c.SuggestionsByCoach)
                .HasForeignKey(s => s.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Suggestion>()
               .HasOne(s => s.SuggestionForUser)
               .WithMany(u => u.SuggestionsForUser)
               .HasForeignKey(s => s.UserId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
