using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NATSInternal.Application.Localization;

namespace NATSInternal.Web.Models;

public class CustomerUpsertIntroducerModel
{
    #region Properties
    [BindRequired]
    [DisplayName(DisplayNames.Id)]
    public Guid? PickedIntroducerId { get; set; }
    
    [BindNever]
    public CustomerBasicModel? PickedIntroducer { get; set; }

    [BindNever]
    public CustomerListModel CustomerList { get; set; } = new();
    #endregion
}