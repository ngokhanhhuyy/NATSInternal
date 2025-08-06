namespace NATSInternal.Core.Dtos;

public class IntroducerSearchResponseDto {
    public int PageCount { get; set; }
    public int ResultsCount { get; set; }
    public List<CustomerBasicResponseDto> Results { get; set; }
}