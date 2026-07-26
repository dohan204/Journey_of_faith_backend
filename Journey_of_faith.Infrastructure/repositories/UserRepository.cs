using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Domain.dtos;

// using Journey_of_faith.Infrastructure.persistence.Dtos;
using Microsoft.Extensions.Options;

namespace Journey_of_faith.Infrastructure.repositories;

public class UserRepository : BaseRepository, Journey_of_faith.Domain.interfaces.IUserRepository
{
    public UserRepository(IDbConnectionFactory connectionFactory, IOptions<TableSchemaName> options) : base(connectionFactory, options) { }
    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default)
    {
        return await QueryAsync<User?>(
            async connection =>
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>($"Select * from [{_schemaName.Schema}].[User] where Id = @Id", new { Id = id.ToString() });
                return user;
            }
        );
    }


    public async Task<PagedResult<User>> GetUsersAsync(int page, int pageSize, string? search)
    {
        return await QueryAsync<PagedResult<User>>(async connection =>
        {
            int currentPageIndex = (page - 1) * pageSize;

            string searchParam = string.IsNullOrEmpty(search) ? null : $"{search}%";

            // 2. Viết câu lệnh SQL tường minh trong một chuỗi duy nhất (Dùng dấu ngoặc đơn () bao quanh cụm OR)
            string sql = $@"select 
                            u.Id, 
                            u.UserName, 
                            u.Email,
                            aspr.Name as Role,
                            u.Avatar,
                            u.IsDeleted
                        from [jcodepro_journey_of_faith].[User] u
                        inner join [jcodepro_journey_of_faith].[AspNetUserRoles] aspur on u.Id = aspur.UserId
                        inner join [jcodepro_journey_of_faith].[AspNetRoles] aspr on aspur.RoleId = aspr.Id
                        where IsDeleted < 1
                        order by u.Id
                        offset 0 rows
                        fetch next 10 rows only";

            var totalCount = await connection.ExecuteScalarAsync<int>($"Select Count(*) from [{_schemaName.Schema}].[User]");
            var users = await connection.QueryAsync<User>(sql, new { currentPageIndex, pageSize });

            return new PagedResult<User>
            {
                Data = users.ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

        });
    }


    public async Task<User?> GetUserAsync(Guid Id)
    {
        return await QueryAsync<User>(async connection =>
        {
            return await connection.QueryFirstOrDefaultAsync<User>($"Select * from [{_schemaName.Schema}] where Id = @Id", new { Id });
        });
    }

    public async Task<bool> DeleteUserAsync(Guid Id)
    {
        return await QueryAsync<bool>(async connection =>
        {
            return 
                await connection
                    .ExecuteScalarAsync<int>(@$"Update [jcodepro_journey_of_faith].[User]
                                                set IsDeleted = 1,
                                                    DeletionTime =getdate()
                                                where Id = @Id
                                                ", new {Id}) > 0;
        });
    }
}