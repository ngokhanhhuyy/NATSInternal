﻿namespace NATSInternal.Validation.Validators;

public class UserUpdateValidator : Validator<UserUpdateRequestDto>
{
    public UserUpdateValidator()
    {
        RuleFor(dto => dto.PersonalInformation)
            .SetValidator(new UserPersonalInformationValidator());
        RuleFor(dto => dto.UserInformation)
            .SetValidator(new UserUserInformationValidator());
    }
}
