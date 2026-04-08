using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.music
{
    public class Playlist
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? CreatedTime { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = [];
    }
}
