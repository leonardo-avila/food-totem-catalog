﻿// <auto-generated />
using System;
using FoodTotem.Catalog.Gateways.MySQL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FoodTotem.Catalog.Gateways.MySQL.Migrations
{
    [DbContext(typeof(CatalogContext))]
    partial class CatalogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("catalog")
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FoodTotem.Catalog.Domain.Models.Food", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("Id");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Category");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Description");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ImageUrl");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Name");

                    b.Property<double>("Price")
                        .HasColumnType("double")
                        .HasColumnName("Price");

                    b.HasKey("Id");

                    b.ToTable("Food", "catalog");
                });
#pragma warning restore 612, 618
        }
    }
}
