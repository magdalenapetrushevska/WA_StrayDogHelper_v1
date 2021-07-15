using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WA_StrayDogHelper_v1.Models.DomainModels;
using WA_StrayDogHelper_v1.Models.IdentityModels;

namespace WA_StrayDogHelper_v1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dog> Dogs { get; set; }
        public DbSet<FavoriteDog> FavoriteDogs { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<FavoriteArticle> FavoriteArticles { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<RequestDonationMoney> RequestDonationMoney { get; set; }
        public DbSet<MakeDonationMoney> MakeDonationMoney { get; set; }
        public DbSet<RequestDonationFood> RequestDonationFood { get; set; }
        public DbSet<Message> Messages { get; set; }
        public object Message { get; internal set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
               .HasMany(c => c.Dogs)
               .WithOne(e => e.User);

            builder.Entity<ApplicationUser>()
                .HasMany(c => c.FavoriteDogs)
                .WithOne(c => c.User);

            builder.Entity<ApplicationUser>()
               .HasMany(c => c.FavoriteArticles)
               .WithOne(c => c.User);

            builder.Entity<Question>()
                .HasMany(m => m.Replies)
                .WithOne(m => m.Question);

            builder.Entity<ApplicationUser>()
                .HasMany(m => m.RequestDonationMoney)
                .WithOne(m => m.User);

            builder.Entity<ApplicationUser>()
                .HasMany(m => m.RequestDonationFood)
                .WithOne(m => m.User);





        }

    }
}
