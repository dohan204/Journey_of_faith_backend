using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.musics
{
    public class Song : AuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public int ArtistId { get; set; }
        public int? AlbumId { get; set; }
        public int? Duration { get; set; }
        public string? AudioUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Lyric { get; set; }
        public int? PlayCount { get; set; }
        public bool? IsActive { get; set; }

        private readonly List<ListeningHistory> _listeningHistories = new();
        private readonly List<PlaylistSong> _playlistSongs = new();
        private readonly List<UserFavoriteSong> _userFavoriteSongs = new();
        private readonly List<SongCategoryMapping> _categoryMappings = new();

        public IReadOnlyCollection<ListeningHistory> ListeningHistories => _listeningHistories.AsReadOnly();
        public IReadOnlyCollection<PlaylistSong> PlaylistSongs => _playlistSongs.AsReadOnly();
        public IReadOnlyCollection<UserFavoriteSong> UserFavoriteSongs => _userFavoriteSongs.AsReadOnly();
        public IReadOnlyCollection<SongCategoryMapping> CategoryMappings => _categoryMappings.AsReadOnly();
    }
}
