namespace NATSInternal.Core.Dtos;

public class ConsultantListResponseDto : IUpsertableListResponseDto<
        ConsultantBasicResponseDto,
        ConsultantExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<ConsultantBasicResponseDto> Items { get; set; }
}