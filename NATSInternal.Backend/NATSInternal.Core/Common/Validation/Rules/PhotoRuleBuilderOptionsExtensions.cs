using FluentValidation;
using ImageMagick;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Common.Validation;

internal static class PhotoRuleBuilderOptionsExtensions
{
    #region ExtensionMethods
    extension<T>(IRuleBuilder<T, List<PhotoUpsertRequestDto>> ruleBuilder)
    {
        public IRuleBuilderOptions<T, List<PhotoUpsertRequestDto>> ContainsNoOrOneThumbnail()
        {
            return ruleBuilder
                .Must((_, photos) => photos.Count(p => p.IsThumbnail) <= 1)
                .WithMessage(ErrorMessages.PhotosCannotContainsMoreThanOneThumbnail
                    .Replace("{Photos}", DisplayNames.Photo.ToLower())
                    .Replace("{Thumbnail}", DisplayNames.Thumbnail.ToLower()))
                .WithName(DisplayNames.Photo);
        }
    }

    extension<T>(IRuleBuilder<T, byte[]?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, byte[]?> IsValidImage()
        {
            return ruleBuilder.Must(file =>
            {
                if (file is null)
                {
                    return true;
                }
                
                try
                {
                    MagickImage _ = new(file);
                    return true;
                }
                catch (MagickMissingDelegateErrorException)
                {
                    return false;
                }
            }).WithMessage(ErrorMessages.Invalid);
        }
    }
    #endregion
}