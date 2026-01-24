using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Web.Models;

public class BrandBasicModel
{
    #region Constructors
    public BrandBasicModel(BrandBasicResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
    }
    #endregion

    #region Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    #endregion
}