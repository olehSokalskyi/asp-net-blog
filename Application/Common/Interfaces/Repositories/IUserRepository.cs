using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Option<User>> GetById(UserId id, CancellationToken cancellationToken);
    Task<Option<User>> GetByUsername(string username, CancellationToken cancellationToken);
    Task<Option<User>> GetByEmail(string email, CancellationToken cancellationToken);

    Task<User> Add(User user, CancellationToken cancellationToken);
    Task<User> Update(User user, CancellationToken cancellationToken);
    Task<User> Delete(User user, CancellationToken cancellationToken);


    Task<Option<List<User>>> GetUsersByIds(List<UserId> userIds, CancellationToken cancellationToken);
}