using JetBrains.Annotations;
using NATSInternal.Core.Common.Validation;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

[UsedImplicitly]
internal class UserGetListValidator : AbstractListValidator<UserListRequestDto, UserListRequestDto.FieldToSort>;