namespace NATSInternal.Services.Dtos;

public class ProductCategoryRequestDto : IRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public void TransformValues()
    {
        Name = Name?.ToNullIfEmpty();
    }
}