using System.Globalization;
using System.Security.Claims;
using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Infrastructure.context;
using Journey_of_faith.Infrastructure.identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Journey_of_faith.Infrastructure.repositories;

public class RoleRepository : BaseRepository, IRoleRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RoleManager<ApplicationRole> _roleManager;
    public RoleRepository(ApplicationDbContext dbContext, 
        IOptions<TableSchemaName> tableSchemaName, IDbConnectionFactory dbConnectionFactory,
        RoleManager<ApplicationRole> roleManager)
    : base(dbConnectionFactory, schemaName: tableSchemaName)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
    }


    public async Task<PagedResult<Role>> GetRolesAsync(int page, int pageSize, string? search)
    {
        int totalRole = await _dbContext.Roles.CountAsync();
        var roles = await _dbContext.Roles.AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Role>
        {
            TotalCount = totalRole,
            Data = roles.Select(r => new Role
            {
                Id = r.Id.ToString(),
                Name = r.Name
            }).ToList(),
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<string> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        var insert = new ApplicationRole
        {
            Name = role.Name,
            Descriptions = role.Descriptions
        };
        await _dbContext.Roles.AddAsync(insert, cancellationToken);
        await _dbContext.SaveChangesAsync();
        return role.Id;
    }

    public async Task<Dictionary<string, int>> GetTotalUserRole()
    {
        return await QueryAsync<Dictionary<string, int>>(async connection =>
        {
            var data = await connection.QueryAsync<(string Name, int TotalUserPerRole)>(@"select  aspr.Name, count(*) as TotalUserPerRole
                                                from[jcodepro_journey_of_faith].[User] u
                                                inner join[jcodepro_journey_of_faith].[AspNetUserRoles] aspur on u.Id = aspur.UserId
                                                inner join[jcodepro_journey_of_faith].[AspNetRoles] aspr on aspur.RoleId = aspr.Id
                                                group by aspr.Name");
            return data.ToDictionary(x => x.Name, x => x.TotalUserPerRole);
        });
    }

    public async Task<bool> AddPermissionForRole(string roleName, List<string> permissions)
    {
        var roleExists = await _roleManager.FindByNameAsync(roleName);
        if(roleExists == null)
        {
            throw new BadRequestException("Tên vai trò không tồn tại");
        }
        // lấy ra các claim(quyền cuẩ vai trò);
        var existingClaims = await _roleManager.GetClaimsAsync(roleExists);

        // lấy ra các giá trị trị cúa claim
        var existingPermissionValues = existingClaims
            .Where(x => x.Type == "Permission")
            .Select(x => x.Value)
            .ToHashSet();
        foreach(string permission in permissions)
        {
            if(!existingPermissionValues.Contains(permission))
            {
                var result = await _roleManager.AddClaimAsync(roleExists, new Claim("Permission", permission));

                if(!result.Succeeded)
                {
                    throw new BadRequestException("Không thể thêm quyền cho Vai trò");
                }
            }
        }
        ;
        return true;
    }

    public async Task<bool> NameExists(string name) => await _dbContext.Roles.AnyAsync(e => e.Name == name);
}