using NATSInternal.Domain.Exceptions;
using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

internal class User : AbstractAggregateRootEntity
{
    #region Fields
    private readonly List<Role> _roles = new();
    #endregion

    #region Constructors
    #nullable disable
    private User() { }
    #nullable enable

    public User(string userName, string passwordHash, Guid createdUserId, DateTime createdDateTime)
    {
        UserName = userName.ToLower();
        PasswordHash = passwordHash;
        CreatedUserId = createdUserId;
        CreatedDateTime = createdDateTime;

        AddDomainEvent(new UserCreatedEvent(Id, createdDateTime));
    }
    #endregion

    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserName { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime? LastUpdatedDateTime { get; private set; }
    public DateTime? DeletedDateTime { get; private set; }
    #endregion
    
    #region ForeignKeyProperties
    public Guid CreatedUserId { get; private set; }
    public Guid LastUpdatedUserId { get; private set; }
    public Guid DeletedUserId { get; private set; }
    #endregion

    #region NavigationProperties
    public IReadOnlyCollection<Role> Roles
    {
        get => _roles.AsReadOnly();
        private set => _roles.AddRange(value);
    }
    #endregion

    #region ComputedProperties
    public int PowerLevel => Roles.Count == 0 ? 0 : Roles.Max(r => r.PowerLevel);
    #endregion

    #region Methods
    public bool HasPermission(string permissionName)
    {
        return Roles.SelectMany(r => r.Permissions).Any(p => p.Name == permissionName);
    }

    public void AddToRoles(ICollection<Role> roles)
    {
        foreach (Role role in roles)
        {
            if (_roles.Any(r => r.Name == role.Name))
            {
                throw new DomainException($"User with id \"{Id}\" is already in role \"{role.Name}\".");
            }
            
            _roles.Add(role);
        }
    }

    public void AddToRoles(ICollection<Role> roles, Guid addedUserId, DateTime addedDateTime)
    {
        AddToRoles(roles);
        
        LastUpdatedUserId = addedUserId;
        LastUpdatedDateTime = addedDateTime;
        
        AddDomainEvent(new UserAddedToRolesEvent(Id, addedDateTime));
    }

    public void RemoveFromRoles(ICollection<Role> roles, Guid removedUserId, DateTime removedDateTime)
    {
        foreach (Role role in roles)
        {
            if (!_roles.Remove(role))
            {
                throw new DomainException($"User with id {Id} is not in role {role.Name}.");
            } 
        }

        LastUpdatedUserId = removedUserId;
        LastUpdatedDateTime = removedDateTime;
        
        AddDomainEvent(new UserRemovedFromRoleEvent(Id, removedDateTime));
    }

    public void ChangePasswordHash(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }

    public void ResetPasswordHash(string newPasswordHash, DateTime resettedDateTime)
    {
        PasswordHash = newPasswordHash;
        
        AddDomainEvent(new UserResetPasswordEvent(Id, resettedDateTime));
    }

    public void MarkAsDeleted(DateTime deletedDateTime)
    {
        if (DeletedDateTime is not null)
        {
            throw new DomainException($"User with id {Id} has already been deleted.");
        }

        DeletedDateTime = deletedDateTime;
    }
    #endregion
}