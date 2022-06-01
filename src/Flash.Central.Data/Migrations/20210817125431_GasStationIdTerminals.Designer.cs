﻿// <auto-generated />
using System;
using Flash.Central.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Flash.Central.Data.Migrations
{
    [DbContext(typeof(CentralDbContext))]
    [Migration("20210817125431_GasStationIdTerminals")]
    partial class GasStationIdTerminals
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Flash.Domain.Entities.Camera", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ApiKey")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("GasStationId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NetworkAddress")
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<int>("Port")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("GasStationId");

                    b.HasIndex("IsDeleted");

                    b.ToTable("Camera");

                    b.HasData(
                        new
                        {
                            Id = new Guid("132f36ee-4f53-4043-a642-233fba6ee8c4"),
                            ApiKey = "Cam1Test",
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GasStationId = 1L,
                            IsDeleted = false,
                            Login = "admin",
                            Name = "Cam1",
                            NetworkAddress = "127.0.0.1",
                            Notes = "Test",
                            Number = 1,
                            Password = "admin",
                            Port = 80,
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Flash.Domain.Entities.CameraRegion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("BottomRightX")
                        .HasColumnType("integer");

                    b.Property<int>("BottomRightY")
                        .HasColumnType("integer");

                    b.Property<Guid>("CameraId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<long?>("TerminalId")
                        .HasColumnType("bigint");

                    b.Property<int>("TopLeftX")
                        .HasColumnType("integer");

                    b.Property<int>("TopLeftY")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CameraId");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("TerminalId")
                        .IsUnique();

                    b.ToTable("CameraRegion");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            BottomRightX = 1920,
                            BottomRightY = 1080,
                            CameraId = new Guid("132f36ee-4f53-4043-a642-233fba6ee8c4"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            TopLeftX = 0,
                            TopLeftY = 0,
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Flash.Domain.Entities.DetectionEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("CameraRegionId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CroppedImageLink")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("OriginalImageLink")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<double>("Probability")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CameraRegionId");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("Timestamp");

                    b.ToTable("DetectionEvent");
                });

            modelBuilder.Entity("Flash.Domain.Entities.GasStation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("GasPumpCount")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("Name");

                    b.ToTable("GasStation");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GasPumpCount = 5,
                            IsDeleted = false,
                            Name = "Test",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Flash.Domain.Entities.RecognitionEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("CameraRegionId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ImageLink")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsIncorrectNumber")
                        .HasColumnType("boolean");

                    b.Property<string>("PlateNumber")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<double>("Probability")
                        .HasColumnType("double precision");

                    b.Property<string>("ProcessedImageLink")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("VisitId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CameraRegionId");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("Timestamp");

                    b.HasIndex("VisitId");

                    b.ToTable("RecognitionEvent");
                });

            modelBuilder.Entity("Flash.Domain.Entities.Terminal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<long>("GasStationId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Terminal");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GasStationId = 0L,
                            IsDeleted = false,
                            Name = "Test",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Flash.Domain.Entities.Visit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("End")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("EventsCount")
                        .HasColumnType("integer");

                    b.Property<long>("GasStationId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsClean")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsIncorrectVisit")
                        .HasColumnType("boolean");

                    b.Property<double>("PercentAssurance")
                        .HasColumnType("double precision");

                    b.Property<string>("PlateNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("GasStationId");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("PlateNumber");

                    b.HasIndex("Start");

                    b.ToTable("Visit");
                });

            modelBuilder.Entity("Flash.Domain.Entities.Camera", b =>
                {
                    b.HasOne("Flash.Domain.Entities.GasStation", "GasStation")
                        .WithMany("Cameras")
                        .HasForeignKey("GasStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GasStation");
                });

            modelBuilder.Entity("Flash.Domain.Entities.CameraRegion", b =>
                {
                    b.HasOne("Flash.Domain.Entities.Camera", "Camera")
                        .WithMany("Regions")
                        .HasForeignKey("CameraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flash.Domain.Entities.Terminal", "WatchingTerminal")
                        .WithOne("AssignedCameraRegion")
                        .HasForeignKey("Flash.Domain.Entities.CameraRegion", "TerminalId");

                    b.Navigation("Camera");

                    b.Navigation("WatchingTerminal");
                });

            modelBuilder.Entity("Flash.Domain.Entities.DetectionEvent", b =>
                {
                    b.HasOne("Flash.Domain.Entities.CameraRegion", "CameraRegion")
                        .WithMany()
                        .HasForeignKey("CameraRegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CameraRegion");
                });

            modelBuilder.Entity("Flash.Domain.Entities.RecognitionEvent", b =>
                {
                    b.HasOne("Flash.Domain.Entities.CameraRegion", "CameraRegion")
                        .WithMany()
                        .HasForeignKey("CameraRegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flash.Domain.Entities.Visit", "Visit")
                        .WithMany("RecognitionEvents")
                        .HasForeignKey("VisitId");

                    b.Navigation("CameraRegion");

                    b.Navigation("Visit");
                });

            modelBuilder.Entity("Flash.Domain.Entities.Visit", b =>
                {
                    b.HasOne("Flash.Domain.Entities.GasStation", "GasStation")
                        .WithMany()
                        .HasForeignKey("GasStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GasStation");
                });

            modelBuilder.Entity("Flash.Domain.Entities.Camera", b =>
                {
                    b.Navigation("Regions");
                });

            modelBuilder.Entity("Flash.Domain.Entities.GasStation", b =>
                {
                    b.Navigation("Cameras");
                });

            modelBuilder.Entity("Flash.Domain.Entities.Terminal", b =>
                {
                    b.Navigation("AssignedCameraRegion");
                });

            modelBuilder.Entity("Flash.Domain.Entities.Visit", b =>
                {
                    b.Navigation("RecognitionEvents");
                });
#pragma warning restore 612, 618
        }
    }
}
