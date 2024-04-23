namespace backend.Entities
{
    public class EmailModel
    {
        public string FromEmail { get; set; } = string.Empty;
        public string ToEmails { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
