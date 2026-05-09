using JetBrains.Annotations;
using NATSInternal.Core.Common.Validation;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

using SearchableListValidator = SearchableListValidator<UserListRequestDto, UserListRequestDto.FieldToSort>;

[UsedImplicitly]
internal class UserListValidator : Validator<UserListRequestDto>
{
    #region Constructors
    public UserListValidator()
    {
        Include(new SearchableListValidator());
    }
    #endregion
}