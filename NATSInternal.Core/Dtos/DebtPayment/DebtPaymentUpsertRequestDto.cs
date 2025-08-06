namespace NATSInternal.Core.Dtos;

/// <summary>
/// A DTO class representing the data from the requests for the debt payment creating and
/// updating operations.
/// </summary>
public class DebtPaymentUpsertRequestDto : IDebtUpsertRequestDto
{
    /// <summary>
    /// Represents the paid amount of the debt.
    /// </summary>
    public long Amount { get; set; }

    /// <summary>
    /// Contains the note about the debt payment.
    /// </summary>
    public string Note { get; set; }

    /// <summary>
    /// Represents the date and time when the debt payment.
    /// </summary>
    /// <remarks>
    /// When this property is left as null, if this DTO instance is consumed in creating mode,
    /// the paid date and time of the creating debt payment will be the current date and
    /// time. If this is DTO instance is consumed in the updating mode, the value of the debt
    /// payment's paid date and time will not be modified and left untouched.
    /// </remarks>
    public DateTime? StatsDateTime { get; set; }

    /// <summary>
    /// Represents the id of the customer to which the creating/updating debt payment belongs.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Contains the reason why the debt payment is updated.
    /// </summary>
    public string UpdatedReason { get; set; }
    
    /// <inheritdoc/>
    public void TransformValues()
    {
        Note = Note?.ToNullIfEmpty();
        UpdatedReason?.ToNullIfEmpty();
    }
}