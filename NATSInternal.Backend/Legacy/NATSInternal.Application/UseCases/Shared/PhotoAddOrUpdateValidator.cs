using FluentValidation;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Shared;

internal class PhotoAddOrUpdateValidator : Validator<PhotoCreateOrUpdateRequestDto>
{
    #region Constructors
    public PhotoAddOrUpdateValidator()
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
                .WithName(DisplayNames.File);
        });
    }
    #endregion
}