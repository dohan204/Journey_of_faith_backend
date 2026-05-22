using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.churches;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.types.churches
{
    [ExtendObjectType(typeof(Diocese))]
    public static partial class ChurchQueryExtension
    {
        public static async Task<Church[]> Churches(
            [Parent] Diocese diocese,
            IChurchesDataLoader churchesDataLoader,
            CancellationToken cancellationToken 
        )
        {
            return await churchesDataLoader.LoadAsync(diocese.Id, cancellationToken);
        }
    }
}
