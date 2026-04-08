using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.music
{
    public class UserFavoriteSong
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int SongId { get; set; }
        public DateTime? CreatedTime { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Song Song { get; set; } = null!;
    }
}
