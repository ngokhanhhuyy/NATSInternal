using MediatR;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Seedwork;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.UnitOfWork;

internal class UnitOfWork : IUnitOfWork
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IMediator _mediator;
    private readonly IDbExceptionConverter _exceptionConverter;
    #endregion

    #region Constructors
    public UnitOfWork(AppDbContext context, IMediator mediator, IDbExceptionConverter exceptionConverter)
    {
        _context = context;
        _mediator = mediator;
        _exceptionConverter = exceptionConverter;
    }
    #endregion

    #region Methods
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        List<AbstractEntity> domainEntities = _context.ChangeTracker
            .Entries<AbstractEntity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        List<IDomainEvent> domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        int result;
        try
        {
            result = await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            PersistenceException? persistenceException = _exceptionConverter.Convert(exception);
            if (persistenceException is null)
            {
                throw;
            }

            throw persistenceException;
        }

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            _ = Task.Run(async () => await _mediator.Publish(domainEvent, cancellationToken), cancellationToken);
        }

        foreach (AbstractEntity entity in domainEntities)
        {
            entity.ClearDomainEvents();
        }

        return result;
    }
    #endregion
}
