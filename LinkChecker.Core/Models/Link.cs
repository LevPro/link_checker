using System;

namespace LinkChecker.Core.Models
{
    public class Link
    {
        public string Url { get; set; }
        public string Site { get; set; }
        public int? ResponseCode { get; set; } = 200;
        public string ResponseDescription { get; set; }
        public DateTime? LastChecked { get; set; }
        public bool IsSelected { get; set; }
    }
}