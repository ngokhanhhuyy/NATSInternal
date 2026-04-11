using FluentValidation;
using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Security;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

internal class BrandUpdateHandler : IRequestHandler<BrandUpdateRequestDto>
{
    #region Fields
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<BrandUpdateRequestDto> _validator;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public BrandUpdateHandler(
        IProductRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<BrandUpdateRequestDto> validator,
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
    public async Task Handle(BrandUpdateRequestDto requestDto, CancellationToken cancellationToken)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        Brand brand = await _repository
            .GetBrandByIdIncludingCountryAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        Country? country = brand.Country;
        if (country?.Id != requestDto.CountryId)
        {
            if (requestDto.CountryId.HasValue)
            {
                country = await _repository.GetCountryByIdAsync(requestDto.CountryId.Value, cancellationToken);
            }
            else
            {
                country = null;
            }
        }

        brand.Update(
            name: requestDto.Name,
            website: requestDto.Website,
            socialMediaUrl: requestDto.SocialMediaUrl,
            phoneNumber: requestDto.PhoneNumber,
            email: requestDto.Email,
            address: requestDto.Address,
            updatedDateTime: _clock.Now,
            updatedUserId: _callerDetailProvider.GetId(),
            country: country);
        
        _repository.UpdateBrand(brand);

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