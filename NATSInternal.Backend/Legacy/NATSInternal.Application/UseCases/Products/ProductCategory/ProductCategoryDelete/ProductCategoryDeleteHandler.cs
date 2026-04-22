using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

internal class ProductCategoryDeleteHandler : IRequestHandler<ProductCategoryDeleteRequestDto>
{
    #region Fields
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    #endregion

    #region Constructors
    public ProductCategoryDeleteHandler(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Methods
    public async Task Handle(ProductCategoryDeleteRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        ProductCategory brand = await _repository
            .GetCategoryByIdAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        _repository.RemoveCategory(brand);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (PersistenceException exception)
        {
            if (exception.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }
    #endregion
}