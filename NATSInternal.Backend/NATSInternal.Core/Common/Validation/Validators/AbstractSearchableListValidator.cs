using FluentValidation;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

internal abstract class AbstractSearchableListValidator<TListRequestDto, TFieldToSort>
    : AbstractListValidator<TListRequestDto, TFieldToSort>
    where TListRequestDto : ISearchableListRequestDto
    where TFieldToSort : struct, Enum
{
    protected AbstractSearchableListValidator()
    {
        RuleFor(dto => dto.SearchContent)
            .MaximumLength(255)
            .WithName(DisplayNames.SearchContent);
    }
}