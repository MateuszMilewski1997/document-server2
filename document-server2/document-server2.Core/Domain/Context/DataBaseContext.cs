﻿using Microsoft.EntityFrameworkCore;

namespace document_server2.Core.Domain.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(key => key.Email);

            modelBuilder.Entity<Role>()
                .HasKey(key => key.Name);

            modelBuilder.Entity<User>(column =>
            {
                column.Property(name => name.Email)
                      .HasColumnType("nvarchar(50)")
                      .IsRequired(true);
                column.Property(name => name.Login)
                      .HasColumnType("nvarchar(20)")
                      .IsRequired(false);
                column.Property(name => name.Password)
                      .HasColumnType("nvarchar(MAX)")
                      .IsRequired(false);
                column.Property(name => name.Role_name)
                      .HasColumnType("nvarchar(20)")
                      .IsRequired(true);
            });

            modelBuilder.Entity<Role>(column =>
            {
                column.Property(name => name.Name)
                      .HasColumnType("nvarchar(20)")
                      .IsRequired(true);
            });

            modelBuilder.Entity<Case>(column =>
            {
                column.Property(name => name.User_email)
                      .HasColumnType("nvarchar(50)")
                      .IsRequired(true);
                column.Property(name => name.Title)
                      .HasColumnType("nvarchar(50)")
                      .IsRequired(true);
                column.Property(name => name.Type)
                      .HasColumnType("nvarchar(60)")
                      .IsRequired(true);
                column.Property(name => name.Date)
                      .HasColumnType("datetime")
                      .IsRequired(true);
                column.Property(name => name.Description)
                      .HasColumnType("varchar(500)")
                      .IsRequired(true);
                column.Property(name => name.Comment)
                      .HasColumnType("nvarchar(MAX)")
                      .IsRequired(true);
                column.Property(name => name.Status)
                      .HasColumnType("nvarchar(20)")
                      .IsRequired(true);
            });

            modelBuilder.Entity<Document>(column =>
            {
                column.Property(name => name.Case_id)
                      .HasColumnType("int")
                      .IsRequired(true);
                column.Property(name => name.Name)
                      .HasColumnType("nvarchar(50)")
                      .IsRequired(true);
                column.Property(name => name.Url)
                      .HasColumnType("nvarchar(MAX)")
                      .IsRequired(true);
            });

            modelBuilder.Entity<Recipient>(column =>
            {
                column.Property(name => name.Case_id)
                      .HasColumnType("int")
                      .IsRequired(true);
                column.Property(name => name.Email)
                      .HasColumnType("nvarchar(50)")
                      .IsRequired(true);
            });

            modelBuilder.Entity<User>()
                        .HasOne(a => a.Role)
                        .WithMany()
                        .HasForeignKey(x => x.Role_name);

            modelBuilder.Entity<User>()
                        .HasMany(a => a.Cases)
                        .WithOne()
                        .HasForeignKey(x => x.User_email);

            modelBuilder.Entity<Case>()
                        .HasMany(a => a.Documents)
                        .WithOne()
                        .HasForeignKey(x => x.Case_id);

            modelBuilder.Entity<Case>()
                        .HasMany(a => a.Recipients)
                        .WithOne()
                        .HasForeignKey(x => x.Case_id);

            modelBuilder.Entity<Role>().HasData(
                new Role("admin"),
                new Role("unregistered"),
                new Role("registered"),
                new Role("dropped"),
                new Role("skarga"),
                new Role("podanie")
                );

            modelBuilder.Entity<User>().HasData(
                new User("admin@admin.com", "admin", "2bb80d537b1da3e38bd30361aa855686bde0eacd7162fef6a25fe97bf527a25b", "admin"),
                new User("podanie@podanie.com", "podanie", "2bb80d537b1da3e38bd30361aa855686bde0eacd7162fef6a25fe97bf527a25b", "podanie"),
                new User("registered@registered.com", "registered", "2bb80d537b1da3e38bd30361aa855686bde0eacd7162fef6a25fe97bf527a25b", "registered"),
                new User("skarga@skarga.com", "skarga", "2bb80d537b1da3e38bd30361aa855686bde0eacd7162fef6a25fe97bf527a25b", "skarga")
            );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
    }
}
