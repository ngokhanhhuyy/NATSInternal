namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IListRequestDto : IRequestDto
{
    int Page { get; set; }
    int ResultsPerPage { get; set; }
}