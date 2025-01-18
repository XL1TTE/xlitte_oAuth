﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence.EF_Repository;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20250118100223_Migration_5")]
    partial class Migration_5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.ApplicationScope", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ScopeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ScopeId");

                    b.ToTable("ApplicationScope", (string)null);
                });

            modelBuilder.Entity("Domain.ClientApplication", b =>
                {
                    b.Property<string>("Client_Id")
                        .HasColumnType("text");

                    b.Property<string>("ApplicationDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ApplicationName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ApplicationPrivacyPolicy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Client_Secret")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HomeUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Client_Id");

                    b.HasIndex("Client_Id")
                        .IsUnique();

                    b.ToTable("ClientApplications");
                });

            modelBuilder.Entity("Domain.RedirectUrl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("ApplicationRedirectUrl", (string)null);
                });

            modelBuilder.Entity("Domain.Scope", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Scopes", (string)null);
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Domain.ApplicationScope", b =>
                {
                    b.HasOne("Domain.ClientApplication", "ClientApplication")
                        .WithMany("ApplicationScopes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Scope", "Scope")
                        .WithMany("ApplicationScopes")
                        .HasForeignKey("ScopeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientApplication");

                    b.Navigation("Scope");
                });

            modelBuilder.Entity("Domain.RedirectUrl", b =>
                {
                    b.HasOne("Domain.ClientApplication", "ClientApplication")
                        .WithMany("RedirectUrls")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientApplication");
                });

            modelBuilder.Entity("Domain.ClientApplication", b =>
                {
                    b.Navigation("ApplicationScopes");

                    b.Navigation("RedirectUrls");
                });

            modelBuilder.Entity("Domain.Scope", b =>
                {
                    b.Navigation("ApplicationScopes");
                });
#pragma warning restore 612, 618
        }
    }
}
