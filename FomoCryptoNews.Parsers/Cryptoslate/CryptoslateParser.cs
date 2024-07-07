using FomoCryptoNews.Api.Service;
using FomoCryptoNews.ExternalDto.Cryptoslate;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.Parsers.Cryptoslate;

public class CryptoslateParser : ICryptoslateParser
{
    private const string BaseUrl = "https://cryptoslate.com";

    private readonly IBaseParser _baseParser;

    private readonly ILogger<CryptoslateParser> _logger;


    public CryptoslateParser(ILogger<CryptoslateParser> logger, IBaseParser baseParser)
    {
        _logger = logger;
        _baseParser = baseParser;
    }


    public async Task<List<CryptoslateNewsDto>> ParseNewsByAmountOfPages(int pageParseCount, CancellationToken cancellationToken)
    {
        var collection = new List<CryptoslateNewsDto>();

        for (var page = 1; page <= pageParseCount; page++)
        {
            var url = page == 1 ? $"{BaseUrl}/news/" : $"{BaseUrl}/news/page/{page}/";

            _logger.LogDebug("\t--- Starting process for url: {Url} ---", url);

            var html = await _baseParser.GetWebsiteStringAsync(url, cancellationToken);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var newsFeed = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='list-feed slate']");
            if (newsFeed == null)
            {
                _logger.LogDebug("Complete");
                break;
            }

            var listPosts = newsFeed.SelectNodes(".//div[@class='list-post']");
            if (listPosts == null || listPosts.Count == 0)
            {
                _logger.LogDebug("Complete");
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
                            postUrl = BaseUrl + postUrl;
                        }
                        var newsDto = await ProcessPostPage(postUrl, cancellationToken);

                        collection.Add(newsDto);

                    }
                }
            }
        }

        return collection.ToList();
    }


    public async Task<CryptoslateNewsDto> ProcessPostPage(string url, CancellationToken cancellationToken)
    {
        _logger.LogDebug("\nProcess a post: {Url}", url);

        var html = await _baseParser.GetWebsiteStringAsync(url, cancellationToken);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var newsItem = new CryptoslateNewsDto();

        var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='post-title ']");
        if (titleNode != null)
        {
            newsItem.Title = titleNode.InnerText.Trim();
        }

        var imageNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='cover']//img[@class=' lazyloaded']");
        if (imageNode != null)
        {
            newsItem.Cover = imageNode.GetAttributeValue("src", "");
        }

        var articleNode = htmlDocument.DocumentNode.SelectSingleNode("//article[@class='full-article']");
        if (articleNode != null)
        {
            var paragraphs = articleNode.SelectNodes(".//p");
            if (paragraphs != null)
            {
                newsItem.Description = string.Join("\n", paragraphs.Select(p => p.InnerText.Trim()));
            }
        }

        return newsItem;
    }
}