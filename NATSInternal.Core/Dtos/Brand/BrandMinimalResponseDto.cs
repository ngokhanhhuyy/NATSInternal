namespace NATSInternal.Core.Dtos;

public class BrandMinimalResponseDto : IMinimalResponseDto
{
    public int Id { get; internal set; }
    public string Name { get; internal set; }

    internal BrandMinimalResponseDto(Brand brand)
    {
        Id = brand.Id;
        Name = brand.Name;
    }
}
