using CAEVSYNC.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CAEVSYNC.Data;

public class CaevsyncDbContext : IdentityUserContext<IdentityUser>
{
    public DbSet<AccountToCalendarConnection> AccountToCalendarConnections { get; set; } 
    
    public DbSet<Calendar> Calendars { get; set; }

    public DbSet<ConnectedAccount> ConnectedAccounts { get; set; }

    public DbSet<EventTransformationStep> EventTransformationSteps { get; set; }
    
    public DbSet<EventTransformationIntReplaceStepData> EventTransformationIntReplaceStepData { get; set; }
    
    public DbSet<EventTransformationStringReplaceStepData> EventTransformationStringReplaceStepData { get; set; }
    
    public DbSet<EventTransformationBoolReplaceStepData> EventTransformationBoolReplaceStepData { get; set; }
    
    public DbSet<EventTransformationIntFilterData> EventTransformationIntFilterStepData { get; set; }
    
    public DbSet<EventTransformationStringFilterData> EventTransformationStringFilterData { get; set; }
    
    public DbSet<EventTransformationBoolFilterData> EventTransformationBoolFilterData { get; set; }
    
    public DbSet<EventTransformationDateTimeFilterData> EventTransformationDateTimeFilterData { get; set; }

    public DbSet<EventTransformationTimeExpandStepData> EventTransformationTimeExpandStepData { get; set; }
    
    public DbSet<SyncedEventData> SyncedEventData { get; set; } 
    
    public DbSet<SyncRule> SyncRules { get; set; }
    
    public DbSet<UserToAccountConnection> UserToAccountConnections { get; set; } 
    
    public CaevsyncDbContext (DbContextOptions<CaevsyncDbContext> options) : base(options) {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SyncRule>()
            .HasOne(e => e.OriginCalendar)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<SyncRule>()
            .HasOne(e => e.TargetCalendar)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}