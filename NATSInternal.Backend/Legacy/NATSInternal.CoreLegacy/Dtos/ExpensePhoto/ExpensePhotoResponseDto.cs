namespace NATSInternal.Core.Dtos;

public class ExpensePhotoResponseDto
{
    public int Id { get; set; }
    public string Url { get; set; }

    internal ExpensePhotoResponseDto(ExpensePhoto photo)
    {
        Id = photo.Id;
        Url = photo.Url;
    }
}