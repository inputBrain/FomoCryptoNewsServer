using HtmlAgilityPack;

namespace ConsoleApp1;

class NewsItem
{
    public string Title { get; set; }
    public string ImageSrc { get; set; }
    public string Content { get; set; }
}

class Program
{
    private static readonly HttpClient client = new();

    static async Task Main(string[] args)
    {
        var baseUrl = "https://cryptoslate.com";
        var maxPages = 1;

        for (var page = 1; page <= maxPages; page++)
        {
            var url = page == 1 ? $"{baseUrl}/news/" : $"{baseUrl}/news/{page}/";
            Console.WriteLine($"Start process for link: {url}");

            var html = await client.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var newsFeed = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='list-feed slate']");
            if (newsFeed == null)
            {
                Console.WriteLine("Complete");
                break;
            }

            var listPosts = newsFeed.SelectNodes(".//div[@class='list-post']");
            if (listPosts == null || listPosts.Count == 0)
            {
                Console.WriteLine("Complete");
                break;
            }

            foreach (var post in listPosts)
            {
                var linkNode = post.SelectSingleNode(".//a");
                if (linkNode != null)
                {
                    var postUrl = linkNode.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(postUrl))
                    {
                        if (!postUrl.StartsWith("http"))
                        {
                            postUrl = baseUrl + postUrl;
                        }
                        await ProcessPostPage(postUrl);
                    }
                }
            }
        }
    }

    static async Task ProcessPostPage(string url)
    {
        Console.WriteLine($"url: {url}");

        var html = await client.GetStringAsync(url);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var newsItem = new NewsItem();

        var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='post-title ']");
        if (titleNode != null)
        {
            newsItem.Title = titleNode.InnerText.Trim();
        }

        var imageNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='cover']//img[@class=' lazyloaded']");
        if (imageNode != null)
        {
            newsItem.ImageSrc = imageNode.GetAttributeValue("src", ""); 
        }

        var articleNode = htmlDocument.DocumentNode.SelectSingleNode("//article[@class='full-article']");
        if (articleNode != null)
        {
            var paragraphs = articleNode.SelectNodes(".//p");
            if (paragraphs != null)
            {
                newsItem.Content = string.Join("\n", paragraphs.Select(p => p.InnerText.Trim()));
            }
        }
        Console.WriteLine($"Title: {newsItem.Title}");
        Console.WriteLine($"ImageSrc: {newsItem.ImageSrc}");
        Console.WriteLine($"Content: {newsItem.Content?.Substring(0, Math.Min(newsItem.Content.Length, 100))}...");
        Console.WriteLine();
    }
}