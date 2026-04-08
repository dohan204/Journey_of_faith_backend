using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.music
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ArtistId { get; set; }
        public int? ReleaseYear { get; set; }
        public string? CoverImageUrl { get; set; }

        public Artist Artist { get; set; } = null!;
        public ICollection<Song> Songs { get; set; } = [];
    }
}
