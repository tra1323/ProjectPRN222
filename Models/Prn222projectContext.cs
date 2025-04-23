using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectPRN222.Models;

public partial class Prn222projectContext : IdentityDbContext<User>
{
    public Prn222projectContext()
    {
    }

    public Prn222projectContext(DbContextOptions<Prn222projectContext> options) : base(options) { }


    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-SOFGUG9\\MSSQLSERVERA;Initial Catalog=PRN222Project; Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(l => l.UserId);

        modelBuilder.Entity<IdentityUserRole<string>>()
            .HasKey(r => new { r.UserId, r.RoleId });

        modelBuilder.Entity<IdentityUserClaim<string>>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<IdentityRoleClaim<string>>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<IdentityUserToken<string>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
