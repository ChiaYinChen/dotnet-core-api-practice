namespace WebApiApp.Models
{
    public class EmailSettings
    {
        public required string HOST { get; set; }
        public required int PORT { get; set; }
        public required string USERNAME { get; set; }
        public required string PASSWORD { get; set; }
        public required string FROM_EMAIL { get; set; }
        public required string EMAIL_SENDER { get; set; }
        public required bool ENABLE_SSL { get; set; }
    }
}
