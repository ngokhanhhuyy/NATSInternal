namespace NATSInternal.Web.Models;

public class DeleteConfirmationModel
{
    #region Properties
    public required string ActionUrl { get; set; }
    public required string CancelUrl { get; set; }
    public required string SuccessUrl { get; set; }
    #endregion
}