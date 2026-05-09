using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Contracts;
using NATSInternal.Core.Common.Entities;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Users;
using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Features.Payments;

internal class Payment : IHasStatsEntity
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    public required DateOnly StatsDate { get; set; }

    [Required]
    public required PaymentType Type { get; set; }

    [Required]
    public required long Amount { get; set; }

    [StringLength(HasStatsContracts.NoteMaxLength)]
    public string? Note { get; set; }

    [Required]
    public required DateTime CreatedDateTime { get; set; }

    public DateTime? LastUpdatedDateTime { get; set; }

    public DateTime? DeletedDateTime { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Required]
    public required int CustomerId { get; set; }

    [Required]
    public required int CreatedUserId { get; set; }

    public int? LastUpdatedUserId { get; set; }

    public int? DeletedUserId { get; set; }
    #endregion

    #region NavigationProperties
    public Customer Customer { get; set; } = null!;
    public User CreatedUser { get; set; } = null!;
    public User? LastUpdatedUser { get; set; } = null!;
    public User? DeletedUser { get; set; } = null!;
    #endregion
}