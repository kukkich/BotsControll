using BotsControll.Core.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace BotsControll.Core.Models;

public class BotsControlContext : DbContext
{
    #region Tables

    public DbSet<Bot> Bots { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Journal> Journal { get; set; } = null!;
    public DbSet<ManagementRecord> ManagementRecords { get; set; } = null!;
    public DbSet<StateRecord> StateRecords { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<BotGroup> BotGroups { get; set; } = null!;
    public DbSet<UserGroup> UserGroups { get; set; } = null!;

    public DbSet<Token> Tokens { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<IdentityClaim> IdentityClaims { get; set; } = null!;

    #endregion

    public BotsControlContext(DbContextOptions options)
        : base(options)
    {
        if (!Database.EnsureCreated()) return;

        var user = new User()
        {
            Name = "admin",
            Email = "kukkich@gmail.com",
            Login = "kukkich",
            PasswordHash = "123"
        };
        var adminRole = new Role { Name = "admin" };
        UserRoles.Add(new UserRole { User = user, Role = adminRole });
        Users.Add(user);
        Bots.Add(new Bot()
        {
            Id = "bob_id",
            Name = "Bob",
            IsActive = false,
            Owner = user
        });

        var guest = new User()
        {
            Name = "guest",
            Email = "pumpkin@mail.ru",
            Login = "Ivan",
            PasswordHash = "111",
        };
        var guestsBot = new Bot()
        {
            Id = "anna_id",
            Name = "Anna",
            IsActive = false,
            Owner = guest
        };

        Users.Add(guest);
        Bots.Add(guestsBot);

        SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        UserRolesConfiguration(modelBuilder);
        GroupConfiguration(modelBuilder);

        modelBuilder.Entity<Journal>()
            .HasMany<ManagementRecord>()
            .WithOne(m => m.Journal)
            .HasForeignKey(m => m.JournalId);


        modelBuilder.Entity<ManagementRecord>()
            .HasOne(m => m.Author)
            .WithMany(u => u.ManagementRecords)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<User>()
            .HasMany<ManagementRecord>()
            .WithOne(m => m.Author)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<User>()
            .HasMany<Group>()
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private static void UserRolesConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>()
            .HasKey(r => new { r.UserId, r.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }
    private static void GroupConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserGroup>()
            .HasKey(k => new { k.UserId, k.GroupId });
        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.User)
            .WithMany(u => u.UserGroups)
            .HasForeignKey(ug => ug.UserId);
        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.Group)
            .WithMany(g => g.UserGroups)
            .HasForeignKey(ug => ug.GroupId);

        modelBuilder.Entity<BotGroup>()
            .HasKey(k => new { k.BotId, k.GroupId });
        modelBuilder.Entity<BotGroup>()
            .HasOne(bg => bg.Bot)
            .WithMany(b => b.BotGroups)
            .HasForeignKey(bg => bg.BotId);
        modelBuilder.Entity<BotGroup>()
            .HasOne(bg => bg.Group)
            .WithMany(g => g.BotGroups)
            .HasForeignKey(bg => bg.GroupId);
    }

}