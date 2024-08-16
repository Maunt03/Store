using Store.Models;
using System;
using System.Data.Entity;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
namespace Store
{
    public class AppDbContext : DbContext
    {
        private static string _dbConnectionString = "Data Source=scp.realhost.com.ua;Initial Catalog=store03;User ID = maunt;Password=12345;Connect Timeout=30;Encrypt=False;";
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Models.Image> Images { get; set; }
        public AppDbContext() : base(_dbConnectionString)
        {
        }
    }
}
