﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Survay_Basket.API.Presistence;

#nullable disable

namespace Survay_Basket.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaimType = "permissions",
                            ClaimValue = "polls:read",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 2,
                            ClaimType = "permissions",
                            ClaimValue = "polls:add",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 3,
                            ClaimType = "permissions",
                            ClaimValue = "polls:update",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 4,
                            ClaimType = "permissions",
                            ClaimValue = "polls:delete",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 5,
                            ClaimType = "permissions",
                            ClaimValue = "questions:read",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 6,
                            ClaimType = "permissions",
                            ClaimValue = "questions:add",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 7,
                            ClaimType = "permissions",
                            ClaimValue = "questions:update",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 8,
                            ClaimType = "permissions",
                            ClaimValue = "users:read",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 9,
                            ClaimType = "permissions",
                            ClaimValue = "users:add",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 10,
                            ClaimType = "permissions",
                            ClaimValue = "users:update",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 11,
                            ClaimType = "permissions",
                            ClaimValue = "roles:read",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 12,
                            ClaimType = "permissions",
                            ClaimValue = "roles:add",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 13,
                            ClaimType = "permissions",
                            ClaimValue = "roles:update",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        },
                        new
                        {
                            Id = 14,
                            ClaimType = "permissions",
                            ClaimValue = "results:read",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "bf301080-df71-4f1f-a9dc-e3b72c0af129",
                            RoleId = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId", "Content");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e",
                            ConcurrencyStamp = "adcbac50-38d5-4c17-90bd-89a5fe7b8896",
                            IsDefault = false,
                            IsDeleted = false,
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "5895c06f-d555-406b-8dfe-692716db429d",
                            ConcurrencyStamp = "f8d821dc-ced0-4c53-ac50-9d1ec179d62e",
                            IsDefault = true,
                            IsDeleted = false,
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "bf301080-df71-4f1f-a9dc-e3b72c0af129",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "78d35802-f198-4baf-92ff-e719824c9977",
                            Email = "admin@survay-basket.com",
                            EmailConfirmed = true,
                            FirstName = "Survay",
                            IsDisabled = false,
                            LastName = "Admin",
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@SURVAY-BASKET.COM",
                            NormalizedUserName = "ADMIN@SURVAY-BASKET.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEGGazIzGMOSb0LidgAkPx5j3DXx08kxbPRNElSZk855KI7jGHHVK0y3nvo3UxkzH1Q==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "AB25ACC0CFEE4807BFE4FBDDBEB46B0C",
                            TwoFactorEnabled = false,
                            UserName = "admin@survay-basket.com"
                        });
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.Poll", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedById")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("EndsAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartsAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdateOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedById")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.HasIndex("UpdatedById");

                    b.ToTable("Polls");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("CreateOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedById")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("PollId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedById")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.HasIndex("PollId", "Content");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.Vote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PollId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmittedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("PollId", "UserId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.VoteAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnswerId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("VoteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("VoteId", "QuestionId");

                    b.ToTable("VoteAnswers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Survay_Basket.API.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Survay_Basket.API.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Survay_Basket.API.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Survay_Basket.API.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Survay_Basket.API.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Survay_Basket.API.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.Answer", b =>
                {
                    b.HasOne("Survay_Basket.API.Entities.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.ApplicationUser", b =>
                {
                    b.OwnsMany("Survay_Basket.API.Entities.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<string>("UserId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<DateTime>("CreateOn")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("ExpiresOn")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime?>("RevokeOn")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Token")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserId", "Id");

                            b1.ToTable("RefreshTokens", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.Poll", b =>
                {
                    b.HasOne("Survay_Basket.API.Entities.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Survay_Basket.API.Entities.ApplicationUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("CreatedBy");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.Question", b =>
                {
                    b.HasOne("Survay_Basket.API.Entities.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Survay_Basket.API.Entities.Poll", "Poll")
                        .WithMany("Questions")
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Survay_Basket.API.Entities.ApplicationUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("CreatedBy");

                    b.Navigation("Poll");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.Vote", b =>
                {
                    b.HasOne("Survay_Basket.API.Entities.Poll", "Poll")
                        .WithMany("Votes")
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Survay_Basket.API.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Poll");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.VoteAnswer", b =>
                {
                    b.HasOne("Survay_Basket.API.Entities.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Survay_Basket.API.Entities.Question", "Question")
                        .WithMany("VoteAnswers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Survay_Basket.API.Entities.Vote", "Vote")
                        .WithMany("Answers")
                        .HasForeignKey("VoteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Question");

                    b.Navigation("Vote");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.Poll", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("VoteAnswers");
                });

            modelBuilder.Entity("Survay_Basket.API.Entities.Vote", b =>
                {
                    b.Navigation("Answers");
                });
#pragma warning restore 612, 618
        }
    }
}
