// using Dapper;
// using Journey_of_faith.Application.common.interfaces;
// using Journey_of_faith.Domain.entities.location;
// using System;
// using System.Collections.Generic;
// using System.Text;

// namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.churches
// {
//     public static partial class DioceseDataLoader
//     {
//         [DataLoader]
//         public static async Task<Dictionary<int, Diocese>> GetDioceseByChurch(
//             IReadOnlyList<int> Ids,
//             [Service] IDbConnectionFactory dbConnectionFactory,
//             CancellationToken cancellationToken)
//         {
//             using var connection = dbConnectionFactory.CreateConnection();
//             var sql = @"Select * from [jcodepro_journey_of_faith].[Diocese] where DioceseId in @Ids";

//             var result = await connection.QueryAsync<Diocese>(sql, new { Ids = Ids });

//             return result.ToDictionary(e => e.Id);

//         }
//     }
// }
