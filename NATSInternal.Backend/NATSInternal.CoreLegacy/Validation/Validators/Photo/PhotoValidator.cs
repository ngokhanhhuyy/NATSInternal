namespace NATSInternal.Core.Validation.Validators;

internal class PhotoValidator : Validator<PhotoRequestDto>
{
    #region Constructors
    public PhotoValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.File)
                .Must(IsValidImage).WithMessage(ErrorMessages.Invalid)
                .WithName(DisplayNames.File);
        });

        RuleSet("CreateAndUpdate", () =>
        {
            RuleFor(dto => dto.File)
                .NotNull()
                .When(dto => !dto.Id.HasValue)
                .Must(IsValidImage).WithName(ErrorMessages.Invalid)
                .When(dto => dto.File != null)
                .WithName(DisplayNames.File);
        });
    }
    #endregion
}
