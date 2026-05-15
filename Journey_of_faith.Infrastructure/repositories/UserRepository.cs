using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Infrastructure.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Journey_of_faith.Infrastructure.repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IDbConnectionFactory connectionFactory, IOptions<TableSchemaName> options): base(connectionFactory, options) {}
    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default)
    {
        return await QueryAsync<User?>(
            async connection =>
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>($"Select * from [{_schemaName.Schema}].[User] where Id = @Id", new { Id = id.ToString()});
                return user;
            }
        );
    }


    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await QueryAsync<IEnumerable<User>>(async connection =>
        {
           var users = await connection.QueryAsync<User>($"Select * from [{_schemaName.Schema}].[User]");
           return users; 
        });
    }
}