using Microsoft.EntityFrameworkCore;
using MS.Comment.Entities;

namespace MS.Comment.Context
{
    public class CommentContext : DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options)
        {
        }

        public DbSet<UserComment> UserComments { get; set; }
    }

}
