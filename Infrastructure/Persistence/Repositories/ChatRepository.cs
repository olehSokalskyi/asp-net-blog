using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Chats;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class ChatRepository(ApplicationDbContext context) : IChatRepository, IChatQueries
{
    public async Task<Chat> Add(Chat chat, CancellationToken cancellationToken)
    {
        await context.Chats.AddAsync(chat, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return chat;
    }
    
    public async Task<Option<Chat>> GetByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Chats
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        return entity == null ? Option.None<Chat>() : Option.Some(entity);
    }
    
    public async Task<Option<Chat>> GetById(ChatId id, CancellationToken cancellationToken)
    {
        var entity = await context.Chats
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Chat>() : Option.Some(entity);
    }
    
    public async Task<Chat> Update(Chat chat, CancellationToken cancellationToken)
    {
        context.Chats.Update(chat);

        await context.SaveChangesAsync(cancellationToken);

        return chat;
    }
    
    public async Task<Chat> Delete(Chat chat, CancellationToken cancellationToken)
    {
        context.Chats.Remove(chat);

        await context.SaveChangesAsync(cancellationToken);

        return chat;
    }


    public async Task<IReadOnlyList<Chat>> GetChatsByUser(UserId user, CancellationToken cancellationToken)
    {
        return await context.Chats
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Users.Any(u => u.Id == user))
            .Include(x => x.Messages)
            .ThenInclude(m => m.User)
            .Include(x=> x.Users)
            .Include(x => x.ChatOwner)
            .ToListAsync(cancellationToken);
    }
}