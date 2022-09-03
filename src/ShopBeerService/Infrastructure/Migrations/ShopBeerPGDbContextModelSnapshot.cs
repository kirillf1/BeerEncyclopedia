﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShopBeerService.Infrastructure;

#nullable disable

namespace ShopBeerService.Migrations
{
    [DbContext(typeof(ShopBeerPGDbContext))]
    partial class ShopBeerPGDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ShopParsers.Shop", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("ShopParsers.ShopBeer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Brand")
                        .HasColumnType("text");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("DetailsUrl")
                        .HasColumnType("text");

                    b.Property<decimal?>("DiscountPrice")
                        .HasColumnType("numeric");

                    b.Property<bool?>("Filtration")
                        .HasColumnType("boolean");

                    b.Property<double?>("InitialWort")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NameEn")
                        .HasColumnType("text");

                    b.Property<bool?>("Pasteurization")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<double?>("Rating")
                        .HasColumnType("double precision");

                    b.Property<Guid>("ShopId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("SourceBeerId")
                        .HasColumnType("uuid");

                    b.Property<double?>("Strength")
                        .HasColumnType("double precision");

                    b.Property<string>("Style")
                        .HasColumnType("text");

                    b.Property<double?>("Volume")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("ShopId");

                    b.ToTable("ShopBeers");
                });

            modelBuilder.Entity("ShopParsers.ShopBeer", b =>
                {
                    b.HasOne("ShopParsers.Shop", null)
                        .WithMany("ShopBeers")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShopParsers.Shop", b =>
                {
                    b.Navigation("ShopBeers");
                });
#pragma warning restore 612, 618
        }
    }
}
