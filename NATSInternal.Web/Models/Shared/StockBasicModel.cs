using System.ComponentModel;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Web.Models;

public class StockBasicModel
{
    #region Constructors
    public StockBasicModel(StockBasicResponseDto responseDto)
    {
        Id = responseDto.Id;
        StockingQuantity = responseDto.StockingQuantity;
        ResupplyThresholdQuantity = responseDto.ResupplyThresholdQuantity;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    
    [DisplayName(DisplayNames.StockingQuantity)]
    public int StockingQuantity { get; }
    
    [DisplayName(DisplayNames.ResupplyThresholdQuantity)]
    public int? ResupplyThresholdQuantity { get; }
    #endregion
}