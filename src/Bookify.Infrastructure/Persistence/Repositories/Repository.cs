﻿using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Persistence.Repositories;

internal abstract class Repository<T> 
    where T : Entity
{
    protected readonly ApplicationDbContext DbContext;
    
    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }
    
    public virtual void Add(T entity)
    {
        DbContext.Set<T>().Add(entity);
    }
}