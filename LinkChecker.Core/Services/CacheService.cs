using LinkChecker.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LinkChecker.Core.Services
{
    public class CacheService
    {
        private string GetSiteDirectory(string siteHost)
        {
            var dir = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
                "LinkChecker",
                siteHost);
            Directory.CreateDirectory(dir);
            return dir;
        }

        public void SaveLink(Link link)
        {
            var dir = GetSiteDirectory(link.Site);
            var file = Path.Combine(dir, $"{SafeFileName(link.Url)}.json");
            File.WriteAllText(file, JsonSerializer.Serialize(link));
        }

        public List<Link> LoadLinks(string siteHost)
        {
            var dir = GetSiteDirectory(siteHost);
            var result = new List<Link>();
            foreach (var file in Directory.EnumerateFiles(dir, "*.json"))
            {
                var json = File.ReadAllText(file);
                result.Add(JsonSerializer.Deserialize<Link>(json));
            }
            return result;
        }

        public void DeleteAllLinks(string siteHost)
        {
            var dir = GetSiteDirectory(siteHost);
            foreach (var file in Directory.EnumerateFiles(dir, "*.json"))
                File.Delete(file);
        }

        public void DeleteLink(Link link)
        {
            var dir = GetSiteDirectory(link.Site);
            var file = Path.Combine(dir, $"{SafeFileName(link.Url)}.json");
            if (File.Exists(file)) File.Delete(file);
        }

        private string SafeFileName(string url) =>
            System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(url));
    }
}