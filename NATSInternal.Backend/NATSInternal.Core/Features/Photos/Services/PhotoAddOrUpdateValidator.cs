using FluentValidation;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Photos;

internal class PhotoAddOrUpdateValidator : Validator<PhotoCreateOrUpdateRequestDto>
{
    #region Constructors
    public PhotoAddOrUpdateValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.File)
                .IsValidImage()
                .WithName(DisplayNames.File);
        });

        RuleSet("CreateAndUpdate", () =>
        {
            RuleFor(dto => dto.File)
                .NotNull()
                .When(dto => !dto.Id.HasValue)
                .IsValidImage()
                .WithName(DisplayNames.File);
        });
    }
    #endregion
}