using JetBrains.Annotations;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Customers;

[UsedImplicitly]
internal class CustomerGetListValidator : AbstractListValidator<
    CustomerGetListRequestDto,
    CustomerGetListRequestDto.FieldToSort>;