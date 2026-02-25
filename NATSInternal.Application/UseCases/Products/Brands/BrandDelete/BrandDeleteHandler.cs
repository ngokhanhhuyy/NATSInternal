using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

internal class BrandDeleteHandler : IRequestHandler<BrandDeleteRequestDto>
{
    #region Fields
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    #endregion

    #region Constructors
    public BrandDeleteHandler(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Methods
    public async Task Handle(BrandDeleteRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        Brand brand = await _repository
            .GetBrandByIdAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        _repository.RemoveBrand(brand);

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