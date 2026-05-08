using HtmlAgilityPack;
using System.Xml;

namespace Api.Services.Implements;

public class Article
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? Thumbnail { get; set; }
}

public class VNExpressCrawler
{
    private readonly HttpClient _httpClient;

    public VNExpressCrawler()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
    }

    public async Task<List<Article>> CrawlAIArticles()
    {
        var articles = new List<Article>();

        string url = "https://vnexpress.net/khoa-hoc-cong-nghe/ai";

        var html = await _httpClient.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Selector hiện tại của VNExpress
        var nodes = doc.DocumentNode.SelectNodes("//article[contains(@class,'item-news')]");

        if (nodes == null)
            return articles;

        foreach (var node in nodes)
        {
            try
            {
                var titleNode = node.SelectSingleNode(".//h3[contains(@class,'title-news')]/a");
                var descNode = node.SelectSingleNode(".//p[contains(@class,'description')]/a");
                var imgNode = node.SelectSingleNode(".//img");

                if (titleNode == null)
                    continue;

                var article = new Article
                {
                    Title = HtmlEntity.DeEntitize(titleNode.InnerText.Trim()),
                    Url = titleNode.GetAttributeValue("href", ""),
                    Description = descNode?.InnerText.Trim() ?? "",
                    Thumbnail = imgNode?.GetAttributeValue("data-src", "")
                                ?? imgNode?.GetAttributeValue("src", "")
                                ?? ""
                };

                articles.Add(article);
            }
            catch
            {
                // bỏ qua article lỗi. Đời crawler vốn khổ như dev maintain legacy code 😑
            }
        }

        return articles;
    }


}

public class TestCrawl
{
    public static async Task Test()
    {
        var crawler = new VNExpressCrawler();

        var articles = await crawler.CrawlAIArticles();

        foreach (var article in articles)
        {
            Console.WriteLine($"Title: {article.Title}");
            Console.WriteLine($"Desc : {article.Description}");
            Console.WriteLine($"Url  : {article.Url}");
            Console.WriteLine($"Image: {article.Thumbnail}");
            Console.WriteLine(new string('-', 80));
        }

    }
}