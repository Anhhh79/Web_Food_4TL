﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web_Food_4TL.Data;

#nullable disable

namespace Web_Food_4TL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Web_Food_4TL.Models.AnhMonAn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MonAnId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MonAnId");

                    b.ToTable("AnhMonAns");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.DanhMuc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("TenDanhMuc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DanhMucs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            TenDanhMuc = "Starters"
                        },
                        new
                        {
                            Id = 2,
                            TenDanhMuc = "Breakfast"
                        },
                        new
                        {
                            Id = 3,
                            TenDanhMuc = "Lunch"
                        },
                        new
                        {
                            Id = 4,
                            TenDanhMuc = "Dinner"
                        });
                });

            modelBuilder.Entity("Web_Food_4TL.Models.GioHang", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Gia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MonAnId")
                        .HasColumnType("int");

                    b.Property<int>("NguoiDungId")
                        .HasColumnType("int");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MonAnId");

                    b.HasIndex("NguoiDungId");

                    b.ToTable("GioHangs");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.HoaDon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DiaChiGiaoHang")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<int>("NguoiDungId")
                        .HasColumnType("int");

                    b.Property<string>("SoDienThoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TrangThai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NguoiDungId");

                    b.ToTable("HoaDons");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.HoaDonChiTiet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Gia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("HoaDonId")
                        .HasColumnType("int");

                    b.Property<int>("MonAnId")
                        .HasColumnType("int");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<string>("TenMonAn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HoaDonId");

                    b.HasIndex("MonAnId");

                    b.ToTable("HoaDonChiTiets");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.MonAn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DanhMucId")
                        .HasColumnType("int");

                    b.Property<decimal>("Gia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenMonAn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DanhMucId");

                    b.ToTable("MonAns");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DanhMucId = 1,
                            Gia = 15000m,
                            MoTa = "Ngon",
                            TenMonAn = "Banh Mi"
                        },
                        new
                        {
                            Id = 2,
                            DanhMucId = 2,
                            Gia = 35000m,
                            MoTa = "Ngon",
                            TenMonAn = "Xôi"
                        },
                        new
                        {
                            Id = 3,
                            DanhMucId = 3,
                            Gia = 35000m,
                            MoTa = "Ngon",
                            TenMonAn = "Cơm xào bò"
                        },
                        new
                        {
                            Id = 4,
                            DanhMucId = 4,
                            Gia = 35000m,
                            MoTa = "Ngon",
                            TenMonAn = "Cơm gà"
                        });
                });

            modelBuilder.Entity("Web_Food_4TL.Models.NguoiDung", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoDienThoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenNguoiDung")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NguoiDungs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "@123",
                            MatKhau = "012",
                            SoDienThoai = "123",
                            TenNguoiDung = "H"
                        });
                });

            modelBuilder.Entity("Web_Food_4TL.Models.TinNhan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("LaTinNhanTuKhach")
                        .HasColumnType("bit");

                    b.Property<int?>("NguoiDungId")
                        .HasColumnType("int");

                    b.Property<int?>("NguoiGuiId")
                        .HasColumnType("int");

                    b.Property<int?>("NguoiNhanId")
                        .HasColumnType("int");

                    b.Property<string>("NoiDung")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ThoiGianGui")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("NguoiDungId");

                    b.HasIndex("NguoiGuiId");

                    b.HasIndex("NguoiNhanId");

                    b.ToTable("TinNhans");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.VaiTro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("TenVaiTro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VaiTros");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            TenVaiTro = "Khách Hàng"
                        },
                        new
                        {
                            Id = 2,
                            TenVaiTro = "Quản lý"
                        });
                });

            modelBuilder.Entity("Web_Food_4TL.Models.VaiTroNguoiDung", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("NguoiDungId")
                        .HasColumnType("int");

                    b.Property<int>("VaiTroId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NguoiDungId");

                    b.HasIndex("VaiTroId");

                    b.ToTable("VaiTroNguoiDungs");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.AnhMonAn", b =>
                {
                    b.HasOne("Web_Food_4TL.Models.MonAn", "MonAnh")
                        .WithMany("AnhMonAnh")
                        .HasForeignKey("MonAnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MonAnh");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.GioHang", b =>
                {
                    b.HasOne("Web_Food_4TL.Models.MonAn", "MonAn")
                        .WithMany()
                        .HasForeignKey("MonAnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web_Food_4TL.Models.NguoiDung", "NguoiDung")
                        .WithMany()
                        .HasForeignKey("NguoiDungId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MonAn");

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.HoaDon", b =>
                {
                    b.HasOne("Web_Food_4TL.Models.NguoiDung", "NguoiDung")
                        .WithMany()
                        .HasForeignKey("NguoiDungId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.HoaDonChiTiet", b =>
                {
                    b.HasOne("Web_Food_4TL.Models.HoaDon", "HoaDon")
                        .WithMany("HoaDonChiTiets")
                        .HasForeignKey("HoaDonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web_Food_4TL.Models.MonAn", "MonAn")
                        .WithMany()
                        .HasForeignKey("MonAnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HoaDon");

                    b.Navigation("MonAn");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.MonAn", b =>
                {
                    b.HasOne("Web_Food_4TL.Models.DanhMuc", "DanhMuc")
                        .WithMany()
                        .HasForeignKey("DanhMucId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DanhMuc");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.TinNhan", b =>
                {
                    b.HasOne("Web_Food_4TL.Models.NguoiDung", "NguoiDung")
                        .WithMany()
                        .HasForeignKey("NguoiDungId");

                    b.HasOne("Web_Food_4TL.Models.NguoiDung", "NguoiGui")
                        .WithMany()
                        .HasForeignKey("NguoiGuiId");

                    b.HasOne("Web_Food_4TL.Models.NguoiDung", "NguoiNhan")
                        .WithMany()
                        .HasForeignKey("NguoiNhanId");

                    b.Navigation("NguoiDung");

                    b.Navigation("NguoiGui");

                    b.Navigation("NguoiNhan");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.VaiTroNguoiDung", b =>
                {
                    b.HasOne("Web_Food_4TL.Models.NguoiDung", "NguoiDung")
                        .WithMany()
                        .HasForeignKey("NguoiDungId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web_Food_4TL.Models.VaiTro", "VaiTro")
                        .WithMany()
                        .HasForeignKey("VaiTroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");

                    b.Navigation("VaiTro");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.HoaDon", b =>
                {
                    b.Navigation("HoaDonChiTiets");
                });

            modelBuilder.Entity("Web_Food_4TL.Models.MonAn", b =>
                {
                    b.Navigation("AnhMonAnh");
                });
#pragma warning restore 612, 618
        }
    }
}
