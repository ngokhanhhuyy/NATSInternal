using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NATSInternal.Web.Models;

public class CustomerUpsertIntroducerModel
{
    #region Properties
    [BindNever]
    public CustomerBasicModel? PickedIntroducer { get; set; }

    [BindNever]
    public CustomerListModel CustomerList { get; set; } = new();
    #endregion
}