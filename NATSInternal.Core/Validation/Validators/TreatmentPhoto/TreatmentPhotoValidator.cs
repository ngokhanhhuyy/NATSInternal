namespace NATSInternal.Validation.Validators;

public class TreatmentPhotoValidator : Validator<TreatmentPhotoRequestDto>
{
    public TreatmentPhotoValidator()
    {
        RuleFor(dto => dto.File)
            .IsValidImage()
            .When(dto => !dto.Id.HasValue)
            .WithName(DisplayNames.File);
    }
}
