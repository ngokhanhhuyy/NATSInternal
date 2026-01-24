using System.ComponentModel;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Web.Models.Shared;

public class PhotoBasicModel
{
    #region Constructors
    public PhotoBasicModel(PhotoBasicResponseDto responseDto)
    {
        Id = responseDto.Id;
        Url = responseDto.Url;
        IsThumbnail = responseDto.IsThumbnail;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string Url { get; }
    public bool IsThumbnail { get; }
    #endregion
}
