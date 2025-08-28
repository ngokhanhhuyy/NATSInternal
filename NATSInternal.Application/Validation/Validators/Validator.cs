using FluentValidation;
using ImageMagick;
using NATSInternal.Application.UseCases;

namespace NATSInternal.Application.Validation.Validators;

public class Validator<TRequestDto> : AbstractValidator<TRequestDto> where TRequestDto : IRequestDto
{
    #region Constructors
    public Validator()
    {
        ClassLevelCascadeMode = CascadeMode.Continue;
        RuleLevelCascadeMode = CascadeMode.Stop;
    }
    #endregion

    #region ProtectedMethods
    protected virtual bool EqualOrEarlierThanToday(DateTime value)
    {
        return value <= DateTime.UtcNow.Date;
    }

    protected virtual bool EqualOrEarlierThanToday(DateTime? value)
    {
        if (value.HasValue)
        {
            return EqualOrEarlierThanToday(value.Value);
        }
        return true;
    }

    protected virtual bool EqualOrEarlierThanToday(DateOnly value)
    {
        return value.ToDateTime(new TimeOnly(0, 0)) <= DateTime.UtcNow.Date;
    }

    protected virtual bool EqualOrEarlierThanToday(DateOnly? value)
    {
        if (value.HasValue)
        {
            return EqualOrEarlierThanToday(value.Value);
        }
        return true;
    }

    protected virtual bool IsValidImage(byte[] imageAsBytes)
    {
        try
        {
            MagickImage _ = new(imageAsBytes);
            return true;
        }
        catch (MagickMissingDelegateErrorException)
        {
            return false;
        }
    }

    protected virtual bool IsEnumElementName<TEnum>(string? name) where TEnum : Enum
    {
        return name is not null && Enum.GetNames(typeof(TEnum)).ToList().Contains(name);
    }
    #endregion
}