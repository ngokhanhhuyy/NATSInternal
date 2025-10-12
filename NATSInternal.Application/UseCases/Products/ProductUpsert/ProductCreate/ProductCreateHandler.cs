using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Security;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
internal class ProductCreateHandler : IRequestHandler<ProductCreateRequestDto, Guid>
{
    #region Fields
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ProductCreateRequestDto> _validator;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion
    
    #region Constructors
    public ProductCreateHandler(
        IProductRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<ProductCreateRequestDto> validator,
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
    public async Task<Guid> Handle(ProductCreateRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);
        
        Brand? brand = null;
        if (requestDto.BrandId.HasValue)
        {
            brand = await _repository
                .GetBrandByIdIncludingCountryAsync(requestDto.BrandId.Value, cancellationToken)
                ?? throw OperationException.NotFound(new object[] { nameof(requestDto.BrandId) }, DisplayNames.Brand);
        }

        ProductCategory? category = null;
        if (requestDto.CategoryName is not null)
        {
            category = await _repository.GetCategoryByNameAsync(requestDto.CategoryName, cancellationToken);
            if (category is null)
            {
                category = new(requestDto.CategoryName, _clock.Now);
                _repository.AddCategory(category);
            }
        }
        
        
        Product product = new(
            requestDto.Name,
            requestDto.Description,
            requestDto.Unit,
            requestDto.DefaultAmountBeforeVatPerUnit,
            requestDto.DefaultVatPercentage,
            requestDto.IsForRetail,
            requestDto.IsDiscontinued,
            _callerDetailProvider.GetId(),
            _clock.Now,
            brand,
            category
        );
        
        _repository.AddProduct(product);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
        catch (PersistenceException exception)
        {
            if (exception.IsConcurrencyConflict || exception.IsForeignKeyConstraintViolation)
            {
                throw new ConcurrencyException();
            }

            if (!exception.IsUniqueConstraintViolation)
            {
                throw;
            }

            switch (exception.ViolatedEntityName)
            {
                case nameof(ProductCategory):
                    throw new ConcurrencyException();
                case nameof(Product):
                    throw OperationException.Duplicated(
                        new object[] { nameof(requestDto.Name) },
                        DisplayNames.ProductName
                    );
                default:
                    throw;
            }
        }
    }
    #endregion
}