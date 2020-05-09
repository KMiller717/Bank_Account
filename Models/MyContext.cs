using Microsoft.EntityFrameworkCore;

namespace Bank_Account.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;}

        public DbSet<Account> Accounts {get;set;}

        
    }
}