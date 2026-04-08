using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.location
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int LevelId { get; set; }
        public string? Thumbnail { get; set; }
        public string? Address { get; set; }

        public SchoolLevel Level { get; set; } = null!;
        public ICollection<ApplicationUser> Users { get; set; } = [];
    }
}
