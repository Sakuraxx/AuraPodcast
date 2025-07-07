namespace AuraPodcastWebAPI.Models
{
    public class PodcastEpisode
    {
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public DateTime? PublishingDate { get; set; }
        public string? AudioUrl { get; set; }
    }
}
