using Domain.Genders;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IGenderQueries
{
    Task<IReadOnlyList<Gender>> GetAll(CancellationToken cancellationToken);
    Task<Option<Gender>> GetById(GenderId id, CancellationToken cancellationToken);
    Task<Option<Gender>> GetByName(string title, CancellationToken cancellationToken);
}