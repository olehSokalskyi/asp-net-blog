using Domain.Messages;

namespace Application.Common.Interfaces.Repositories;

public interface IMessageRepository
{
    Task<Message> Add(Message message, CancellationToken cancellationToken);
}