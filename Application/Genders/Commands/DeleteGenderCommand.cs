using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Genders.Exceptions;
using Domain.Genders;
using MediatR;

namespace Application.Genders.Commands;

public record DeleteGenderCommand : IRequest<Result<Gender, GenderException>>
{
    public required Guid GenderId { get; init; }
}

public class DeleteGenderCommandHandler(IGenderRepository genderRepository)
    : IRequestHandler<DeleteGenderCommand, Result<Gender, GenderException>>
{
    public async Task<Result<Gender, GenderException>> Handle(
        DeleteGenderCommand request,
        CancellationToken cancellationToken)
    {
        var genderId = new GenderId(request.GenderId);

        var existingGender = await genderRepository.GetById(genderId, cancellationToken);

        return await existingGender.Match<Task<Result<Gender, GenderException>>>(
            async g => await DeleteEntity(g, cancellationToken),
            () => Task.FromResult<Result<Gender, GenderException>>(new GenderNotFoundException(genderId)));
    }

    public async Task<Result<Gender, GenderException>> DeleteEntity(Gender gender, CancellationToken cancellationToken)
    {
        try
        {
            return await genderRepository.Delete(gender, cancellationToken);
        }
        catch (Exception exception)
        {
            return new GenderUnknownException(gender.Id, exception);
        }
    }
}