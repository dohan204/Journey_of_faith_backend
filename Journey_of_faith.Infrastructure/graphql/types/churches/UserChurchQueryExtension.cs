using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.churches;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.types.churches
{
    [ExtendObjectType(typeof(Church))]
    public static partial class UserChurchQueryExtension
    {
        public static async Task<User[]> UsersAsync(
            [Parent] Church church,
            IUserChurchByMappingDataLoader userChurchByMappingDataLoader,
            CancellationToken cancellation
        )
        {
            return await userChurchByMappingDataLoader.LoadAsync(church.Id, cancellation);
        }
    }
}
