
using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Domain.dtos;
using Microsoft.Extensions.Options;
using System.Data;

namespace Journey_of_faith.Infrastructure.repositories;

public class UserRepository : BaseRepository, Journey_of_faith.Domain.interfaces.IUserRepository
{
    public UserRepository(IDbConnectionFactory connectionFactory, IOptions<TableSchemaName> options)
        : base(connectionFactory, options) { }

    // ==============================
    // CÁC METHOD CŨ
    // ==============================

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default)
    {
        return await QueryAsync<User?>(
            async connection =>
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(
                    $"Select * from [{_schemaName.Schema}].[User] where Id = @Id",
                    new { Id = id.ToString() });
                return user;
            }
        );
    }

    public async Task<PagedResult<User>> GetUsersAsync(int page, int pageSize, string? search)
    {
        return await QueryAsync<PagedResult<User>>(async connection =>
        {
            int currentPageIndex = (page - 1) * pageSize;

            string sql = $@"select 
                            u.Id, 
                            u.UserName, 
                            u.Email,
                            aspr.Name as Role,
                            u.Avatar,
                            u.IsDeleted
                        from [{_schemaName.Schema}].[User] u
                        inner join [{_schemaName.Schema}].[AspNetUserRoles] aspur on u.Id = aspur.UserId
                        inner join [{_schemaName.Schema}].[AspNetRoles] aspr on aspur.RoleId = aspr.Id
                        where IsDeleted = 0
                        order by u.Id
                        offset @currentPageIndex rows
                        fetch next @pageSize rows only";

            var totalCount = await connection.ExecuteScalarAsync<int>(
                $"Select Count(*) from [{_schemaName.Schema}].[User] where IsDeleted = 0");
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
        return await QueryAsync<User?>(async connection =>
        {
            return await connection.QueryFirstOrDefaultAsync<User>(
                $"Select * from [{_schemaName.Schema}].[User] where Id = @Id",
                new { Id = Id.ToString() });
        });
    }

    public async Task<bool> DeleteUserAsync(Guid Id)
    {
        return await QueryAsync<bool>(async connection =>
        {
            return await connection
                .ExecuteScalarAsync<int>($@"Update [{_schemaName.Schema}].[User]
                                            set IsDeleted = 1,
                                                DeletionTime = getdate()
                                            where Id = @Id",
                                            new { Id = Id.ToString() }) > 0;
        });
    }

    // ==============================
    // 📌 CÁC METHOD CHO GOOGLE LOGIN (ĐÃ LOẠI BỎ Role, RoleId)
    // ==============================

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await QueryAsync<User?>(async connection =>
        {
            var sql = $@"SELECT 
                            Id,
                            Name,
                            Username,
                            Email,
                            PasswordHash,
                            Avatar,
                            ChurchId,
                            ProvinceId,
                            SchoolId,
                            CreatorUserId,
                            CreationTime,
                            LastModifierUserId,
                            LastModificationTime,
                            DeleterUserId,
                            DeletionTime,
                            IsDeleted,
                            AccessFailedCount,
                            EmailConfirmed,
                            LockoutEnabled,
                            TwoFactorEnabled,
                            PhoneNumber,
                            PhoneNumberConfirmed
                        FROM [{_schemaName.Schema}].[User]
                        WHERE Email = @Email";

            var result = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
            return result;
        });
    }

    public async Task CreateAsync(User user)
    {
        await QueryAsync(async connection =>
        {
            var sql = $@"INSERT INTO [{_schemaName.Schema}].[User] 
                        (Id, Name, Username, Email, PasswordHash, Avatar,
                         ChurchId, ProvinceId, SchoolId, CreatorUserId, CreationTime,
                         LastModifierUserId, LastModificationTime, DeleterUserId, DeletionTime, IsDeleted,
                         AccessFailedCount, EmailConfirmed, LockoutEnabled, TwoFactorEnabled,
                         PhoneNumber, PhoneNumberConfirmed)
                        VALUES 
                        (@Id, @Name, @Username, @Email, @PasswordHash, @Avatar,
                         @ChurchId, @ProvinceId, @SchoolId, @CreatorUserId, @CreationTime,
                         @LastModifierUserId, @LastModificationTime, @DeleterUserId, @DeletionTime, @IsDeleted,
                         @AccessFailedCount, @EmailConfirmed, @LockoutEnabled, @TwoFactorEnabled,
                         @PhoneNumber, @PhoneNumberConfirmed)";

            await connection.ExecuteAsync(sql, new
            {
                user.Id,
                user.Name,
                user.Username,
                user.Email,
                user.PasswordHash,
                user.Avatar,
                ChurchId = user.ChurchId,
                ProvinceId = user.ProvinceId,
                SchoolId = user.SchoolId,
                CreatorUserId = user.CreatorUserId,
                CreationTime = user.CreationTime,
                LastModifierUserId = user.LastModifierUserId == Guid.Empty ? user.CreatorUserId : user.LastModifierUserId,
                LastModificationTime = user.LastModificationTime ?? DateTime.UtcNow,
                DeleterUserId = user.DeleterUserId == Guid.Empty ? Guid.Empty : user.DeleterUserId,
                DeletionTime = user.DeletionTime,
                IsDeleted = user.IsDeleted ?? false,
                AccessFailedCount = user.AccessFailedCount,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                TwoFactorEnabled = user.TwoFactorEnabled,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            });

            return Task.CompletedTask;
        });
    }

    public async Task UpdateAsync(User user)
    {
        await QueryAsync(async connection =>
        {
            var sql = $@"UPDATE [{_schemaName.Schema}].[User] 
                        SET Name = @Name,
                            Username = @Username,
                            Email = @Email,
                            PasswordHash = @PasswordHash,
                            Avatar = @Avatar,
                            ChurchId = @ChurchId,
                            ProvinceId = @ProvinceId,
                            SchoolId = @SchoolId,
                            CreatorUserId = @CreatorUserId,
                            CreationTime = @CreationTime,
                            LastModifierUserId = @LastModifierUserId,
                            LastModificationTime = @LastModificationTime,
                            DeleterUserId = @DeleterUserId,
                            DeletionTime = @DeletionTime,
                            IsDeleted = @IsDeleted,
                            AccessFailedCount = @AccessFailedCount,
                            EmailConfirmed = @EmailConfirmed,
                            LockoutEnabled = @LockoutEnabled,
                            TwoFactorEnabled = @TwoFactorEnabled,
                            PhoneNumber = @PhoneNumber,
                            PhoneNumberConfirmed = @PhoneNumberConfirmed
                        WHERE Id = @Id";

            await connection.ExecuteAsync(sql, new
            {
                user.Id,
                user.Name,
                user.Username,
                user.Email,
                user.PasswordHash,
                user.Avatar,
                user.ChurchId,
                user.ProvinceId,
                user.SchoolId,
                user.CreatorUserId,
                user.CreationTime,
                LastModifierUserId = user.LastModifierUserId == Guid.Empty ? user.CreatorUserId : user.LastModifierUserId,
                LastModificationTime = DateTime.UtcNow,
                user.DeleterUserId,
                user.DeletionTime,
                user.IsDeleted,
                user.AccessFailedCount,
                user.EmailConfirmed,
                user.LockoutEnabled,
                user.TwoFactorEnabled,
                user.PhoneNumber,
                user.PhoneNumberConfirmed
            });

            return Task.CompletedTask;
        });
    }
}