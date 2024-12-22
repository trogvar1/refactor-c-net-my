using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BankomatClassLibrary { 
    public class ATMContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AutomatedTellerMachine> ATMs { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankATM> BankATMs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(a => a.CardNumber);
            modelBuilder.Entity<AutomatedTellerMachine>().HasKey(atm => atm.AtmId);
            modelBuilder.Entity<Bank>().HasKey(b => b.BankName);

            modelBuilder.Entity<BankATM>().HasKey(ba => new { ba.BankName, ba.AtmId });

            modelBuilder.Entity<BankATM>().HasOne(ba => ba.Bank).WithMany(b => b.BankATMs).HasForeignKey(ba => ba.BankName);

            modelBuilder.Entity<BankATM>().HasOne(ba => ba.ATM).WithMany(atm => atm.BankATMs).HasForeignKey(ba => ba.AtmId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            var connectionString = configuration.GetConnectionString("ATMDatabase");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}