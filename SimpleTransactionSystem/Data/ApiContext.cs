using Microsoft.EntityFrameworkCore;
using SimpleTransactionSystem.Models;

namespace SimpleTransactionSystem.Data
{
    public class ApiContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>()
           .HasDiscriminator<string>("AccountType")
           .HasValue<SavingsAccount>("Savings")
           .HasValue<ChequingAccount>("Chequing");

            base.OnModelCreating(builder);
        }
    }
}
