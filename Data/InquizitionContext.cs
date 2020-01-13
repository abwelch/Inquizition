using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inquizition.Models;

namespace Inquizition.Data
{
    public class InquizitionContext : IdentityDbContext<IdentityUser>
    {
        public InquizitionContext(DbContextOptions<InquizitionContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<IdentityUser> AspNetUsers { get; set; }
        public DbSet<UserInfo> UserOverviewInfo { get; set; }
        public DbSet<FlashCardEntry> FlashCards { get; set; }
    }
}
