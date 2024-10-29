using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Queries;
using Domain.Genders;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class GenderRepository(ApplicationDbContext context) : IGenderRepository, IGenderQueries
{
    public async Task<IReadOnlyList<Gender>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Genders
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Option<Gender>> GetByName(string title, CancellationToken cancellationToken)
    {
        var entity = await context.Genders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Title == title, cancellationToken);

        return entity == null ? Option.None<Gender>() : Option.Some(entity);
    }
    
    public async Task<Option<Gender>> GetById(GenderId id, CancellationToken cancellationToken)
    {
        var entity = await context.Genders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Gender>() : Option.Some(entity);
    }
    
    public async Task<Gender> Add(Gender gender, CancellationToken cancellationToken)
    {
        await context.Genders.AddAsync(gender, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return gender;
    }
    
    public async Task<Gender> Update(Gender gender, CancellationToken cancellationToken)
    {
        context.Genders.Update(gender);

        await context.SaveChangesAsync(cancellationToken);

        return gender;
    }
    
    public async Task<Gender> Delete(Gender gender, CancellationToken cancellationToken)
    {
        context.Genders.Remove(gender);

        await context.SaveChangesAsync(cancellationToken);

        return gender;
    }
}