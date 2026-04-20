using FluentValidation;
using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Common.Validation;

public class Validator<TRequestDto> : AbstractValidator<TRequestDto> where TRequestDto : IRequestDto
{
    #region Constructors
    protected Validator()
    {
        ClassLevelCascadeMode = CascadeMode.Continue;
        RuleLevelCascadeMode = CascadeMode.Stop;
    }
    #endregion
}