using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Domain.entities;

namespace Journey_of_faith.Domain.interfaces;


public interface IUserRepository
{
    Task<PagedResult<User>> GetUsersAsync(int page, int pageSize, string? search);
    Task<User?> GetUserAsync(Guid Id);
    // Task<bool> UpdateUserAsync(User user);

    Task<bool> DeleteUserAsync(Guid Id);
}