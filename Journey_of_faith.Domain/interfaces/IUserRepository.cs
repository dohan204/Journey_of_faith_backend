
using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Domain.entities;

namespace Journey_of_faith.Domain.interfaces;

public interface IUserRepository
{
    Task<PagedResult<User>> GetUsersAsync(int page, int pageSize, string? search);
    Task<User?> GetUserAsync(Guid Id);
    Task<bool> DeleteUserAsync(Guid Id);
    
    // Thêm 3 method này cho Google Login
    Task<User?> GetByEmailAsync(string email);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
}