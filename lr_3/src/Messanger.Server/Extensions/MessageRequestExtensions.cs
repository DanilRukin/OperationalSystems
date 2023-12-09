using Messanger.Server.Model;

namespace Messanger.Server.Extensions
{
    public static class MessageRequestExtensions
    {
        public static Message ToMessage(this MessageRequest messageRequest)
        {
            return new Message()
            {
                Id = Guid.NewGuid(),
                RecieverId = Guid.Parse(messageRequest.SourceAddress),
                SenderId = Guid.Parse(messageRequest.DestinationAddress),
                Text = messageRequest.Message,
                WasSended = false,
                DateSended = DateTime.UtcNow
            };
        }
    }
}
