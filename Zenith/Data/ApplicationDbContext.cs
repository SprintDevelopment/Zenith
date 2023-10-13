using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Buy> Buys { get; set; }
        public DbSet<BuyItem> BuyItems { get; set; }
        public DbSet<Cash> Cashes { get; set; }
        public DbSet<Cheque> Cheques { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineOutgo> MachineOutgoes { get; set; }
        public DbSet<MachineIncome> MachineIncomes { get; set; }
        //public DbSet<MaterialAvailability> MaterialAvailabilities { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Mixture> Mixtures { get; set; }
        public DbSet<MixtureItem> MixtureItems { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Outgo> Outgoes { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<OutgoCategory> OutgoCategories { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<SalaryPayment> SalaryPayments { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Username = "admin",
                    Password = "1234",
                    CreateDateTime = DateTime.Today,
                }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = 1,
                    Name = "Workshop Account",
                    CostCenter = CostCenters.Workshop,
                    Balance = 0,
                    CreditValue = 0,
                    Comment = string.Empty
                },
                new Account
                {
                    AccountId = 2,
                    Name = "Transportation Account",
                    CostCenter = CostCenters.Transportation,
                    Balance = 0,
                    CreditValue = 0,
                    Comment = string.Empty
                },
                new Account
                {
                    AccountId = 3,
                    Name = "Consumables Account",
                    CostCenter = CostCenters.Consumables,
                    Balance = 0,
                    CreditValue = 0,
                    Comment = string.Empty
                });

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Site)
                .WithMany(s => s.Deliveries)
                .HasForeignKey(d => d.SiteId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
