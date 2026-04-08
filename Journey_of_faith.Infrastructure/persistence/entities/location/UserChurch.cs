using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.location
{
    public class UserChurch
    {
        public Guid UserId { get; set; }
        public int ChurchId { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Church Church { get; set; } = null!;
    }
}
