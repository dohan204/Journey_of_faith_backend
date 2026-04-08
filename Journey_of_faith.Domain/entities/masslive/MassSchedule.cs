using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.masslive
{
    public class MassSchedule : AuditableEntity
    {
        public bool? IsFixed { get; set; }
        public int ChurchId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan Time { get; set; }
        public int? MassTypeId { get; set; }
    }
}
