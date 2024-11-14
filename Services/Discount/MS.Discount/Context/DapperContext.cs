using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MS.Discount.Entities;
using System.Data;

namespace MS.Discount.Context
{
    public class DapperContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;initial Catalog=MultiShopDiscountDb;integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true");
        }
        public DbSet<Coupon> Coupones { get; set; }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
