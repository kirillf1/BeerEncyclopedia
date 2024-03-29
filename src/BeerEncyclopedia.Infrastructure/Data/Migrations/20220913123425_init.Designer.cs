﻿// <auto-generated />
using System;
using System.Collections.Generic;
using BeerEncyclopedia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BeerEncyclopedia.Infrastructure.Migrations
{
    [DbContext(typeof(BeerEncyclopediaPgDbContext))]
    [Migration("20220913123425_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BeerEncyclopedia.Domain.Beer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AltName")
                        .HasColumnType("text");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly?>("CreationTime")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Rating")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("AltName");

                    b.HasIndex("CountryId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Name");

                    b.ToTable("Beers");
                });

            modelBuilder.Entity("BeerEncyclopedia.Domain.Color", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("BeerEncyclopedia.Domain.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("BeerEncyclopedia.Domain.Manufacturer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("BeerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("character varying(600)");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BeerId");

                    b.HasIndex("CountryId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Name");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("BeerEncyclopedia.Domain.Style", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("BeerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NameEn")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<string>("NameRus")
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.HasKey("Id");

                    b.HasIndex("BeerId");

                    b.ToTable("Styles");
                });

            modelBuilder.Entity("BeerEncyclopedia.Domain.Beer", b =>
                {
                    b.HasOne("BeerEncyclopedia.Domain.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.OwnsOne("BeerEncyclopedia.Domain.BeerImages", "BeerImages", b1 =>
                        {
                            b1.Property<Guid>("BeerId")
                                .HasColumnType("uuid");

                            b1.Property<List<string>>("ImageUrls")
                                .IsRequired()
                                .HasColumnType("text[]");

                            b1.Property<string>("MainImageUrl")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("BeerId");

                            b1.ToTable("BeerImages", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("BeerId");
                        });

                    b.OwnsOne("BeerEncyclopedia.Domain.ChemicalIndicators", "ChemicalIndicators", b1 =>
                        {
                            b1.Property<Guid>("BeerId")
                                .HasColumnType("uuid");

                            b1.Property<bool>("Filtration")
                                .HasColumnType("boolean");

                            b1.Property<double>("InitialWort")
                                .HasColumnType("double precision");

                            b1.Property<bool>("Pasteurization")
                                .HasColumnType("boolean");

                            b1.Property<double>("Strength")
                                .HasColumnType("double precision");

                            b1.HasKey("BeerId");

                            b1.ToTable("ChemicalIndicators", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("BeerId");
                        });

                    b.OwnsOne("BeerEncyclopedia.Domain.OrganolepticIndicators", "OrganolepticIndicators", b1 =>
                        {
                            b1.Property<Guid>("BeerId")
                                .HasColumnType("uuid");

                            b1.Property<double>("Bitterness")
                                .HasColumnType("double precision");

                            b1.Property<Guid>("ColorId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Taste")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("BeerId");

                            b1.HasIndex("ColorId");

                            b1.ToTable("OrganolepticIndicators", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("BeerId");

                            b1.HasOne("BeerEncyclopedia.Domain.Color", "Color")
                                .WithMany()
                                .HasForeignKey("ColorId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.Navigation("Color");
                        });

                    b.Navigation("BeerImages")
                        .IsRequired();

                    b.Navigation("ChemicalIndicators")
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("OrganolepticIndicators")
                        .IsRequired();
                });

            modelBuilder.Entity("BeerEncyclopedia.Domain.Manufacturer", b =>
                {
                    b.HasOne("BeerEncyclopedia.Domain.Beer", null)
                        .WithMany("Manufacturers")
                        .HasForeignKey("BeerId");

                    b.HasOne("BeerEncyclopedia.Domain.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("BeerEncyclopedia.Domain.Style", b =>
                {
                    b.HasOne("BeerEncyclopedia.Domain.Beer", null)
                        .WithMany("Styles")
                        .HasForeignKey("BeerId");
                });

            modelBuilder.Entity("BeerEncyclopedia.Domain.Beer", b =>
                {
                    b.Navigation("Manufacturers");

                    b.Navigation("Styles");
                });
#pragma warning restore 612, 618
        }
    }
}
