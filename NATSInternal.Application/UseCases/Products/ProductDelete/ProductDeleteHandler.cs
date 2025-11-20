using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Security;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Domain.Features.Products;

internal class ProductDeleteHandler : IRequestHandler<ProductDeleteRequestDto>
{
    #region Fields
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public ProductDeleteHandler(
        IProductRepository repository,
        IUnitOfWork unitOfWork,
        ICallerDetailProvider callerDetailProvider,
        IClock clock)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _callerDetailProvider = callerDetailProvider;
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task Handle(ProductDeleteRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        Product product = await _repository
            .GetProductByIdIncludingBrandWithCountryAndCategoryAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        product.Delete(_callerDetailProvider.GetId(), _clock.Now);
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