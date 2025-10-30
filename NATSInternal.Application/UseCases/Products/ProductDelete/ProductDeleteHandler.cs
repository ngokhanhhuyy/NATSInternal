using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Domain.Features.Products;

internal class ProductDeleteHandler : IRequestHandler<ProductDeleteRequestDto>
{
    #region Fields
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    #endregion

    #region Constructors
    public ProductDeleteHandler(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Methods
    public async Task Handle(ProductDeleteRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        Product product = await _repository
            .GetProductByIdIncludingBrandWithCountryAndCategoryAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        product.MarkAsDeleted();
        _repository.UpdateProduct(product);

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