using Messanger.Server.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Messanger.Server.Data
{
    public class MessangerDataContext : DbContext
    {
        public MessangerDataContext(DbContextOptions<MessangerDataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
