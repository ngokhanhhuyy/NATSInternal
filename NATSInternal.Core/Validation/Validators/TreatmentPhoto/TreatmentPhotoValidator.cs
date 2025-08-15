namespace NATSInternal.Core.Validation.Validators;

internal class TreatmentPhotoValidator : Validator<TreatmentPhotoRequestDto>
{
    public TreatmentPhotoValidator()
    {
        RuleFor(dto => dto.File)
            .IsValidImage()
            .When(dto => !dto.Id.HasValue)
            .WithName(DisplayNames.File);
    }
}
