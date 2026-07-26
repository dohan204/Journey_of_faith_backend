using AutoMapper;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.types.users;

[ExtendObjectType(typeof(User))]
public static partial class UserChurches
{
    public static async Task<Church[]> GetChurchesAsync(
        [Parent] User user,
        IChurchsByUserIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        return await dataLoader.LoadAsync(user.Id, cancellationToken);
    }
}
