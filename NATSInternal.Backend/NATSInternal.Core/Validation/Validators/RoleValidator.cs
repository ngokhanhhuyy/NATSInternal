namespace NATSInternal.Core.Validation.Validators;

internal class RoleValidator : Validator<RoleRequestDto>
{
    public RoleValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithName(dto => DisplayNames.Get(nameof(dto.Name)));
    }
}
