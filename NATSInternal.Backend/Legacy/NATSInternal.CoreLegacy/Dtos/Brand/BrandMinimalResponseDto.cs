namespace NATSInternal.Core.Dtos;

public class BrandMinimalResponseDto : IMinimalResponseDto
{
    #region Properties
    public Guid Id { get; internal set; }
    public string Name { get; internal set; }
    #endregion

    #region Constructors
    internal BrandMinimalResponseDto(Brand brand)
    {
        Id = brand.Id;
        Name = brand.Name;
    }
    #endregion
}
