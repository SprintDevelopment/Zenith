using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }

        public DbSet<Buy> Buys { get; set; }
        public DbSet<BuyItem> BuyItems { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Outgo> Outgoes { get; set; }
        public DbSet<OutgoCategory> OutgoCategories { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
