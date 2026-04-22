using MediatR;
using NATSInternal.Application.AuditLogs;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.EventHandlers.Users;

internal class UserAddedToRolesEventHandler : INotificationHandler<UserAddedToRolesEvent>
{
    #region Fields
    private readonly IAuditLogService _auditLogService;
    private readonly IUserRepository _repository;
    #endregion

    #region Constructors
    public UserAddedToRolesEventHandler(IAuditLogService auditLogService, IUserRepository repository)
    {
        _auditLogService = auditLogService;
        _repository = repository;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserAddedToRolesEvent domainEvent, CancellationToken token = default)
    {
        User user = await _repository
            .GetUserByIdAsync(domainEvent.Id, token)
            ?? throw new InvalidOperationException($"{nameof(User)} with id \"{domainEvent.Id}\" is not found.");
        
        await _auditLogService.LogUserAddToRolesActionAsync(
            domainEvent.Id,
            new(user),
            domainEvent.AddedDateTime,
            token
        );
    }
    #endregion
}