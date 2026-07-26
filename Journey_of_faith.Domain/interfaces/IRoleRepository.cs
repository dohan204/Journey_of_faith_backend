using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Domain.entities;

namespace Journey_of_faith.Domain.interfaces;


public interface IRoleRepository
{
    Task<PagedResult<Role>> GetRolesAsync(int page, int pageSize, string? search);
    Task<string> CreateAsync(Role role, CancellationToken cancellationToken);
    Task<Dictionary<string, int>> GetTotalUserRole();
    Task<bool> AddPermissionForRole(string roleName, List<string> permissions);
    Task<List<object>> GetPermissionForRole();

    Task<bool> DeleteRoleAsync(string roleName);
    Task<bool> NameExists(string name);
}