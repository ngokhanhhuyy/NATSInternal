using FluentValidation;
using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Security;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

internal class BrandCreateHandler : IRequestHandler<BrandCreateRequestDto, Guid>
{
    #region Fields
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<BrandCreateRequestDto> _validator;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public BrandCreateHandler(
        IProductRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<BrandCreateRequestDto> validator,
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
    public async Task<Guid> Handle(BrandCreateRequestDto requestDto, CancellationToken cancellationToken)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);
        
        Country? country = null;
        if (requestDto.CountryId.HasValue)
        {
            country = await _repository.GetCountryByIdAsync(requestDto.CountryId.Value, cancellationToken);
        }

        Brand brand = new(
            name: requestDto.Name,
            website: requestDto.Website,
            socialMediaUrl: requestDto.SocialMediaUrl,
            phoneNumber: requestDto.SocialMediaUrl,
            email: requestDto.Email,
            address: requestDto.Address,
            createdDateTime: _clock.Now,
            createdUserId: _callerDetailProvider.GetId(),
            country: country);

        _repository.AddBrand(brand);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return brand.Id;
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
