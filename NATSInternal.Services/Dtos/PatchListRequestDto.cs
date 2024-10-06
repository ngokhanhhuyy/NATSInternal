namespace NATSInternal.Services.Dtos;

public class PatchListRequestDto<TRequestDto> : IRequestDto
        where TRequestDto : IRequestDto {
    public List<PatchRequestDto<TRequestDto>> Items { get; set; }

    public void TransformValues() {
        Items = Items
            .Select(item => {
                item.Data = item.Data.TransformValues();
                return item;
            }).ToList();
    }
}
