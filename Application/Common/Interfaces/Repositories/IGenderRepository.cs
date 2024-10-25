using Domain.Genders;
using Optional;

namespace Application.Common.Interfaces;

public interface IGenderRepository
{
    Task<Gender> Add(Gender gender, CancellationToken cancellationToken);
    Task<Option<Gender>> GetByName(string title, CancellationToken cancellationToken);
    Task<Option<Gender>> GetById(GenderId id, CancellationToken cancellationToken);
    Task<Gender> Update(Gender gender, CancellationToken cancellationToken);
    Task<Gender> Delete(Gender gender, CancellationToken cancellationToken);
}