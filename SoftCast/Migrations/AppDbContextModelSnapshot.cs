﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoftCast.Data;

#nullable disable

namespace SoftCast.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SoftCast.Models.Conteudo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("ClassificacaoIndicativa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CriadorID")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlaylistID")
                        .HasColumnType("int");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VideoPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CriadorID");

                    b.HasIndex("PlaylistID");

                    b.ToTable("Conteudos");
                });

            modelBuilder.Entity("SoftCast.Models.Criador", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Criadores");
                });

            modelBuilder.Entity("SoftCast.Models.ItemPlaylist", b =>
                {
                    b.Property<int>("PlaylistID")
                        .HasColumnType("int");

                    b.Property<int>("ConteudoID")
                        .HasColumnType("int");

                    b.HasKey("PlaylistID", "ConteudoID");

                    b.HasIndex("ConteudoID");

                    b.ToTable("ItensPlaylist");
                });

            modelBuilder.Entity("SoftCast.Models.Playlist", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UsuarioID");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("SoftCast.Models.Usuario", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DtNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("SoftCast.Models.Conteudo", b =>
                {
                    b.HasOne("SoftCast.Models.Criador", "Criador")
                        .WithMany("Conteudos")
                        .HasForeignKey("CriadorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SoftCast.Models.Playlist", null)
                        .WithMany("Conteudos")
                        .HasForeignKey("PlaylistID");

                    b.Navigation("Criador");
                });

            modelBuilder.Entity("SoftCast.Models.ItemPlaylist", b =>
                {
                    b.HasOne("SoftCast.Models.Conteudo", "Conteudo")
                        .WithMany()
                        .HasForeignKey("ConteudoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SoftCast.Models.Playlist", "Playlist")
                        .WithMany()
                        .HasForeignKey("PlaylistID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conteudo");

                    b.Navigation("Playlist");
                });

            modelBuilder.Entity("SoftCast.Models.Playlist", b =>
                {
                    b.HasOne("SoftCast.Models.Usuario", "Usuario")
                        .WithMany("Playlists")
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("SoftCast.Models.Criador", b =>
                {
                    b.Navigation("Conteudos");
                });

            modelBuilder.Entity("SoftCast.Models.Playlist", b =>
                {
                    b.Navigation("Conteudos");
                });

            modelBuilder.Entity("SoftCast.Models.Usuario", b =>
                {
                    b.Navigation("Playlists");
                });
#pragma warning restore 612, 618
        }
    }
}
