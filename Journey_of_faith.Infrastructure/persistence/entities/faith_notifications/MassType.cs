using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.faith_notifications
{
    public class MassType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<MassSchedule> MassSchedules { get; set; } = [];
    }
}
