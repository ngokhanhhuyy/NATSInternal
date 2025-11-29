using MediatR;

namespace NATSInternal.Application.UseCases.Customers;

public class CustomerDeleteRequestDto : IRequest, IRequestDto
{
    #region Properties
    public Guid Id { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}