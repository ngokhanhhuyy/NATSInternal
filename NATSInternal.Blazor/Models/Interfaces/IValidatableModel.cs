namespace NATSInternal.Blazor.Models.Interfaces;

public interface IValidatableModel<TRequestDto>
{
    TRequestDto ToRequestDto();
}
