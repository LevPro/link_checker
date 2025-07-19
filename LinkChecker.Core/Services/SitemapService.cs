using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinkChecker.Core.Services
{
    public class SitemapService
    {
        public List<string> LoadLinksFromLocalSitemap(string path)
        {
            var xml = File.ReadAllText(path);
            return ParseSitemap(xml);
        }

        public async Task<List<string>> LoadLinksFromRemoteSitemapAsync(string url)
        {
            using var client = new HttpClient();
            var xml = await client.GetStringAsync(url);
            return ParseSitemap(xml);
        }

        public List<string> ParseSitemap(string xmlContent)
        {
            var result = new List<string>();
            var doc = XDocument.Parse(xmlContent);

            foreach (var loc in doc.Descendants("{http://www.sitemaps.org/schemas/sitemap/0.9}loc"))
                result.Add(loc.Value);

            return result;
        }
    }
}