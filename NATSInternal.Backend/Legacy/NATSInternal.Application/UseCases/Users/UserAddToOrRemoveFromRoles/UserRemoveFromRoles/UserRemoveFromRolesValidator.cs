using JetBrains.Annotations;

namespace NATSInternal.Application.UseCases.Users;

[UsedImplicitly]
internal class UserRemoveFromRolesValidator : UserAddToOrRemoveFromRolesValidator<UserRemoveFromRolesRequestDto>;