namespace NATSInternal.Core.Interfaces.Dtos;

public interface IListRequestDto : IRequestDto
{
    int Page { get; set; }
    int ResultsPerPage { get; set; }
}