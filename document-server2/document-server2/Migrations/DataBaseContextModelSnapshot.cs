﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using document_server2.Core.Domain.Context;

namespace document_server2.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("document_server2.Core.Domain.Case", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("User_email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("User_email");

                    b.ToTable("Cases");
                });

            modelBuilder.Entity("document_server2.Core.Domain.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Case_id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(MAX)");

                    b.HasKey("Id");

                    b.HasIndex("Case_id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("document_server2.Core.Domain.Recipient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Case_id")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Case_id");

                    b.ToTable("Recipients");
                });

            modelBuilder.Entity("document_server2.Core.Domain.Role", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("Add_comments")
                        .HasColumnType("bit");

                    b.Property<bool>("Add_documents")
                        .HasColumnType("bit");

                    b.Property<bool>("Add_users")
                        .HasColumnType("bit");

                    b.Property<bool>("Change_status")
                        .HasColumnType("bit");

                    b.HasKey("Name");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Name = "admin",
                            Add_comments = true,
                            Add_documents = true,
                            Add_users = true,
                            Change_status = true
                        },
                        new
                        {
                            Name = "unregistered",
                            Add_comments = false,
                            Add_documents = true,
                            Add_users = false,
                            Change_status = false
                        },
                        new
                        {
                            Name = "registered",
                            Add_comments = true,
                            Add_documents = true,
                            Add_users = false,
                            Change_status = false
                        },
                        new
                        {
                            Name = "dropped",
                            Add_comments = true,
                            Add_documents = false,
                            Add_users = false,
                            Change_status = false
                        },
                        new
                        {
                            Name = "skarga",
                            Add_comments = true,
                            Add_documents = false,
                            Add_users = false,
                            Change_status = true
                        },
                        new
                        {
                            Name = "podanie",
                            Add_comments = true,
                            Add_documents = false,
                            Add_users = true,
                            Change_status = true
                        });
                });

            modelBuilder.Entity("document_server2.Core.Domain.User", b =>
                {
                    b.Property<string>("Email")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Role_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Email");

                    b.HasIndex("Role_name");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("document_server2.Core.Domain.Case", b =>
                {
                    b.HasOne("document_server2.Core.Domain.User")
                        .WithMany("Cases")
                        .HasForeignKey("User_email")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("document_server2.Core.Domain.Document", b =>
                {
                    b.HasOne("document_server2.Core.Domain.Case")
                        .WithMany("Documents")
                        .HasForeignKey("Case_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("document_server2.Core.Domain.Recipient", b =>
                {
                    b.HasOne("document_server2.Core.Domain.Case")
                        .WithMany("Recipients")
                        .HasForeignKey("Case_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("document_server2.Core.Domain.User", b =>
                {
                    b.HasOne("document_server2.Core.Domain.Role", "Role")
                        .WithMany()
                        .HasForeignKey("Role_name")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}