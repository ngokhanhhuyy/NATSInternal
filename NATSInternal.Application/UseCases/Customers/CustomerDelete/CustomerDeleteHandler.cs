using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Security;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Application.UseCases.Customers;

internal class CustomerDeleteHandler : IRequestHandler<CustomerDeleteRequestDto>
{
    #region Fields
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;
    private readonly Guid _callerId;
    #endregion

    #region Constructors
    public CustomerDeleteHandler(
        ICustomerRepository repository,
        IUnitOfWork unitOfWork,
        IClock clock,
        ICallerDetailProvider callerDetailProvider)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _clock = clock;
        _callerId = callerDetailProvider.GetId();
    }
    #endregion

    #region Methods
    public async Task Handle(CustomerDeleteRequestDto requestDto, CancellationToken cancellationToken)
    {
        Customer customer = await _repository
            .GetCustomerByIdAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();
        
        await DeleteAsync(customer: customer, isSoftDeletion: false, cancellationToken);
    }
    #endregion

    #region PrivateMethods
    private async Task DeleteAsync(
        Customer customer,
        bool isSoftDeletion,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!isSoftDeletion)
            {
                _repository.DeleteCustomer(customer);
            }
            else
            {
                customer.Delete(deletedDateTime: _clock.Now, deletedUserId: _callerId);
                _repository.UpdateCustomer(customer);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (PersistenceException exception)
        {
            if (exception.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            if (exception.IsForeignKeyConstraintViolation && !isSoftDeletion)
            {
                await DeleteAsync(customer: customer, isSoftDeletion: true, cancellationToken);
                return;
            }

            throw;
        }
    }
    #endregion
}