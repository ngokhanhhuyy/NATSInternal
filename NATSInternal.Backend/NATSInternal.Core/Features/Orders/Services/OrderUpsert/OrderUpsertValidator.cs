using FluentValidation;
using NATSInternal.Core.Common.Contracts;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Orders;

internal class OrderUpsertValidator : Validator<OrderUpsertRequestDto>
{
    #region Constructors
    public OrderUpsertValidator(
        IValidator<CustomerUpsertRequestDto> customerValidator,
        IValidator<OrderUpsertProductItemRequestDto> productItemValidator,
        IValidator<OrderUpsertServiceItemRequestDto> serviceItemValidator,
        IValidator<PhotoUpsertRequestDto> photoValidator,
        IClock clock)
    {
        RuleFor(dto => dto.StatsDate)
            .IsValidStatsDate(clock.Today)
            .WithName(DisplayNames.StatsDate);
        RuleFor(dto => dto.Note)
            .MaximumLength(HasStatsContracts.NoteMaxLength);
            
        RuleFor(dto => dto.CustomerId)
            .GreaterThan(0)
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.Customer);

        RuleFor(dto => dto.Customer)
            .SetValidator(customerValidator)
            .When(dto => dto.Customer is not null)
            .WithName(DisplayNames.Customer);

        RuleFor(dto => dto.ProductItems)
            .NotEmpty()
            .Unique(pi => pi.ProductId)
            .When(dto => dto.Type is OrderType.Retail)
            .WithName(DisplayNames.OrderProductItem);

        RuleForEach(dto => dto.ProductItems)
            .SetValidator(productItemValidator)
            .WithName(DisplayNames.OrderProductItem);

        RuleFor(dto => dto.ServiceItems)
            .NotEmpty()
            .Unique(pi => pi.Name)
            .When(dto => dto.Type is OrderType.Consultant or OrderType.Treatment)
            .WithName(DisplayNames.OrderServiceItem);
        RuleForEach(dto => dto.ServiceItems)
            .SetValidator(serviceItemValidator)
            .WithName(DisplayNames.OrderProductItem);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.Type)
                .IsInEnum()
                .WithName(DisplayNames.OrderType);
            RuleForEach(dto => dto.Photos)
                .SetValidator(photoValidator, ruleSets: "Create")
                .WithName(DisplayNames.Photo);
        });

        RuleSet("Update", () =>
        {
            RuleForEach(dto => dto.Photos)
                .SetValidator(photoValidator, ruleSets: "CreateOrUpdate")
                .WithName(DisplayNames.Photo);
        });
    }
    #endregion
}