using LinkChecker.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace LinkChecker.Core.Services
{
    public class LinkService
    {
        public List<string> ParseLinks(string input)
        {
            var separators = new[] { "\r\n", "\n", ",", ";" };
            return input.Split(separators, System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(u => u.Trim())
                        .Where(u => u.StartsWith("http"))
                        .Distinct()
                        .ToList();
        }

        public List<Site> GroupLinksBySite(IEnumerable<string> urls)
        {
            return urls.GroupBy(url => new System.Uri(url).Host)
                       .Select(g => new Site { Host = g.Key, Links = g.Select(u => new Link { Url = u, Site = g.Key }).ToList() })
                       .ToList();
        }

        public Link FindLink(IEnumerable<Site> sites, string search)
        {
            foreach (var site in sites)
            {
                var found = site.Links.FirstOrDefault(l => l.Url.Contains(search));
                if (found != null) return found;
            }
            return null;
        }
    }
}