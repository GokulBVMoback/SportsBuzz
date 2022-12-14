using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public partial class DbSportsBuzzContext : DbContext
{
    public DbSportsBuzzContext(DbContextOptions<DbSportsBuzzContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookedGroundView> BookedGroundViews { get; set; }

    public virtual DbSet<ChallengeTeamView> ChallengeTeamViews { get; set; }

    public virtual DbSet<GroundView> GroundViews { get; set; }

    public virtual DbSet<TblBookGround> TblBookGrounds { get; set; }

    public virtual DbSet<TblChallenge> TblChallenges { get; set; }

    public virtual DbSet<TblGround> TblGrounds { get; set; }

    public virtual DbSet<TblSession> TblSessions { get; set; }

    public virtual DbSet<TblSportType> TblSportTypes { get; set; }

    public virtual DbSet<TblTeam> TblTeams { get; set; }

    public virtual DbSet<TblTeamMember> TblTeamMembers { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserRole> TblUserRoles { get; set; }

    public virtual DbSet<TeamMemberView> TeamMemberViews { get; set; }

    public virtual DbSet<TeamView> TeamViews { get; set; }

    public virtual DbSet<UserView> UserViews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookedGroundView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("BookedGroundView");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.SportType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeamName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Venue)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ChallengeTeamView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ChallengeTeamView");

            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.SportType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Team1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Team2)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GroundView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("GroundView");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.SportType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Venue)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblBookGround>(entity =>
        {
            entity.HasKey(e => e.BookedId).HasName("PK__tbl_Book__FA2CBA5A5DB7F084");

            entity.ToTable("tbl_BookGround");

            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.GroundId).HasColumnName("GroundID");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Ground).WithMany(p => p.TblBookGrounds)
                .HasForeignKey(d => d.GroundId)
                .HasConstraintName("FK__tbl_BookG__Groun__4F7CD00D");

            entity.HasOne(d => d.Session).WithMany(p => p.TblBookGrounds)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__tbl_BookG__Sessi__4E88ABD4");

            entity.HasOne(d => d.Team).WithMany(p => p.TblBookGrounds)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__tbl_BookG__TeamI__4D94879B");
        });

        modelBuilder.Entity<TblChallenge>(entity =>
        {
            entity.HasKey(e => e.ChallengeId).HasName("PK__tbl_Chal__C7AC81C86F467E2A");

            entity.ToTable("tbl_Challenge");

            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.TeamId1).HasColumnName("TeamID1");
            entity.Property(e => e.TeamId2).HasColumnName("TeamID2");

            entity.HasOne(d => d.Session).WithMany(p => p.TblChallenges)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__tbl_Chall__Sessi__534D60F1");

            entity.HasOne(d => d.SportTypeNavigation).WithMany(p => p.TblChallenges)
                .HasForeignKey(d => d.SportType)
                .HasConstraintName("FK__tbl_Chall__Sport__5535A963");

            entity.HasOne(d => d.TeamId1Navigation).WithMany(p => p.TblChallengeTeamId1Navigations)
                .HasForeignKey(d => d.TeamId1)
                .HasConstraintName("FK__tbl_Chall__TeamI__52593CB8");

            entity.HasOne(d => d.TeamId2Navigation).WithMany(p => p.TblChallengeTeamId2Navigations)
                .HasForeignKey(d => d.TeamId2)
                .HasConstraintName("FK__tbl_Chall__TeamI__5441852A");
        });

        modelBuilder.Entity<TblGround>(entity =>
        {
            entity.HasKey(e => e.GroundId).HasName("PK__tbl_Grou__3B3A8E8032515E6E");

            entity.ToTable("tbl_Ground");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Latitude)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Longitude)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Venue)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.SportTypeNavigation).WithMany(p => p.TblGrounds)
                .HasForeignKey(d => d.SportType)
                .HasConstraintName("FK__tbl_Groun__Sport__4316F928");

            entity.HasOne(d => d.User).WithMany(p => p.TblGrounds)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tbl_Groun__UserI__440B1D61");
        });

        modelBuilder.Entity<TblSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__tbl_Sess__C9F49290ADF43E8E");

            entity.ToTable("tbl_Session");
        });

        modelBuilder.Entity<TblSportType>(entity =>
        {
            entity.HasKey(e => e.SportTypeId).HasName("PK__tbl_Spor__509BB1FFC75231C7");

            entity.ToTable("tbl_SportType");

            entity.Property(e => e.SportType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblTeam>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PK__tbl_Team__123AE7995B0650EB");

            entity.ToTable("tbl_Team");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeamName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.SportTypeNavigation).WithMany(p => p.TblTeams)
                .HasForeignKey(d => d.SportType)
                .HasConstraintName("FK__tbl_Team__SportT__33D4B598");

            entity.HasOne(d => d.User).WithMany(p => p.TblTeams)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tbl_Team__UserID__34C8D9D1");
        });

        modelBuilder.Entity<TblTeamMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__tbl_Team__0CF04B180BFFDEE3");

            entity.ToTable("tbl_Team_Members");

            entity.Property(e => e.PlayerFirstName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PlayerLastName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Team).WithMany(p => p.TblTeamMembers)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__tbl_Team___TeamI__66603565");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__tbl_User__1788CC4C84C0F818");

            entity.ToTable("tbl_User");

            entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("smalldatetime");

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.UserRole)
                .HasConstraintName("FK__tbl_User__UserRo__29572725");
        });

        modelBuilder.Entity<TblUserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__tbl_User__3D978A35710DEFE9");

            entity.ToTable("tbl_User_Role");

            entity.Property(e => e.UserRole)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TeamMemberView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TeamMemberView");

            entity.Property(e => e.PlayerFirstName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PlayerLastName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeamName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TeamView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TeamView");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SportType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeamName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<UserView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("UserView");

            entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UserRole)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
