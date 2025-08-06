namespace NATSInternal.Core.Dtos;

/// <summary>
/// A DTO class representing the data from the requests for the debt incurrence creating and
/// updating operations.
/// </summary>
public class DebtIncurrenceUpsertRequestDto : IDebtUpsertRequestDto
{
    /// <summary>
    /// Represents the incurred amount of the debt.
    /// </summary>
    public long Amount { get; set; }

    /// <summary>
    /// Contains the note about the debt incurrence.
    /// </summary>
    public string Note { get; set; }

    /// <summary>
    /// Represents the date and time when the debt incurred.
    /// </summary>
    /// <remarks>
    /// When this property is left as null, if this DTO instance is consumed in creating mode,
    /// the incurred date and time of the creating debt incurrence will be the current date and
    /// time. If this is DTO instance is consumed in the updating mode, the value of the debt
    /// incurrence's incurred date and time will not be modified and left untouched.
    /// </remarks>
    public DateTime? StatsDateTime { get; set; }

    /// <summary>
    /// Represents the id of the customer to which the creating/updating debt incurrence
    /// belongs.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Contains the reason why the debt incurrence is updated.
    /// </summary>
    public string UpdatedReason { get; set; }
    
    /// <inheritdoc/>
    public void TransformValues()
    {
        Note = Note?.ToNullIfEmpty();
    }
}