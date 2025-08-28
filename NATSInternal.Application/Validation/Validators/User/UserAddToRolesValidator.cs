namespace NATSInternal.Core.Validation.Validators;

public class UserAddToRolesValidator : Validator<UserAddToRolesRequestDto>
{
    #region Constructors
    public UserAddToRolesValidator()
    {
        RuleFor(dto => dto.RoleNames)
            .NotEmpty()
            .WithName(DisplayNames.Role);
    }
    #endregion
}
