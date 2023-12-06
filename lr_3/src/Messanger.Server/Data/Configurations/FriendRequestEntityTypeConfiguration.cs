using Messanger.Server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messanger.Server.Data.Configurations
{
    public class FriendRequestEntityTypeConfiguration : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder.HasKey(fr => new { fr.RequestSenderId, fr.RequestRecieverId });
        }
    }
}
