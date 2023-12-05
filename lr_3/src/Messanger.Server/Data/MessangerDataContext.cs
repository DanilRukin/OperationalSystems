using Messanger.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Server.Data
{
    public class MessangerDataContext : DbContext
    {
        public MessangerDataContext(DbContextOptions<MessangerDataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
