namespace NATSInternal.Web.Models;

public class SuccessfulSubmissionConfirmationModel
{
    #region Constructors
    public SuccessfulSubmissionConfirmationModel(string? returningUrl)
    {
        ReturningUrl = returningUrl;
    }
    #endregion

    #region Properties
    public string? ReturningUrl { get; set; }
    #endregion
}