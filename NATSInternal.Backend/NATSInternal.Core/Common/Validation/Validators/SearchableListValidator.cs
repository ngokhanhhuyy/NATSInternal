using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

[UsedImplicitly]
internal class SearchableListValidator<TListRequestDto, TFieldToSort> : Validator<TListRequestDto>
    where TListRequestDto : ISearchableListRequestDto
    where TFieldToSort : struct, Enum
{
    public SearchableListValidator()
    {
        Include(new ListValidator<TListRequestDto, TFieldToSort>());
        
        RuleFor(dto => dto.SearchContent)
            .MinimumLength(3)
            .MaximumLength(255)
            .WithName(DisplayNames.SearchContent);
    }
}