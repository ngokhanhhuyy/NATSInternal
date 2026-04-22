using FluentValidation;
using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Security;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Application.UseCases.Customers;

internal class CustomerUpdateHandler : IRequestHandler<CustomerUpdateRequestDto>
{
    #region Fields
    private readonly IValidator<CustomerUpdateRequestDto> _validator;
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;
    private readonly Guid _callerId;
    #endregion

    #region Constructors
    public CustomerUpdateHandler(
        IValidator<CustomerUpdateRequestDto> validator,
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
    public async Task Handle(CustomerUpdateRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        OperationException introducerNotFoundException = OperationException.NotFound(
            new object[] { nameof(requestDto.IntroducerId) },
            DisplayNames.Introducer
        );

        Customer? introducer = null;
        if (requestDto.IntroducerId.HasValue)
        {
            introducer = await _repository
                .GetCustomerByIdAsync(requestDto.IntroducerId.Value, cancellationToken)
                ?? throw introducerNotFoundException;
        }

        Customer customer = await _repository
            .GetCustomerByIdIncludingIntroducerAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        customer.Update(
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
            updatedUserId: _callerId,
            updatedDateTime: _clock.Now
        );

        _repository.UpdateCustomer(customer);

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

            if (exception.IsForeignKeyConstraintViolation)
            {
                throw introducerNotFoundException;
            }

            throw;
        }
    }
    #endregion
}
