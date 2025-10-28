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
internal class ProductUpdateHandler : IRequestHandler<ProductUpdateRequestDto>
{
    #region Fields
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ProductUpdateRequestDto> _validator;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion
    
    #region Constructors
    public ProductUpdateHandler(
        IProductRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<ProductUpdateRequestDto> validator,
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
    public async Task Handle(ProductUpdateRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        Product product = await _repository
            .GetProductByIdIncludingBrandWithCountryAndCategoryAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        Brand? brand = product.Brand;
        if (requestDto.BrandId.HasValue && requestDto.BrandId.Value != product.BrandId)
        {
            brand = await _repository
                .GetBrandByIdIncludingCountryAsync(requestDto.BrandId.Value, cancellationToken)
                ?? throw OperationException.NotFound(new object[] { nameof(requestDto.BrandId) }, DisplayNames.Brand);
        }

        ProductCategory? category = product.Category;
        if (requestDto.CategoryName is not null)
        {
            Guid? oldCategoryId = product.CategoryId;
            
            category = await _repository.GetCategoryByNameAsync(requestDto.CategoryName, cancellationToken);
            if (category is null)
            {
                category = new(requestDto.CategoryName, _clock.Now);
                _repository.AddCategory(category);
            }

            if (oldCategoryId.HasValue &&
                await _repository.GetProductByIdIncludingBrandWithCountryAndCategoryAsync(oldCategoryId.Value) > 0)
            {
                
            }
        }
        
        product.Update(
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
        
        _repository.UpdateProduct(product);

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