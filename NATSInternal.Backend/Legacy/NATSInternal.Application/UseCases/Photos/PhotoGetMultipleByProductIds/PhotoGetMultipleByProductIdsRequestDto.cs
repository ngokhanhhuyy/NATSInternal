using MediatR;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Photos;

public class PhotoGetMultipleByProductIdsRequestDto : IRequest<ICollection<PhotoBasicResponseDto>>, IRequestDto
{
    #region Properties
    public ICollection<Guid> ProductIds { get; set; } = new List<Guid>();
    #endregion
    
    #region Methods
    public void TransformValues() { }
    #endregion
}