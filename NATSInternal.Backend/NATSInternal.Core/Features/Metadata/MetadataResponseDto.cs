using JetBrains.Annotations;

namespace NATSInternal.Core.Features.Metadata;

public class MetadataGetResponseDto
{
    #region Properties
    public required IDictionary<string, string> DisplayNameList { [UsedImplicitly] get; init; }
    public required MetadataListOptionsListResponseDto ListOptionsList { [UsedImplicitly] get; init; }
    public required MetadataCreatingAuthorizationListResponseDto CreatingAuthorization
    {
        [UsedImplicitly]
        get;
        init;
    }
    #endregion
}


