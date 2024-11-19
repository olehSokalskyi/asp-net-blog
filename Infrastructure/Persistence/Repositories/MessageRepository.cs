using Application.Common.Interfaces.Repositories;
using Domain.Messages;

namespace Infrastructure.Persistence.Repositories;

public class MessageRepository(ApplicationDbContext context) : IMessageRepository
{
    public async Task<Message> Add(Message message, CancellationToken cancellationToken)
    {
        await context.Messages.AddAsync(message, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return message;
    }
}