using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public partial class DbSportsBuzzContext : DbContext
{
    public DbSportsBuzzContext()
    {
    }

    public DbSportsBuzzContext(DbContextOptions<DbSportsBuzzContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblSportType> TblSportTypes { get; set; }

    public virtual DbSet<TblTeam> TblTeams { get; set; }

    public virtual DbSet<TblTeamMember> TblTeamMembers { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserRole> TblUserRoles { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server = 65.0.181.176;Database=db_SportsBuzz;User Id = admin;Password = Asdf1234*;TrustServerCertificate=True;Connection Timeout=300;command timeout=300;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            entity.HasKey(e => e.MemberId).HasName("PK__tbl_Team__0CF04B18801EEA95");

            entity.ToTable("tbl_Team_Members");

            entity.Property(e => e.MemberId).ValueGeneratedNever();
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
                .HasConstraintName("FK__tbl_Team___TeamI__403A8C7D");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
