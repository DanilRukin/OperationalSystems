namespace Messanger.Server.Model
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public User Reciever { get; set; }
        public Guid RecieverId { get; set; }
        public User Sender { get; set; }
        public Guid SenderId { get; set; }
        public bool WasSended { get; set; }
        public DateTime DateSended { get; set; }
    }
}
