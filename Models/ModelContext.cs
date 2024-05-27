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

    public virtual DbSet<Aboutu> Aboutus { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contactu> Contactus { get; set; }

    public virtual DbSet<Homepage> Homepages { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Userinfo> Userinfos { get; set; }

    public virtual DbSet<Visa> Visas { get; set; }

    public virtual DbSet<VisaChecker> VisaCheckers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##Recipe;PASSWORD=Test321;DATA SOURCE=192.168.1.19:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##RECIPE")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008591");

            entity.ToTable("ABOUTUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Content)
                .HasColumnType("CLOB")
                .HasColumnName("CONTENT");
            entity.Property(e => e.ImgPath)
                .HasColumnType("CLOB")
                .HasColumnName("IMG_PATH");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("SYS_C008570");

            entity.ToTable("CATEGORY");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.CategoryImg)
                .HasColumnType("CLOB")
                .HasColumnName("CATEGORY_IMG");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CATEGORY_NAME");
        });

        modelBuilder.Entity<Contactu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008589");

            entity.ToTable("CONTACTUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Message)
                .HasColumnType("CLOB")
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Subject)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SUBJECT");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
        });

        modelBuilder.Entity<Homepage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008593");

            entity.ToTable("HOMEPAGE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.HeroImgContent)
                .HasColumnType("CLOB")
                .HasColumnName("HERO_IMG_CONTENT");
            entity.Property(e => e.HeroImgTitle)
                .HasColumnType("CLOB")
                .HasColumnName("HERO_IMG_TITLE");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.WebsiteName)
                .HasColumnType("CLOB")
                .HasColumnName("WEBSITE_NAME");
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

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("SYS_C008573");

            entity.ToTable("RECIPE");

            entity.Property(e => e.RecipeId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPE_ID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.CreationDate)
                .HasColumnType("DATE")
                .HasColumnName("CREATION_DATE");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Price)
                .HasColumnType("FLOAT")
                .HasColumnName("PRICE");
            entity.Property(e => e.RecipeImg)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("RECIPE_IMG");
            entity.Property(e => e.RecipeInfo)
                .HasColumnType("CLOB")
                .HasColumnName("RECIPE_INFO");
            entity.Property(e => e.StatusId)
                .HasColumnType("NUMBER")
                .HasColumnName("STATUS_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CATEGORY_FK");

            entity.HasOne(d => d.Status).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("RECIPE_FK1");

            entity.HasOne(d => d.User).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USERS_ID_FK");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("SYS_C008620");

            entity.ToTable("REQUEST");

            entity.Property(e => e.RequestId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("REQUEST_ID");
            entity.Property(e => e.RecipeId)
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPE_ID");
            entity.Property(e => e.RequestTax)
                .HasColumnType("FLOAT")
                .HasColumnName("REQUEST_TAX");
            entity.Property(e => e.RequsetDate)
                .HasColumnType("DATE")
                .HasColumnName("REQUSET_DATE");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Requests)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("REQUEST_RECIPE_FK");

            entity.HasOne(d => d.User).WithMany(p => p.Requests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("REQUEST_USER_FK");
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

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008585");

            entity.ToTable("STATUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.StatusType)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("STATUS_TYPE");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("SYS_C008595");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.TestId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TEST_ID");
            entity.Property(e => e.Content)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("SYSTIMESTAMP")
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("CREATION_DATE");
            entity.Property(e => e.StatusId)
                .HasColumnType("NUMBER")
                .HasColumnName("STATUS_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Status).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("TESTIMONIAL_FK1");

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
            entity.HasKey(e => e.Id).HasName("SYS_C008601");

            entity.ToTable("VISA");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.BillingAdrress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BILLING_ADRRESS");
            entity.Property(e => e.CardName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CARD_NAME");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CARD_NUMBER");
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

        modelBuilder.Entity<VisaChecker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008624");

            entity.ToTable("VISA_CHECKER");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("BALANCE");
            entity.Property(e => e.CardHolderName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CARD_HOLDER_NAME");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CARD_NUMBER");
            entity.Property(e => e.Cvc)
                .HasPrecision(3)
                .HasColumnName("CVC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
