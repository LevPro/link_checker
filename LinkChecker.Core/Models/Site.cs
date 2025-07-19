using System.Collections.Generic;

namespace LinkChecker.Core.Models
{
    public class Site
    {
        public string Host { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
    }
}