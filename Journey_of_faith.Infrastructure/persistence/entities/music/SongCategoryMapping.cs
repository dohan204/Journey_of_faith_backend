using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.music
{
    public class SongCategoryMapping
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public int CategoryId { get; set; }

        public Song Song { get; set; } = null!;
        public SongCategory Category { get; set; } = null!;
    }
}
