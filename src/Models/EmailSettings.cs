namespace WebApiApp.Models
{
    public class EmailSettings
    {
        public string HOST { get; set; }
        public int PORT { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string FROM_EMAIL { get; set; }
        public string EMAIL_SENDER { get; set; }
        public bool ENABLE_SSL { get; set; }
    }
}
