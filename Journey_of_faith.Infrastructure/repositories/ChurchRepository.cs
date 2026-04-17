using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Application.usecases.churchs.dtos;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.entities.masslive;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Domain.objectvalues.churchs;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Infrastructure.context;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Journey_of_faith.Infrastructure.repositories
{
    public class ChurchRepository : BaseRepository, IChurchRepository 
    {
        #region Fields & constructor
        public ChurchRepository(IDbConnectionFactory _dbConnection, IOptions<TableSchemaName> _schemaName) : base(_dbConnection, _schemaName)
        {
        }
        #endregion
        #region Crud church
        public async Task<int> CreateAsync(Domain.entities.location.Church church)
        {
            var churh = new CreateChurchDto
            {
                Name = church.Name,
                Thumbnail = church.Thumbnail ?? "unknow",
                Website = church.Website ?? "unknow",
                Address = church.Address,
                DioceseId = church.DioceseId!.Value,
                Latitude = church.GeoLocation.Latitude,
                Longitude = church.GeoLocation.Longitude,
                CreatorUserId = church.CreatorUserId,
                LastModifierUserId = church.LastModifierUserId
            };
            return await ExecuteAsync<int>(async connection =>
                await connection.ExecuteAsync($@"
                    INSERT INTO [{_schemaName.Schema}].[{TableTopicChurch.Church}] 
                    (Name,Thumbnail, Website, Address, DioceseId, Latitude, Longitude, CreatorUserId, LastModifierUserId,)
                    Output inserted.Id
                    VALUES (@Name,@Thumbnail, @Website, @Address, @DioceseId, @Latitude, @Longitude, @CreatorUserId, @LastModifierUserId)
                ", churh)
            );
        }

        public async Task<IEnumerable<Church>> GetAllAsync()
        {
            return await ExecuteAsync<IEnumerable<Church>>(async connection =>
            {
                using (var multiple = await connection.QueryMultipleAsync($@"
                    SELECT * FROM [{_schemaName.Schema}].[{TableTopicChurch.Church}]
                    SELECT * FROM [{_schemaName.Schema}].[{TableTopicChurch.MassSchedule}]
                "))
                {
                    var church = (await multiple.ReadAsync<Church>()).ToList();
                    var massChedule = (await multiple.ReadAsync<MassSchedule>()).ToList();

                    foreach(var d in church)
                    {
                        d.SetMassSchedule(massChedule.Where(e => e.ChurchId == d.Id).ToList());
                    }
                    return church;
                }
            });
        }

        public async Task<Church?> GetByIdAsync(int id)
        {
            return await ExecuteAsync<Church>(async connection =>
            {
                using (var multiple = await
                    connection.QueryMultipleAsync($@"SELECT * FROM [{_schemaName.Schema}].[{TableTopicChurch.Church}] WHERE Id = @Id
                        SELECT * FROM [{_schemaName.Schema}].[{TableTopicChurch.MassSchedule}] WHERE ChurchId = @Id 
                    ", new { Id = id }))
                {
                    var church = await multiple.ReadSingleOrDefaultAsync<Church>();
                    if (church == null) return null;
                    var massSchedule = (await multiple.ReadAsync<MassSchedule>()).ToList();
                    
                    church.SetMassSchedule(massSchedule);

                    return church;
                }
            });
        }

        public async Task<int> UpdateAsync(Domain.entities.location.Church church)
        {
            var updateChurchDto = new ChurchUpdateDto
            {
                Id = church.Id,
                Name = church.Name,
                Thumbnail = church.Thumbnail,
                Website = church.Website,
                Address = church.Address,
                DioceseId = church.DioceseId,
                Latitude = church.GeoLocation.Latitude,
                Longitude = church.GeoLocation.Longitude
            };
            return await ExecuteAsync<int>(async connection =>
                await connection.ExecuteAsync($@"
                  UPDATE [{_schemaName.Schema}].[{TableTopicChurch.Church}] SET
                    Name = COALESCE(@Name, Name),
                    Thumbnail = COALESCE(@Thumbnail, Thumbnail),
                    Website = COALESCE(@Website, Website),
                    Address = COALESCE(@Address, Address),
                    DioceseId = COALESCE(@DioceseId, DioceseId),
                    Latitude = COALESCE(@Latitude, Latitude),
                    Longitude = COALESCE(@Longitude, Longitude)
                WHERE Id = @Id
            ", updateChurchDto)
            );
        }

        #endregion

        #region Discose
        public async Task<int> CreateAsync(Diocese diocese)
        {
            
            var discose = new CreateDiscoseDto
            {
                Name = diocese.Name,
                Website = diocese.Website ?? "Chưa có website",
                Address = diocese.Address!,
                Thumbnail = diocese.Thumbnail ?? "Chưa có ảnh",
                CreatorUserId = diocese.CreatorUserId,
                LastModifierUserId = diocese.LastModifierUserId,
            };
            Console.WriteLine(discose.Thumbnail);
            return await ExecuteAsync<int>(async connection =>
                await connection.ExecuteAsync($@"
                    INSERT INTO [{_schemaName.Schema}].[{TableTopicChurch.Diocese}] (Name,  Website, Address, Thumbnail, CreatorUserId, LastModifierUserId)
                    VALUES (@Name, @Website, @Address, @Thumbnail, @CreatorUserId, @LastModifierUserId)
                ", discose)
            );
        }
        public async Task<Diocese?> GetDioceseByIdAsync(int id)
        {

            return await ExecuteAsync<Diocese>(async connection =>
            {
                using (var multiple = await connection.QueryMultipleAsync($@"
                SELECT * FROM [{_schemaName.Schema}].[{TableTopicChurch.Diocese}] WHERE Id = @Id;
                SELECT * FROM [{_schemaName.Schema}].[{TableTopicChurch.Church} ] WHERE DioceseId = @Id;
            ", new { Id = id }))
                {
                    var diocese = await multiple.ReadSingleOrDefaultAsync<Diocese>();
                    var church = (await multiple.ReadAsync<Church>()).ToList();

                    diocese?.SetChurch(church);
                    return diocese;
                }
            });
            
        }
        public async Task<IEnumerable<Diocese>> GetAllDiocesesAsync()
        {
            return await ExecuteAsync<IEnumerable<Diocese>>(async connection =>
            {
                using (var multi = await connection.QueryMultipleAsync($@"
                    Select * from [{_schemaName.Schema}].[{TableTopicChurch.Diocese}] where IsDeleted = 0
                    Select * from [{_schemaName.Schema}].[{TableTopicChurch.Church}]
                "))
                {
                    var diocese = (await multi.ReadAsync<Diocese>()).ToList();
                    var church = (await multi.ReadAsync<Church>()).ToList();

                    var churchLookup = church
                         .GroupBy(c => c.DioceseId)
                         .ToDictionary(g => g.Key, g => g.ToList());

                    foreach (var d in diocese)
                    {
                        d.SetChurch(churchLookup.GetValueOrDefault(d.Id, new List<Church>()));
                    }
                    return diocese;
                }
            });
        }
        public async Task<int> CreateAsync(MassType massType)
        {
            return await ExecuteAsync<int>(async connection =>
                await connection.ExecuteAsync($@"
                    INSERT INTO [{_schemaName.Schema}].[{TableTopicChurch.MassSchedule}]
                    (Name) VALUES(@Name)
                ", new {Name = massType.Name})
            );
        }

        public async Task<int> DeleteMassType(int id)
        {
            return await ExecuteAsync<int>(async connection =>
                await connection.ExecuteAsync($@"
                    UPDATE [{_schemaName.Schema}].[{TableTopicChurch.MassType}]
                        SET IsDeleted = 1
                    WHERE Id = @Id
                ", new { Id = id})
            );
        }

        public Task<int> UpdateAsync(Diocese diocese)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteDiocese(int id)
        {
            return await ExecuteAsync<int>(async connection =>
                await connection.ExecuteAsync($@"
                    UPDATE [{_schemaName.Schema}].[{TableTopicChurch.Diocese}]
                        SET IsDeleted = 1
                    WHERE Id = @Id
                ", new {Id = id})
            );
        }
        #endregion
        public async Task<bool> GetDioceseExistsAsync(int dioceseId)
        {
            return await ExecuteAsync<bool>(async connection =>
                await connection.ExecuteAsync($@"
                    IF EXISTS (Select Id from [{_schemaName.Schema}].[{TableTopicChurch.Diocese}] where Id = id)
                        Select 1
                    ELSE 
                        Select 0
                ", new { Id = dioceseId }) > 0
            );
        }
        public async Task<bool> UniqueNameDiocese(string name)
        {
            return await ExecuteAsync<bool>(async con =>
                await con.ExecuteScalarAsync<int>($@"
                    IF exists (select name from [{_schemaName.Schema}].[{TableTopicChurch.Diocese}])
                        Select 1
                    ELSE 
                        Select 0
                ") > 0
            );
        }

    }


    #region Constants
    public static class TableTopicChurch
    {
        public const string Church = "Church";
        public const string MassSchedule = "MassSchedule";
        public const string LiveStream = "LiveStream";
        public const string Diocese = "Diocese";
        public const string MassType = "MassType";
    }

    #endregion
}
