using Application.Chats.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Chats;
using Domain.Users;
using MediatR;

namespace Application.Chats.Commands;

public record CreateChatCommand: IRequest<Result<Chat,ChatException>>
{
    public required string Name { get; init; }
    public required bool IsGroup { get; init; }
    public required Guid ChatOwnerId { get; init; }
    public required List<Guid> Users { get; init; }
    
}
public class CreateChatCommandHandler(IChatRepository chatRepository, IUserRepository userRepository)
    : IRequestHandler<CreateChatCommand, Result<Chat, ChatException>>
{
    public async Task<Result<Chat, ChatException>> Handle(CreateChatCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userIds = request.Users.Select(x => new UserId(x)).ToList();
            var users = await userRepository.GetUsersByIds(userIds, cancellationToken);

            return await users.Match(
                async s =>
                {
                    if (s.Count != userIds.Count)
                    {
                        var missingUserIds = userIds.Except(s.Select(u => u.Id)).ToList();
                        return new ChatUsersNotFoundException(new Exception($"Users not found: {string.Join(", ", missingUserIds)}"));
                    }

                    var existingChat = await chatRepository.GetByName(request.Name, cancellationToken);

                    return await existingChat.Match(
                        c => Task.FromResult<Result<Chat, ChatException>>(new ChatAlreadyExistsException(c.Id)),
                        async () => await CreateEntity(request.Name, request.IsGroup, new UserId(request.ChatOwnerId), s, cancellationToken));
                },
                () => Task.FromResult<Result<Chat, ChatException>>(new ChatUsersNotFoundException(new Exception("No users found"))));
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(ChatId.Empty(), exception);
        }
    }

    private async Task<Result<Chat, ChatException>> CreateEntity(
        string name,
        bool isGroup,
        UserId chatOwnerId,
        List<User> users,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Chat.New(ChatId.New(), name, isGroup, chatOwnerId, users);

            return await chatRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ChatUnknownException(ChatId.Empty(), exception);
        }
    }
}