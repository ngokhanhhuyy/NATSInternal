using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Supplies;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Supplies;

public class SupplyGetDetailResponseDto
{
    #region Constructors
    internal SupplyGetDetailResponseDto(Supply supply, User? createdUser)
    {
        Id = supply.Id;
        ShipmentFee = supply.ShipmentFee;
        CreatedDateTime = supply.CreatedDateTime;
        CreatedUser = new(createdUser);
    }

    internal SupplyGetDetailResponseDto(
        Supply supply,
        User? createdUser,
        User? lastUpdatedUser) : this(supply, createdUser)
    {
        LastUpdatedDateTime = supply.LastUpdatedDateTime;
        LastUpdatedUser = new(lastUpdatedUser);
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public long ShipmentFee { get; }
    public DateTime CreatedDateTime { get; }
    public UserBasicResponseDto CreatedUser { get; }
    public DateTime? LastUpdatedDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    #endregion
}

public class SupplyGetDetailSupplyItem
{
    #region Constructors
    internal SupplyGetDetailSupplyItem(SupplyItem supplyItem)
    {
        Id = supplyItem.Id;
        AmountPerUnit = supplyItem.AmountPerUnit;
        Quantity = supplyItem.Quantity;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public long AmountPerUnit { get; }
    public int Quantity { get; }
    #endregion
}