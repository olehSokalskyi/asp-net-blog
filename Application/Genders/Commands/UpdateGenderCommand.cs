using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Genders.Exceptions;
using Domain.Genders;
using MediatR;
using Optional;

namespace Application.Genders.Commands;

public record UpdateGenderCommand : IRequest<Result<Gender, GenderException>>
{
    public required Guid GenderId { get; init; }
    public required string Title { get; init; }
}

public class UpdateGenderCommandHandler(
    IGenderRepository genderRepository) : IRequestHandler<UpdateGenderCommand, Result<Gender, GenderException>>
{
    public async Task<Result<Gender, GenderException>> Handle(
        UpdateGenderCommand request,
        CancellationToken cancellationToken)
    {
        var genderId = new GenderId(request.GenderId);
        var gender = await genderRepository.GetById(genderId, cancellationToken);

        return await gender.Match(
            async g =>
            {
                var existingGender = await CheckDuplicated(genderId, request.Title, cancellationToken);

                return await existingGender.Match(
                    eg => Task.FromResult<Result<Gender, GenderException>>(new GenderAlreadyExistsException(eg.Id)),
                    async () => await UpdateEntity(g, request.Title, cancellationToken));
            },
            () => Task.FromResult<Result<Gender, GenderException>>(new GenderNotFoundException(genderId)));
    }

    private async Task<Result<Gender, GenderException>> UpdateEntity(
        Gender gender,
        string title,
        CancellationToken cancellationToken)
    {
        try
        {
            gender.UpdateDetails(title);

            return await genderRepository.Update(gender, cancellationToken);
        }
        catch (Exception exception)
        {
            return new GenderUnknownException(gender.Id, exception);
        }
    }

    private async Task<Option<Gender>> CheckDuplicated(
        GenderId genderId,
        string title,
        CancellationToken cancellationToken)
    {
        var gender = await genderRepository.GetByName(title, cancellationToken);

        return gender.Match(
            g => g.Id == genderId ? Option.None<Gender>() : Option.Some(g),
            Option.None<Gender>);
    }
}