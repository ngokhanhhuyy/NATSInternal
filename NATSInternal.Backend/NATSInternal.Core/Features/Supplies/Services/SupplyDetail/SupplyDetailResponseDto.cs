using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Features.Supplies;

public class SupplyDetailResponseDto
{
    #region Constructors
    internal SupplyDetailResponseDto(Supply supply, SupplyExistingAuthorizationResponseDto authorization)
    {
        Id = supply.Id;
        ShipmentFee = supply.ShipmentFee;
        CreatedDateTime = supply.CreatedDateTime;
        CreatedUser = new(supply.CreatedUser);
        LastUpdatedDateTime = supply.LastUpdatedDateTime;
        DeletedDateTime = supply.DeletedDateTime;
        Photos = supply.Photos.Select(p => new PhotoBasicResponseDto(p)).ToList();
        Authorization = authorization;

        if (supply.LastUpdatedUser is not null)
        {
            LastUpdatedUser = new(supply.LastUpdatedUser); 
        }

        if (supply.DeletedUser is not null)
        {
            DeletedUser = new(supply.DeletedUser);
        }
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public long ShipmentFee { get; }
    public DateTime CreatedDateTime { get; }
    public UserBasicResponseDto CreatedUser { get; }
    public DateTime? LastUpdatedDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public DateTime? DeletedDateTime { get; }
    public UserBasicResponseDto? DeletedUser { get; }
    public List<PhotoBasicResponseDto> Photos { get; }
    public SupplyExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}