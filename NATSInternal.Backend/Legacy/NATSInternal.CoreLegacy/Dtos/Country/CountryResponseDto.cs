namespace NATSInternal.Core.Dtos;

public class CountryResponseDto : IMinimalResponseDto
{
    #region Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    #endregion

    #region Constructors
    internal CountryResponseDto(Country country)
    {
        Id = country.Id;
        Name = country.Name;
        Code = country.Code;
    }
    #endregion
}
