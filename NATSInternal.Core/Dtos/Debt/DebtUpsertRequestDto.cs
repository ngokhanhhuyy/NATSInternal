namespace NATSInternal.Core.Dtos;

/// <summary>
/// A DTO class representing the data from the requests for the debt incurrence creating and
/// updating operations.
/// </summary>
public class DebtUpsertRequestDto : IDebtUpsertRequestDto
{
    #region Properties
    /// <summary>
    /// Represents the type of the debt (incurrence or payment).
    /// </summary>
    public DebtType Type { get; set; }
    
    /// <summary>
    /// Represents the amount of the debt.
    /// </summary>
    public long Amount { get; set; }

    /// <summary>
    /// Contains the note about the debt.
    /// </summary>
    public string Note { get; set; }

    /// <summary>
    /// Represents the date and time when the debt.
    /// </summary>
    /// <remarks>
    /// When this property is left as null, if this DTO instance is consumed in creating mode, the date and time of the
    /// creating debt will be the current date and time. If this is DTO instance is consumed in the updating mode, the
    /// value of the debt's date and time will not be modified and left untouched.
    /// </remarks>
    public DateTime? StatsDateTime { get; set; }

    /// <summary>
    /// Represents the id of the customer to which the creating/updating debt incurrence belongs.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Contains the reason why the debt incurrence is updated.
    /// </summary>
    public string UpdatedReason { get; set; }
    #endregion
    
    #region Methods
    /// <inheritdoc/>
    public void TransformValues()
    {
        Note = Note?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion
}