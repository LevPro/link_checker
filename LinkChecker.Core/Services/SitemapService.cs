using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

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
            using var client = new HttpClient() ;
            var xml = await client.GetStringAsync(url);
            return ParseSitemap(xml);
        }

        public List<string> ParseSitemap(string xmlContent)
        {
            var result = new List<string>();
            var xml = new XmlDocument();
            xml.LoadXml(xmlContent);

            XmlNodeList urlNodes = xml.GetElementsByTagName("url");
            if (urlNodes.Count > 0)
            {
                foreach (XmlNode urlNode in urlNodes)
                {
                    var locNode = urlNode["loc"];
                    if (locNode != null && !string.IsNullOrWhiteSpace(locNode.InnerText))
                        result.Add(locNode.InnerText.Trim());
                }
            }
            else // SitemapIndex (могут быть вложенные sitemap'ы)
            {
                XmlNodeList sitemapNodes = xml.GetElementsByTagName("sitemap");
                foreach (XmlNode sitemapNode in sitemapNodes)
                {
                    var locNode = sitemapNode["loc"];
                    if (locNode != null && !string.IsNullOrWhiteSpace(locNode.InnerText))
                        result.Add(locNode.InnerText.Trim());
                }
            }

            return result;
        }
    }
}