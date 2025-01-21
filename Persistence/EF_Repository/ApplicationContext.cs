using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EF_Repository
{
    public class ApplicationContext : DbContext
    {

        public DbSet<User> Users {  get; set; }
        public DbSet<ClientApplication> ClientApplications { get; set; }
        public DbSet<RedirectUrl> RedirectUrls { get; set; }
        public DbSet<ApplicationScope> ApplicationScopes {  get; set; }
        public DbSet<Scope> Scopes {  get; set; }

        public ApplicationContext(){
        Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=xlitte_service;Username=xlitte;Password=Dsbuhsdf.gentdre1");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(entity => entity.Id);

            modelBuilder.Entity<Scope>()
                .ToTable("Scopes")
                .HasKey(entity => entity.Id);

            modelBuilder.Entity<Scope>()
                .HasAlternateKey(e => e.Name);

            modelBuilder.Entity<RedirectUrl>()
                .ToTable("ApplicationRedirectUrl");

            modelBuilder.Entity<ApplicationScope>()
                .ToTable("ApplicationScope");

            modelBuilder.Entity<RedirectUrl>()
                .HasOne(ru => ru.ClientApplication)
                .WithMany(ca => ca.RedirectUrls)
                .HasForeignKey(ru => ru.ClientId);

            // Настройка отношений между ClientApplication и Scope через ApplicationScope
            modelBuilder.Entity<ApplicationScope>()
                .HasOne(a => a.ClientApplication)
                .WithMany(c => c.ApplicationScopes) // Здесь нужно добавить свойство в ClientApplication
                .HasForeignKey(a => a.ClientId);

            modelBuilder.Entity<ApplicationScope>()
                .HasOne(a => a.Scope)
                .WithMany()
                .HasForeignKey(a => a.ScopeId);

            // Настройка уникальности для Client_Id в ClientApplication
            modelBuilder.Entity<ClientApplication>()
                .HasIndex(ca => ca.Client_Id)
                .IsUnique();

            modelBuilder.Entity<ClientApplication>()
                .HasKey(e => e.Client_Id);


            modelBuilder.Entity<ClientApplication>()
                .Property(entity => entity.Client_Id)
                .ValueGeneratedNever();
        }
    }
}
