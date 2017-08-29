namespace BuisenessLogicLayer.Models
{
    public class MailServerInfo
    {
        public string Name { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool EnableSsl { get; set; }
    }
}
