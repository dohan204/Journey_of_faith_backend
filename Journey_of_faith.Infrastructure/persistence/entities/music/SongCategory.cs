using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.music
{
    public class SongCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public ICollection<SongCategoryMapping> SongMappings { get; set; } = [];
    }
}
