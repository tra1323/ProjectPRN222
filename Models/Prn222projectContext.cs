using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProjectPRN222.Models;

public partial class Prn222projectContext : IdentityDbContext<User>
{
    public Prn222projectContext()
    {
    }

    public Prn222projectContext(DbContextOptions<Prn222projectContext> options) : base(options) { }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Manufacture> Manufactures { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-58FEABT;Initial Catalog=PRN222Project; Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Card__3214EC07172F3856");

            entity.ToTable("Card");

            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Product).WithMany(p => p.Cards)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Card__ProductId__619B8048");

            entity.HasOne(d => d.User).WithMany(p => p.Cards)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Card__UserId__628FA481");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07C3CEB23A");

            entity.ToTable("Category");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC07E31F1277");

            entity.ToTable("Comment");

            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Comment__ParentI__6383C8BA");

            entity.HasOne(d => d.Product).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Comment__Product__6477ECF3");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Comment__UserId__656C112C");
        });

        modelBuilder.Entity<Manufacture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC075E3549C7");

            entity.ToTable("Manufacture");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07618D1DCE");

            entity.ToTable("Notification");

            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(450);
            entity.Property(e => e.SendTo).HasMaxLength(450);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.NotificationCreateByNavigations)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK__Notificat__Creat__66603565");

            entity.HasOne(d => d.SendToNavigation).WithMany(p => p.NotificationSendToNavigations)
                .HasForeignKey(d => d.SendTo)
                .HasConstraintName("FK__Notificat__SendT__6754599E");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC070206F792");

            entity.ToTable("Order");

            entity.Property(e => e.CreateAt).HasColumnType("datetime");

            var statusConverter = new ValueConverter<OrderStatus, string>(
                v => v.ToString(),
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v)
            );

            entity.Property(e => e.Status)
                .HasConversion(statusConverter)  
                .HasMaxLength(50);             

            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Order__UserId__68487DD7");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC076B7F6493");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__693CA210");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__Produ__6A30C649");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC07709EFB8D");

            entity.ToTable("Payment");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TransactionCode).HasMaxLength(100);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Payment__OrderId__6B24EA82");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC0751366E7E");

            entity.ToTable("Product");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__Categor__6C190EBB");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("FK__Product__Manufac__6D0D32F4");
        });
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

