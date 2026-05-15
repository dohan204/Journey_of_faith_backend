using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.entities.location;

namespace Journey_of_faith.Domain.interfaces;


public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default);
    Task<IEnumerable<User>> GetUsersAsync();
}



