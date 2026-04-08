using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.musics
{
    public class SongCategoryMapping
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public int CategoryId { get; set; }
    }
}
