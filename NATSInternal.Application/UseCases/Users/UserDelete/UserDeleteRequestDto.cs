using MediatR;

namespace NATSInternal.Application.UseCases.Users;

public class UserDeleteRequestDto : IRequestDto, IRequest
{
    #region Properties
    public Guid Id { get; set; }
    #endregion
    
    #region Methods
    public void TransformValues() { }
    #endregion
}