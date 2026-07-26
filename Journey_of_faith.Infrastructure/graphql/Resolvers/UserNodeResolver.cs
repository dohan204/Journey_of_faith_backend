using AutoMapper;
using AutoMapper.QueryableExtensions;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.context;

namespace Journey_of_faith.Infrastructure.graphql.Resolvers;

[ExtendObjectType(typeof(Query))]
public static partial class UserNodeResolver
{
    [UsePaging(IncludeTotalCount = true)]
    public static async Task<Page<User>> GetUsersAsync (
        PagingArguments pagingArgs,
        [Service] ApplicationDbContext db,
        [Service] IMapper mapper,
        CancellationToken cancellationToken
    ) {
        return await db.Users.OrderBy(u => u.Id)
        .ProjectTo<User>(mapper.ConfigurationProvider)
        .ToPageAsync(pagingArgs, cancellationToken);
    }


    public static async Task<User> GetMeAsync(
        string userId, [Service] IUserRepository userRepository,
        CancellationToken cancellation
        )
    {
        return await userRepository.GetUserAsync(Guid.Parse(userId)) 
            ?? throw new GraphQLException("User not found");
    }
}