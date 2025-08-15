namespace NATSInternal.Core.Validation.Validators;

internal class UserUserInformationValidator : Validator<UserUserInformationRequestDto>
{
    public UserUserInformationValidator()
    {
        RuleFor(dto => dto.JoiningDate)
            .Must(EqualOrEarlierThanToday)
            .WithMessage(ErrorMessages.EarlierThanOrEqualToToday
                .Replace("{Today}", DateOnly.FromDateTime(DateTime.Today).ToString("dd-MM-yyyy")))
            .WithName(dto => DisplayNames.Get(nameof(dto.JoiningDate)));
        RuleFor(dto => dto.Note)
            .MaximumLength(255)
            .WithName(dto => DisplayNames.Get(nameof(dto.Note)));
        RuleFor(dto => dto.RoleName)
            .NotEmpty()
            .WithName(dto => dto.RoleName);
    }
}
