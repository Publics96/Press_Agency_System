using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Press_Agency_System.Observer;

namespace Press_Agency_System.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

            [Required]
            [MaxLength(50)]
            public string FirstName { get; set; }

            [Required]
            [MaxLength(50)]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            public string Phone { get; set; }

            public string PhotoPath { get; set; }
            public IdentityRole roleType { get; set; }
            public bool activeUser { get; set; }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext()
                : base("DefaultConnection")
            {
            }
            public DbSet<Post> Posts { get; set; }

            public DbSet<SavedPosts> SavedPosts { get; set; }

            public DbSet<InteractedPosts> IneractedPosts { get; set; }

            public DbSet<Questions> Questions { get; set; }
            public DbSet<Message> Message { get; set; }
            public DbSet<UserConnection> UserConnections { get; set; }
            public object Messages { get; internal set; }

            public DbSet<Observers> observers { get; set; }
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<InteractedPosts>().HasKey(c => new { c.UserId, c.PostId });
                modelBuilder.Entity<SavedPosts>().HasKey(c => new { c.UserId, c.PostId });
            }

        }
    }