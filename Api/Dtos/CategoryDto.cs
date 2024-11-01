using Domain.Categories;

namespace Api.Dtos;

public record CategoryDto(
    Guid? Id,
    string Name,
    DateTime? CreatedAt,
    DateTime? UpdatedAt)
{
    public static CategoryDto FromDomainModel(Category category)
        => new(
            Id: category.Id.Value,
            Name: category.Name,
            CreatedAt: category.CreatedAt,
            UpdatedAt: category.UpdatedAt);
}