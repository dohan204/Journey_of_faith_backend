using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.location;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.churches
{
    public static partial class ChurchDataLoader
    {
        [DataLoader]
        public static async Task<Dictionary<int, Church[]>> GetChurchesAsync(
            IReadOnlyList<int> Ids, 
            IDbConnectionFactory connectionFactory,
            CancellationToken cancellationToken
        )
        {
            using var connection = connectionFactory.CreateConnection();
            var sql = @"Select * from [jcodepro_journey_of_faith].[Church] where DioceseId in @Ids";

            var result = await connection.QueryAsync<Church>(sql, new { Ids = Ids.Distinct().ToArray() });

            return result.GroupBy(e => e.DioceseId)
                .ToDictionary(e => e.Key, e => e.ToArray());
        }
    }
}
