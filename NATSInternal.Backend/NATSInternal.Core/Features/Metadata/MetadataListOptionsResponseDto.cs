namespace NATSInternal.Core.Features.Metadata;

public class MetadataListOptionsResponseDto<TOptionEnum> where TOptionEnum : struct, Enum 
{
    #region Fields
    private static readonly IEnumerable<TOptionEnum> SortByFieldOptions;
    #endregion

    #region Constructors
    static MetadataListOptionsResponseDto()
    {
        SortByFieldOptions = Enum.GetValues<TOptionEnum>().ToList();
    }
    #endregion

    #region Properties
    public required string ResourceName { get; init; }
    public IEnumerable<string> SortByFieldNameOptions => SortByFieldOptions.Select(o => o.ToString());
    public required string? DefaultSortByFieldName { get; init; }
    public required bool? DefaultSortByAscending { get; init; }
    public required int? DefaultResultsPerPage { get; init; }
    #endregion
}
