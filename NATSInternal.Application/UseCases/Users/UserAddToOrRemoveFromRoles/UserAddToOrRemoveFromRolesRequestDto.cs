using MediatR;

namespace NATSInternal.Application.UseCases.Users;

public class UserAddToOrRemoveFromRolesRequestDto : IRequestDto, IRequest
{
    #region Properties
    public Guid Id { get; set; }
    public List<string> RoleNames { get; set; } = new();
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}