using MediatR;
using NATSInternal.Application.AuditLogs;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.EventHandlers.Users;

internal class UserCreateEventHandler : INotificationHandler<UserCreatedEvent>
{
    #region Fields
    private readonly IUserRepository _repository;
    private readonly IAuditLogService _auditLogService;
    #endregion

    #region Constructors
    public UserCreateEventHandler(IUserRepository repository, IAuditLogService auditLogService)
    {
        _repository = repository;
        _auditLogService = auditLogService;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserCreatedEvent domainEvent, CancellationToken token = default)
    {
        User user = await _repository
            .GetUserByIdAsync(domainEvent.Id, token)
            ?? throw new InvalidOperationException($"{nameof(User)} with id \"{domainEvent.Id}\" is not found.");
        
        await _auditLogService.LogUserCreateActionAsync(
            domainEvent.Id,
            new(user),
            domainEvent.CreatedDateTime,
            token
        );
    }
    #endregion
}