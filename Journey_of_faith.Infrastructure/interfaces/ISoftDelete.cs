using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.interfaces
{
    public interface ISoftDelete
    {
        public bool IsDelete { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
