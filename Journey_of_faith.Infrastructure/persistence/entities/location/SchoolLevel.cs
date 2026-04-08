using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.location
{
    public class SchoolLevel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<School> Schools { get; set; } = [];
    }
}
