using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.musics
{
    public class ListeningHistory
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int SongId { get; set; }
        public DateTime? ListenTime { get; set; }
    }
}
