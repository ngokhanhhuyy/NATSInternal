using JetBrains.Annotations;
using System.Reflection;
using NATSInternal.Application.Localization;

namespace NATSInternal.Application.UseCases.Metadata;

public class MetadataGetResponseDto
{
    #region Fields
    private static readonly IDictionary<string, string> _displayNames;
    #endregion

    #region Constructors
    static MetadataGetResponseDto()
    {
        FieldInfo[] fields = typeof(DisplayNames).GetFields(BindingFlags.Public | BindingFlags.Static);
        _displayNames = fields
            .Where(f => f.GetValue(null) is not null)
            .ToDictionary(f => f.Name, f => (string)f.GetValue(null)!);
    }

    public MetadataGetResponseDto()
    {
        DisplayNames = _displayNames;
    }
    #endregion

    #region Properties
    public IDictionary<string, string> DisplayNames { get; }
    public required IDictionary<string, MetadataListOptionsResponseDto> ListOptionsList { get; init; }
    public required IDictionary<string, bool> CreatingAuthorizationList { get; init; }
    #endregion
}

public class MetadataListOptionsResponseDto
{
    #region Constructors
    public MetadataListOptionsResponseDto(
        string resourceName,
        IEnumerable<string>? sortByFieldNameOptions = null,
        string? defaultSortByFieldName = null,
        bool? defaultSortByAscending = null,
        int? defaultResultsPerPage = null)
    {
        ResourceName = resourceName;
        SortByFieldNameOptions = sortByFieldNameOptions ?? new List<string>();
        DefaultSortByFieldName = defaultSortByFieldName;
        DefaultSortByAscending = defaultSortByAscending;
        DefaultResultsPerPage = defaultResultsPerPage;
    }
    #endregion

    #region Properties
    public string ResourceName { [UsedImplicitly] get; }
    public IEnumerable<string> SortByFieldNameOptions { [UsedImplicitly] get; }
    public string? DefaultSortByFieldName { [UsedImplicitly] get; }
    public bool? DefaultSortByAscending { [UsedImplicitly] get; }
    public int? DefaultResultsPerPage { [UsedImplicitly] get; }
    #endregion
}

public class MetadataListOptionsResponseDto<TOptionEnum> : MetadataListOptionsResponseDto
    where TOptionEnum : struct, Enum 
{
    #region Constructors
    static MetadataListOptionsResponseDto()
    {
        SortByFieldOptions = Enum.GetValues<TOptionEnum>().ToList();
    }

    public MetadataListOptionsResponseDto(
        string resourceName,
        TOptionEnum? defaultSortByField = null,
        bool? defaultSortByAscending = null,
        int? defaultResultsPerPage = null) : base(
            resourceName,
            SortByFieldOptions.Select(f => f.ToString()),
            defaultSortByField?.ToString(),
            defaultSortByAscending,
            defaultResultsPerPage)
    {
    }
    #endregion

    #region StaticProperties
    private static IEnumerable<TOptionEnum> SortByFieldOptions { get; }
    #endregion
}