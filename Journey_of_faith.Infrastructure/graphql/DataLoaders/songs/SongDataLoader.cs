using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.musics;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Infrastructure.context;
using Journey_of_faith.Infrastructure.repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.songs
{
    public static partial class SongDataLoader 
    {
        [DataLoader]
        public static async Task<Dictionary<Guid, Song[]>> GetSongsByUserIdAsync(
            IReadOnlyList<Guid> userIds,
            [Service] IDbConnectionFactory dbConnectionFactory,  
            CancellationToken cancellationToken)
        {
            using var connection = dbConnectionFactory.CreateConnection();
        
            var sql = @"Select uf.UserId, s.* 
                        from [jcodepro_journey_of_faith].[UserFavoriteSong] uf
                        inner join [jcodepro_journey_of_faith].Song s on s.Id = uf.SongId
                        where uf.UserId in @userIds";

            var result = await connection.QueryAsync<Guid, Song, (Guid userId, Song song)>(
                sql,
                (userId, song) => (userId, song), 
                new {userIds = userIds.Distinct().ToArray()},
                splitOn: "Id"
                );

            return result
                .GroupBy(e => e.userId)
                .ToDictionary(g => g.Key, g => g.Select(e => e.song).ToArray());
        }

        [DataLoader]
        public static async Task<Dictionary<int, Song[]>> GetSongByArtistAsync(
            IReadOnlyList<int> artistId,
            [Service] IDbConnectionFactory dbConnectionFactory,
            CancellationToken cancellation)
        {
            using var connection = dbConnectionFactory.CreateConnection();

            var sql = @"Select * from [jcodepro_journey_of_faith].[Song] Where ArtistId in @Ids";

            var result = await connection.QueryAsync<Song>(sql, new { Ids = artistId.Distinct().ToArray() });

            return result.GroupBy(e => e.ArtistId)
                    .ToDictionary(g => g.Key, g => g.ToArray());
        }

        [DataLoader]
        public static async Task<Dictionary<int, Song[]>> GetSongByCategory(
            IReadOnlyList<int> categoryIds,
            [Service] IDbConnectionFactory dbConnectionFactory,
            CancellationToken cancellationToken
        )
        {
            using var connection = dbConnectionFactory.CreateConnection();
            var sql = @"Select scm.CategoryId, s.* from [jcodepro_journey_of_faith].[SongCategoryMapping] scm
                    inner join [jcodepro_journey_of_faith].[Song] s on scm.SongId = s.Id
                    where scm.CategoryId in @CategoryIds
                ";

            var result = await connection.QueryAsync<int, Song, (int CategoryId, Song song)>(
                sql, 
                (categoryId, song) => (categoryId, song),
                new { CategoryIds = categoryIds.Distinct().ToArray()},
                splitOn: "Id"
            );

            return result.GroupBy(e => e.CategoryId)
                .ToDictionary(g => g.Key, g => g.Select(e => e.song).ToArray());
        }
    }
}
