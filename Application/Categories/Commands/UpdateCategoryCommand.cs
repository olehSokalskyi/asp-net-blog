using Application.Categories.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Categories;
using MediatR;
using Optional;

namespace Application.Categories.Commands;

public record UpdateCategoryCommand : IRequest<Result<Category, CategoryException>>
{
    public required Guid CategoryId { get; init; }
    public required string Name { get; init; }
}

public class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<UpdateCategoryCommand, Result<Category, CategoryException>>
{
    public async Task<Result<Category, CategoryException>> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryId = new CategoryId(request.CategoryId);
        var faculty = await categoryRepository.GetById(categoryId, cancellationToken);

        return await faculty.Match(
            async f =>
            {
                var existingCategory = await CheckDuplicated(categoryId, request.Name, cancellationToken);

                return await existingCategory.Match(
                    c => Task.FromResult<Result<Category, CategoryException>>(new CategoryAlreadyExistsException(c.Id)),
                    async () => await UpdateEntity(f, request.Name, cancellationToken));
            },
            () => Task.FromResult<Result<Category, CategoryException>>(new CategoryNotFoundException(categoryId)));
    }

    private async Task<Result<Category, CategoryException>> UpdateEntity(
        Category entity,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(name);

            return await categoryRepository.Update(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new CategoryUnknownException(CategoryId.Empty(), e);
        }
    }

    private async Task<Option<Category>> CheckDuplicated(
        CategoryId categoryId,
        string name,
        CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByName(name, cancellationToken);

        return category.Match(
            c => c.Id == categoryId ? Option.None<Category>() : Option.Some(c),
            Option.None<Category>);
    }
}