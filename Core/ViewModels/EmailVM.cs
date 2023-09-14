namespace Acacia_Back_End.Core.ViewModels
{
    public class EmailVM
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        //public List<IFormFile> Attachments { get; set; }
    }
}
