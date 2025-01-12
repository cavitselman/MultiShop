using Microsoft.EntityFrameworkCore;
using MS.Comment.Entities;

namespace MS.Comment.Context
{
    public class CommentContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1436;Initial Catalog=MultiShopCommentDb;User Id=sa;Password=123456aA*;TrustServerCertificate=True");
        }

        public DbSet<UserComment> UserComments { get; set; }
    }
}
