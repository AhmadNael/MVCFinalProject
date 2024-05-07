using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MVCFinalProject.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Userinfo> Userinfos { get; set; }

    public virtual DbSet<Visa> Visas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##Recipe;PASSWORD=Test321;DATA SOURCE=192.168.1.19:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##RECIPE")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("SYS_C008552");

            entity.ToTable("INGREDIENT");

            entity.Property(e => e.IngredientId)
                .HasPrecision(10)
                .HasColumnName("INGREDIENT_ID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.IngredientName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("INGREDIENT_NAME");
            entity.Property(e => e.MeasuringTool)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MEASURING_TOOL");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("SYS_C008528");

            entity.ToTable("LOGIN");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.RoleId)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");

            entity.HasOne(d => d.Role).WithMany(p => p.Logins)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ROLE_FK");

            entity.HasOne(d => d.User).WithMany(p => p.Logins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USER_FK");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("SYS_C008523");

            entity.ToTable("ROLE");

            entity.Property(e => e.RoleId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ROLE_NAME");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("SYS_C008567");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.TestId)
                .HasPrecision(5)
                .HasColumnName("TEST_ID");
            entity.Property(e => e.Content)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.CreationDate)
                .HasColumnType("DATE")
                .HasColumnName("CREATION_DATE");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USER_TEST_FK");
        });

        modelBuilder.Entity<Userinfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008525");

            entity.ToTable("USERINFO");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.BirthDate)
                .HasColumnType("DATE")
                .HasColumnName("BIRTH_DATE");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("GENDER");
            entity.Property(e => e.ImgPath)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("IMG_PATH");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LAST_NAME");
        });

        modelBuilder.Entity<Visa>(entity =>
        {
            entity.HasKey(e => e.CardNumber).HasName("SYS_C008534");

            entity.ToTable("VISA");

            entity.Property(e => e.CardNumber)
                .HasPrecision(16)
                .ValueGeneratedNever()
                .HasColumnName("CARD_NUMBER");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(20)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.BillingAdrress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BILLING_ADRRESS");
            entity.Property(e => e.CardName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CARD_NAME");
            entity.Property(e => e.Cvc)
                .HasPrecision(3)
                .HasColumnName("CVC");
            entity.Property(e => e.ExpDate)
                .HasColumnType("DATE")
                .HasColumnName("EXP_DATE");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Visas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USER_VISA_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
