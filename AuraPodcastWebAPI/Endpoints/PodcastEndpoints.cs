using AuraPodcastWebAPI.Models;
using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;

namespace AuraPodcastWebAPI.Endpoints
{
    public static class PodcastEndpoints
    {
        public static void MapPodcastEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/podcast", static async (string? url) =>
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return Results.BadRequest("URL parameter is required.");
                }

                try
                {
                    var feed = await FeedReader.ReadAsync(url);
                    var episodes = feed.Items.Select(item =>
                    {
                        // Cast the specific item to Rss20FeedItem to access Enclosure
                        var rssItem = item.SpecificItem as MediaRssFeedItem;

                        return new PodcastEpisode
                        {
                            Title = item.Title,
                            Summary = item.Description,
                            PublishingDate = item.PublishingDate,
                            AudioUrl = rssItem?.Enclosure?.Url
                        };
                    }).ToList();

                    return Results.Ok(episodes);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error fetching or parsing feed: {ex.Message}");
                }
            });
        }
    }
}
