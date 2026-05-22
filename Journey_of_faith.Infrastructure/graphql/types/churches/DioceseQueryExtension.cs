using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.churches;

namespace Journey_of_faith.Infrastructure.graphql.types.churches
{
    [ExtendObjectType(typeof(Church))]
    public static partial class DioceseQueryExtension
    {
        public static async Task<Diocese> GetDioceseByChurch(
            [Parent] Church church,
            IDioceseByChurchDataLoader dioceseByChurchDataLoader,
            CancellationToken cancellationToken
        )
        {
            return await dioceseByChurchDataLoader.LoadAsync(church.DioceseId, cancellationToken);
        }
    }
}
