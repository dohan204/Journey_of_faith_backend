using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.music
{
    public class ListeningHistory
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int SongId { get; set; }
        public DateTime? ListenTime { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Song Song { get; set; } = null!;
    }
}
