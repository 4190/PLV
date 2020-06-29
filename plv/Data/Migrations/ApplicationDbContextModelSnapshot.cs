﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using plv.Data;

namespace plv.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
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

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
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

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("plv.BlockModels.FirstBlock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddedByHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentUserHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateAddedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateReceivedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocIdHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousAddedByHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousCurrentUserHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousDateAddedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousDateReceivedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousDocIdHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousReceiverHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousSenderHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousShortOptionalDescriptionHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortOptionalDescriptionHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FirstBlock");
                });

            modelBuilder.Entity("plv.BlockModels.SecondBlock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddedByHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentUserHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateAddedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateReceivedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocIdHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousAddedByHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousCurrentUserHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousDateAddedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousDateReceivedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousDocIdHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousReceiverHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousSenderHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousShortOptionalDescriptionHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortOptionalDescriptionHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SecondBlock");
                });

            modelBuilder.Entity("plv.BlockModels.ThirdBlock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddedByHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentUserHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateAddedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateReceivedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocIdHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousAddedByHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousCurrentUserHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousDateAddedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousDateReceivedHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousDocIdHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousReceiverHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousSenderHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousShortOptionalDescriptionHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortOptionalDescriptionHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ThirdBlock");
                });

            modelBuilder.Entity("plv.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

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
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("plv.Models.DocEdits", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<string>("DocumentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EditTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EditedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NewDateIssued")
                        .HasColumnType("datetime2");

                    b.Property<string>("NewDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewReceiver")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewSender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousReceiver")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousSender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousUser")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DocumentEdits");
                });

            modelBuilder.Entity("plv.Models.DocumentInDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateReceived")
                        .HasColumnType("datetime2");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Receiver")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Section")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortOptionalDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("plv.Models.DocumentsSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DocumentInDBId")
                        .HasColumnType("int");

                    b.Property<string>("SectionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DocumentInDBId");

                    b.HasIndex("SectionId");

                    b.ToTable("DocumentsSections");
                });

            modelBuilder.Entity("plv.Models.Downloads", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DownloadTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SectionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Downloads");
                });

            modelBuilder.Entity("plv.Models.Section", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("plv.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("plv.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("plv.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("plv.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("plv.Models.DocumentsSection", b =>
                {
                    b.HasOne("plv.Models.DocumentInDB", "DocumentInDB")
                        .WithMany()
                        .HasForeignKey("DocumentInDBId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("plv.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId");
                });
#pragma warning restore 612, 618
        }
    }
}
