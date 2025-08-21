namespace NATSInternal.Core.Validation.Validators;

public class UserRemoveFromRolesValidator : Validator<UserRemoveFromRolesRequestDto>
{
    #region Constructors
    public UserRemoveFromRolesValidator()
    {
        RuleFor(dto => dto.RoleNames)
            .NotEmpty()
            .WithName(DisplayNames.Role);
    }
    #endregion
}
