namespace MS.WebUI.Models
{
    public class EmailRequest
    {
        public string ReceiverMail { get; set; }
        public string Subject { get; set; }
        public string MessageContent { get; set; }
    }
}
