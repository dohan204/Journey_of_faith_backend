using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.entities.masslive;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.churches;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.types.churches
{
    [ExtendObjectType(typeof(Church))]
    public static partial class MassScheduleQueryExtension
    {
        public static async Task<MassSchedule[]> MassSchedule(
            [Parent] Church church,
            IMassSchedulesDataLoader massSchedulesDataLoader,
            CancellationToken cancellation
        )
        {
            return await massSchedulesDataLoader.LoadAsync(church.Id, cancellation)  ?? [];
        }
    }
}
