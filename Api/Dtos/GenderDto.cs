using Domain.Genders;

namespace Api.Dtos;

public record GenderDto(Guid? Id, string Title)
{
    public static GenderDto FromDomainModel(Gender gender)
        => new(gender.Id.Value, gender.Title);
}
