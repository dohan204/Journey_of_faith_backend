using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.music
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ArtistId { get; set; }
        public int? AlbumId { get; set; }
        public int? Duration { get; set; }
        public string? AudioUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Lyric { get; set; }
        public int? PlayCount { get; set; }
        public bool? IsActive { get; set; }

        public Guid? CreatorUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public Artist Artist { get; set; } = null!;
        public Album? Album { get; set; }
        public ICollection<SongCategoryMapping> CategoryMappings { get; set; } = [];
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = [];
        public ICollection<UserFavoriteSong> FavoritedBy { get; set; } = [];
        public ICollection<ListeningHistory> ListeningHistories { get; set; } = [];
    }
}
