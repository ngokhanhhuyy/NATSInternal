using MediatR;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Photos;

public class PhotoGetSingleRequestDto : IRequest<PhotoBasicResponseDto>, IRequestDto
{
    #region Properties
    public Guid Id { get; init; }
    #endregion
    
    #region Methods
    public void TransformValues() { }
    #endregion
}