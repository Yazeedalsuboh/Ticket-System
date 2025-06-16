using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SupportTicketingData.Entities;
using SupportTicketingData.Enums;
using SupportTicketingData.Interface;

namespace SupportTicketingData.Data
{
    public class AppDbContext : DbContext
    {
       
        public DbSet<Ticket> Ticket => Set<Ticket>();
        public DbSet<Attachment> Attachment => Set<Attachment>();
        public DbSet<Comment> Comment => Set<Comment>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Users> Users => Set<Users>();
        public DbSet<VerificationCode> VerificationCode => Set<VerificationCode>();
        public DbSet<Notification> Notifications => Set<Notification>();

        private readonly ICurrentUserRepo _currentUserRepository;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserRepo currentUserRepository)
        : base(options)
        {
            _currentUserRepository = currentUserRepository;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is EntityBase &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            var userId = _currentUserRepository.GetUserId();


            foreach (var entry in entries)
            {
                var entity = (EntityBase)entry.Entity;
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                    entity.CreatedBy = userId;
                }
                if ((entry.State == EntityState.Modified))
                {
                    entity.ModifiedAt = now;
                    entity.ModifiedBy = userId;
                }

            }

            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relation for Ticket
            modelBuilder.Entity<Ticket>().HasOne(t => t.Client)
                .WithMany(u => u.ClientTickets).HasForeignKey(t => t.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Ticket>().HasOne(t => t.Support)
               .WithMany(u => u.SupportTickets).HasForeignKey(t => t.SupportId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>().HasOne(c => c.User)
                .WithMany(u => u.Comments).HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Users>().HasData
           (
                new Users
                {
                    Id = 1,
                    Role=UserRole.Manager,
                    FullName ="System Manager",
                    Image="",
                    PasswordHash = "AQAAAAIAAYagAAAAEAhi7hQORBl+2d6gMU8qo/" +
                    "gWO+Tx+qxdjnCqyAqXqJdxFJNfgNcMj2Wolf374xoaPA==",//1234Sys@
                    Mobile ="0779882931",
                    Email="SysMan@gmail.com",
                    Address="Jordan-Irbid",
                    DateOfBirth= new DateTime(1999,7,30),
                    IsActive=true,
                    CreatedAt= new DateTime(2025,5,6),
                    CreatedBy = 0
                }              
           );

            modelBuilder.Entity<Product>().HasData
                (
                   new Product
                   {
                       Id = 1,
                       Name= "Legends of Valor",
                       Description = "Multiplayer online battle arena " +
                       "(MOBA) game with ranked matchmaking and team strategy.",
                       Category = "Action/Strategy",
                       CreatedAt = new DateTime(2025, 5, 6),
                       CreatedBy = 0
                   },
                   new Product
                   {
                       Id = 2,
                       Name = "Fantasy Quest Online",
                       Description = "Massively multiplayer online role-playing game" +
                       " (MMORPG) with expansive world and character progression.",
                       Category = "RPG",
                       CreatedAt = new DateTime(2025, 5, 6),
                       CreatedBy = 0
                   }
                );
        }
    }
}
