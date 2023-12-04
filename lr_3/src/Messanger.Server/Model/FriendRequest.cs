namespace Messanger.Server.Model
{
    public class FriendRequest
    {
        public User RequestSender { get; set; }
        public Guid RequestSenderId { get; set; }

        public User RequestReciever { get; set; }
        public Guid RequestRecieverId { get; set; }

        public bool Accepted { get; set; }
    }
}
