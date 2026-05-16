namespace NATSInternal.Core.Features.Metadata;

public class MetadataResponseDto
{
    #region Properties
    public required IDictionary<string, string> DisplayNameList { get; init; }
    public required MetadataListOptionsListResponseDto ListOptionsList { get; init; }
    public required MetadataCreatingAuthorizationListResponseDto CreatingAuthorization { get; init; }
    #endregion
}
