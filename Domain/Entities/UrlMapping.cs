using System;

namespace Domain.Entities
{
    public class UrlMapping
    {
        public Guid Id { get; set; }
        public string LongUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessedAt { get; set; }
        public int ClickCount { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
