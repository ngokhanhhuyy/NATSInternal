using JetBrains.Annotations;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Users;

[UsedImplicitly]
internal class UserGetListValidator : AbstractListValidator<UserGetListRequestDto, UserGetListRequestDto.FieldToSort>;