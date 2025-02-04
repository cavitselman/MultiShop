using Microsoft.EntityFrameworkCore;
using MS.Message.DAL.Entities;

namespace MS.Message.DAL.Context
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options) : base(options)
        {

        }
        public DbSet<UserMessage> UserMessages { get; set; }
    }
}
