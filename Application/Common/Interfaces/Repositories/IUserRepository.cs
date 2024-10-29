using Domain.Users;
using Optional;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User> Add(User user, CancellationToken cancellationToken);
    Task<Option<User>> GetByUsername(string username, CancellationToken cancellationToken);
    
    Task<Option<User>> GetById(UserId id, CancellationToken cancellationToken);
}