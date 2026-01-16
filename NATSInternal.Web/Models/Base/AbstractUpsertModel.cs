namespace NATSInternal.Web.Models;

public abstract class AbstractUpsertModel
{
    #region Properties
    public int CurrentStep { get; set; }
    public int NextStep { get; set; }
    #endregion
}