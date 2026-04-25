using JetBrains.Annotations;
using NATSInternal.Core.Common.Validation;
using NATSInternal.Core.Features.Customers;

namespace NATSInternal.Application.UseCases.Customers;

[UsedImplicitly]
internal class CustomerGetListValidator : AbstractListValidator<
    CustomerListRequestDto,
    CustomerListRequestDto.FieldToSort>;