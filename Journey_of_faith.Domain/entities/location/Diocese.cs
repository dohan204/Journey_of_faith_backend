using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.location
{
    public class Diocese : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? Address { get; set; }
        public string? Thumbnail { get; set; }
    }
}
