using MediatR;

namespace NATSInternal.Application.UseCases.Users;

public class UserCreateRequestDto : IRequestDto, IRequest<Guid>
{
    #region Properties
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmationPassword { get; set; } = string.Empty;
    public List<string> RoleNames { get; set; } = new();
    #endregion
    
    #region Methods
    public void TransformValues() { }
    #endregion
}