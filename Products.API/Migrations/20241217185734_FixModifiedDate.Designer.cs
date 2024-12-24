﻿// <auto-generated />
using System;
using InnoShop.Products.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InnoShop.Products.API.Migrations
{
    [DbContext(typeof(ProductsDbContext))]
    [Migration("20241217185734_FixModifiedDate")]
    partial class FixModifiedDate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InnoShop.Products.API.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("character varying(3000)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatedDate")
                        .HasDatabaseName("IX_Product_Created");

                    b.HasIndex("IsAvailable")
                        .HasDatabaseName("IX_Product_Availability");

                    b.HasIndex("IsDeleted")
                        .HasDatabaseName("IX_Product_Deleted");

                    b.HasIndex("ModifiedDate")
                        .HasDatabaseName("IX_Product_Modified");

                    b.HasIndex("Price")
                        .HasDatabaseName("IX_Product_Price");

                    b.HasIndex("Title")
                        .HasDatabaseName("IX_Product_Title");

                    b.HasIndex("UserId")
                        .HasDatabaseName("IX_Product_User");

                    b.ToTable("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
