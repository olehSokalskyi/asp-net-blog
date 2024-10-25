using Domain.Messages;

namespace Application.Common.Interfaces;

public interface IMessageRepository
{
    Task<Message> Add(Message message, CancellationToken cancellationToken);
}