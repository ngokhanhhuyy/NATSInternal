using FluentValidation;
using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Security;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Application.UseCases.Customers;

internal class CustomerCreateHandler : IRequestHandler<CustomerCreateRequestDto, Guid>
{
    #region Fields
    private readonly IValidator<CustomerCreateRequestDto> _validator;
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;
    private readonly Guid _callerId;
    #endregion

    #region Constructors
    public CustomerCreateHandler(
        IValidator<CustomerCreateRequestDto> validator,
        ICustomerRepository repository,
        IUnitOfWork unitOfWork,
        IClock clock,
        ICallerDetailProvider callerDetailProvider)
    {
        _validator = validator;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _clock = clock;
        _callerId = callerDetailProvider.GetId();
    }
    #endregion

    #region Methods
    public async Task<Guid> Handle(CustomerCreateRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);


        OperationException introducerNotFoundException = OperationException.NotFound(
            new[] { nameof(requestDto.IntroducerId) },
            DisplayNames.Introducer
        );

        Customer? introducer = null;
        if (requestDto.IntroducerId.HasValue)
        {
            introducer = await _repository
                .GetCustomerByIdAsync(requestDto.IntroducerId.Value, cancellationToken)
                ?? throw introducerNotFoundException;
        }

        Customer customer = new(
            firstName: requestDto.FirstName,
            middleName: requestDto.MiddleName,
            lastName: requestDto.LastName,
            nickName: requestDto.NickName,
            gender: requestDto.Gender,
            birthday: requestDto.Birthday,
            phoneNumber: requestDto.PhoneNumber,
            zaloNumber: requestDto.ZaloNumber,
            facebookUrl: requestDto.FacebookUrl,
            email: requestDto.Email,
            address: requestDto.Address,
            note: requestDto.Note,
            introducer: introducer,
            createdUserId: _callerId,
            createdDateTime: _clock.Now
        );

        _repository.AddCustomer(customer);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return customer.Id;
        }
        catch (PersistenceException exception)
        {
            if (exception.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            if (exception.IsForeignKeyConstraintViolation)
            {
                throw introducerNotFoundException;
            }

            throw;
        }
    }
    #endregion
}