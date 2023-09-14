namespace Acacia_Back_End.Core.Models
{
    public class Media:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MediaType Type { get; set; } = MediaType.Video;
        public string FileUrl { get; set; }
    }
}
