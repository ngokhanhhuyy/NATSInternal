using FluentValidation;
using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Security;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

internal class ProductCategoryUpdateHandler : IRequestHandler<ProductCategoryUpdateRequestDto>
{
    #region Fields
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ProductCategoryUpdateRequestDto> _validator;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public ProductCategoryUpdateHandler(
        IProductRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<ProductCategoryUpdateRequestDto> validator,
        ICallerDetailProvider callerDetailProvider,
        IClock clock)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _callerDetailProvider = callerDetailProvider;
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task Handle(ProductCategoryUpdateRequestDto requestDto, CancellationToken cancellationToken)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        ProductCategory productCategory = await _repository
            .GetCategoryByIdAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        productCategory.Update(requestDto.Name, _callerDetailProvider.GetId(), _clock.Now);
        _repository.UpdateCategory(productCategory);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (PersistenceException exception)
        {
            if (exception.IsConcurrencyConflict || exception.IsForeignKeyConstraintViolation)
            {
                throw new ConcurrencyException();
            }

            if (exception.IsUniqueConstraintViolation)
            {
                throw OperationException.Duplicated(new object[] { nameof(requestDto.Name) }, DisplayNames.Name);
            }

            throw;
        }
    }
    #endregion
}