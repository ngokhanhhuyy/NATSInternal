namespace NATSInternal.Core.Dtos;

public class IntroducerSearchRequestDto : IRequestDto {
    public string SearchByContent { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues() {
        SearchByContent = SearchByContent?.ToNullIfEmptyOrWhiteSpace();
    }
}