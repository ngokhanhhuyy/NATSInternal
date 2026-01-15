namespace NATSInternal.Web.Models;

public abstract class AbstractUpsertModel
{
    #region Properties
    public abstract bool IsFinalSubmission { get; set; }
    #endregion
}