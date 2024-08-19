﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebReactApp.Server.Data;

#nullable disable

namespace WebReactApp.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240819161345_v00003")]
    partial class v00003
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("WebReactApp.Server.ModelObjects.Identity.Account", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("WebReactApp.Server.ModelObjects.Identity.AccountConfirmTicket", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AccountID")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("ExpireAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.ToTable("AccountConfirmTickets");
                });

            modelBuilder.Entity("WebReactApp.Server.ModelObjects.Identity.AccountRole", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AccountID")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.ToTable("AccountRoles");
                });

            modelBuilder.Entity("WebReactApp.Server.ModelObjects.Identity.LoginMethod.UsernamePasswordMethod", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AccountID")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("PasswordItr")
                        .HasColumnType("int");

                    b.Property<int>("PasswordMethod")
                        .HasColumnType("int");

                    b.Property<int>("PasswordPrf")
                        .HasColumnType("int");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("PasswordSaltLength")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.HasIndex("AccountID")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("UsernamePasswordMethods");
                });

            modelBuilder.Entity("WebReactApp.Server.ModelObjects.Identity.AccountConfirmTicket", b =>
                {
                    b.HasOne("WebReactApp.Server.ModelObjects.Identity.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("WebReactApp.Server.ModelObjects.Identity.AccountRole", b =>
                {
                    b.HasOne("WebReactApp.Server.ModelObjects.Identity.Account", null)
                        .WithMany("Roles")
                        .HasForeignKey("AccountID");
                });

            modelBuilder.Entity("WebReactApp.Server.ModelObjects.Identity.LoginMethod.UsernamePasswordMethod", b =>
                {
                    b.HasOne("WebReactApp.Server.ModelObjects.Identity.Account", "Account")
                        .WithOne("UsernamePasswordMethod")
                        .HasForeignKey("WebReactApp.Server.ModelObjects.Identity.LoginMethod.UsernamePasswordMethod", "AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("WebReactApp.Server.ModelObjects.Identity.Account", b =>
                {
                    b.Navigation("Roles");

                    b.Navigation("UsernamePasswordMethod")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
