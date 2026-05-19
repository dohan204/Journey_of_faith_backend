using Azure.Core;
using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.musics;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.common;
using Microsoft.AspNetCore.StaticAssets;
using Microsoft.Extensions.Options;

namespace Journey_of_faith.Infrastructure.repositories;

public class SongRepository : BaseRepository, ISongRepository
{
    public SongRepository(IDbConnectionFactory dbConnection, IOptions<TableSchemaName> options)
    : base(dbConnection, options)
    {
        
    }
    public async Task<bool> ExitsCategorySongAsync(string name)
    {
        return await QueryAsync<bool>(async connection =>
        {
            return await connection.ExecuteScalarAsync<int>(@"
                if exists (select 1 from [jcodepro_journey_of_faith].SongCategory 
                where Name = @Name)
                    select 1
                else 
                    select 0
            ", new {Name = name}) > 0;
        });
    }
    public async Task<int> CreateSongCategoryAsync(SongCategory songCategory, CancellationToken cancellationToken)
    {
        return await QueryAsync<int>(async connection =>
        {
           var sql = $@"insert into 
            [{_schemaName.Schema}].[{SongRelationShip.SongCategory}] (Name) values (@Name)";

            return await connection.ExecuteAsync(sql: sql, new {Name = songCategory.Name});
        });
    }
    public async Task<bool> DeleteSongCategoryAsync(int id, Guid userId, CancellationToken cancellationToken)
    {
        return await QueryAsync<bool>(async connection =>
        {
           var songCategory = await connection.ExecuteAsync($@"
              update from [{_schemaName.Schema}].[{SongRelationShip.SongCategory}]
                set IsDeleted = true,
                    DeletionTime = getdate(),
                    LastModificationTime = getdate(),
                    LastModifierUserId = @userId,
                    DeleterUserId = @userId
                where Id = @Id
           ", new
           {
                userId = userId,
                Id = id                                                  
           });

           return songCategory > 0;
        });
    }
    
    public async Task<int> CreateArtistAsync(Artist artist, CancellationToken cancellationToken)
    {
        return await QueryAsync<int>(async connection =>
        {
           var sql = $@"insert into [{_schemaName.Schema}].[{SongRelationShip.Artist}] (Name, Description, ImageUrl) values
            values(@Name, @Description, @ImageUrl)
           " ;
           return await connection.ExecuteAsync(sql: sql, new { Name = artist.Name, Description = artist.Description, ImageUrl = artist.ImageUrl});
        });
    }

    public async Task<bool> ExitsNameArtistAsync(string name)
    {
        return await QueryAsync<bool>(async connection =>
        {
            return await connection.ExecuteScalarAsync<int>($@"
                IF EXITS (SELECT 1 FROM [{_schemaName.Schema}].[{SongRelationShip.Artist}] where Name = @Name)
                    SELECT 1
                ELSE 
                    SELECT 0)
            ", new {Name = name}) > 0;
        });
    }
    public async Task<int> UpdateArtistAsync(int id, Artist artist, CancellationToken cancellationToken)
    {
        return await QueryAsync<int>(async connection =>
        {
           return await connection.ExecuteAsync("sp_UpdateArtist", new
           {
               Id = id,
               Name = artist.Name,
               Description = artist.Description,
               ImageUrl = artist.ImageUrl,
           },commandType: System.Data.CommandType.StoredProcedure); 
        });
    }

    public async Task<bool> DeleteArtistAsync(int id, Guid userId, CancellationToken cancellationToken)
    {
        return await QueryAsync<bool>(async connection =>
        {
            var artist = await connection.ExecuteAsync($@"
              update from [{_schemaName.Schema}].[{SongRelationShip.Artist}]
                set IsDeleted = true,
                    DeletionTime = getdate(),
                    LastModificationTime = getdate(),
                    LastModifierUserId = @userId,
                    DeleterUserId = @userId
                where Id = @Id
           ", new
           {
                userId = userId,
                Id = id                                                  
           });

           return artist > 0;
        });
    }

    public async Task<int> CreateAlbumAsync(Album album, CancellationToken cancellationToken)
    {
        return await QueryAsync<int>(async connection =>
        {
            var sql = $@"insert into [{_schemaName.Schema}].[{SongRelationShip.Album}] (Title, ArtistId, ReleaseYear, CoverImageUrl)
            values(@Title, @ArtistId, @ReleaseYear, @CoverImageUrl)";

            return await connection.ExecuteAsync(sql: sql, new
            {
                Title = album.Title,
                ArtistId = album.ArtistId,
                ReleaseYear = album.ReleaseYear,
                CoverImageUrl = album.CoverImageUrl,
            });
        });
    }


    public async Task<bool> DeleteAlbumAsync(int id, Guid userId, CancellationToken cancellationToken)
    {
        return await QueryAsync<bool>(async connection =>
        {
           var artist = await connection.ExecuteAsync($@"
              update from [{_schemaName.Schema}].[{SongRelationShip.Album}]
                set IsDeleted = true,
                    DeletionTime = getdate(),
                    LastModificationTime = getdate(),
                    LastModifierUserId = @userId,
                    DeleterUserId = @userId
                where Id = @Id
           ", new
           {
                userId = userId,
                Id = id                                                  
           });

           return artist > 0;
        });
    }

    public async Task<int> CreateSongAsync(Song song,int categoryId, CancellationToken token)
    {
        return await QueryAsync<int>(async connection =>
        {

           var songId = await connection.ExecuteScalarAsync<int>("sp_CreateSong", new
           {
               Title = song.Title,
               ArtistId = song.ArtistId,
               AlbumId = song.AlbumId,
               Duration = song.Duration,
               AudioUrl = song.AudioUrl,
               CoverImageUrl = song.CoverImageUrl,
               Lyric = song.Lyric,
               PlayCount = song.PlayCount,
               IsActive = song.IsActive,
           }, commandType: System.Data.CommandType.StoredProcedure); 
            await connection.ExecuteAsync($@"
                insert into [{_schemaName.Schema}].[{SongRelationShip.SongCategoryMapping}] (SongId, CategoryId)
                values(@SongId, @CategoryId)
            ", new {SongId = songId, CategoryId = categoryId});

            return songId;
        });
    }
    
    public async Task<bool> DeleteSongAsync(int id, Guid userId, CancellationToken cancellationToken)
    {
        return await QueryAsync<bool>(async connection =>
        {
           var song = await connection.ExecuteAsync($@"
              update from [{_schemaName.Schema}].[{SongRelationShip.Song}]
                set IsDeleted = true,
                    DeletionTime = getdate(),
                    LastModificationTime = getdate(),
                    LastModifierUserId = @userId,
                    DeleterUserId = @userId
                where Id = @Id
           ", new
           {
                userId = userId,
                Id = id                                                  
           });

           return song > 0;
        });
    }
    public async Task<int> CreatePlaylistSongAsync(PlaylistSong playlistSong, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public async Task<int> CreatePlaylistAsync(Playlist playlist, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CreateListeningHistoryAsync(ListeningHistory history, CancellationToken cancellationToken)
    {
        return await QueryAsync<int>(async connection =>
        {
           return await connection.ExecuteAsync($@"insert into [{_schemaName.Schema}].[{SongRelationShip.ListeningHistory}] (UserId, SongId, ListenTime)
                values(@UserId, @SongId, @ListenTime
           ", new
           {
                UserId = history.UserId,
                SongId = history.SongId,
                ListenTime = DateTime.UtcNow
           });
        });
    }

    public async Task<int> CreateUserFavoriteSongAsync(UserFavoriteSong userFavoriteSong,CancellationToken cancellationToken)
    {
        return await QueryAsync<int>(async connection =>
        {
            return await connection.ExecuteAsync($@"insert into [{_schemaName.Schema}].[{SongRelationShip.UserFavoriteSong}] (UserId, SongId, CreatedTime)
                values(@UserId, @SongId, @CreatedTime)
            ", new {UserId = userFavoriteSong.UserId, SongId = userFavoriteSong.SongId, CreatedTime = DateTime.UtcNow});
        });
    }
}


public static class SongRelationShip
{
    public const string UserFavoriteSong = "UserFavoriteSong";
    public const string ListeningHistory = "ListeningHistory";
    public const string Playlist = "Playlist";
    public const string Song = "Song";
    public const string PlaylistSong = "PlaylistSong";
    public const string SongCategoryMapping = "SongCategoryMapping";
    public const string SongCategory = "SongCategory";
    public const string Artist = "Artist";
    public const string Album = "Album";
}