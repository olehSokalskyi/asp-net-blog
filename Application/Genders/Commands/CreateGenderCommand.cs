using Application.Common;
using Application.Common.Interfaces;
using Application.Genders.Exceptions;
using Domain.Genders;
using MediatR;

namespace Application.Genders.Commands;

public record CreateGenderCommand : IRequest<Result<Gender, GenderException>>
{
    public required string Title { get; init; }
}

public class CreateGenderCommandHandler(
    IGenderRepository genderRepository) : IRequestHandler<CreateGenderCommand, Result<Gender, GenderException>>
{
    public async Task<Result<Gender, GenderException>> Handle(
        CreateGenderCommand request,
        CancellationToken cancellationToken)
    {
        var existingGender = await genderRepository.GetByName(request.Title, cancellationToken);

        return await existingGender.Match(
            g => Task.FromResult<Result<Gender, GenderException>>(new GenderAlreadyExistsException(g.Id)),
            async () => await CreateEntity(request.Title, cancellationToken));
    }

    private async Task<Result<Gender, GenderException>> CreateEntity(
        string title,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Gender.New(GenderId.New(), title);

            return await genderRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new GenderUnknownException(GenderId.Empty, exception);
        }
    }
}