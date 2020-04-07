using GroupExpenseManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupExpenseManager.API.Database.Context
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {  }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>()
                        .HasOne(ug=>ug.User)
                        .WithMany(u=>u.UserGroups)
                        .HasForeignKey(ug=>ug.UserId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserGroup>()
                        .HasOne(ug=>ug.Group)
                        .WithMany(g=>g.UserGroups)
                        .HasForeignKey(ug=>ug.GroupId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasAlternateKey(u=>u.UserName);
        }
    }
}