using System.ComponentModel;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Web.Models;

public class StockUpsertModel
{
    #region Properties
    [DisplayName(DisplayNames.StockingQuantity)]
    public int StockingQuantity { get; set; }
    
    [DisplayName(DisplayNames.ResupplyThresholdQuantity)]
    public int ResupplyThresholdQuantity { get; set; }
    #endregion
}