using NATSInternal.Core.Common.Contracts;
using NATSInternal.Core.Common.Entities;
using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Features.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NATSInternal.Core.Features.Expenses;

internal class Expense : IHasStatsEntity
{
    #region Properties
    public int Id { get; private set; }

    [Required]
    public required DateOnly StatsDate { get; set; }

    [Required]
    public required long Amount { get; set; }

    [Required]
    public required ExpenseType Type { get; set; }

    [StringLength(HasStatsContracts.NoteMaxLength)]
    public string? Note { get; set; }

    [Required]
    public required DateTime CreatedDateTime { get; set; }
    
    public DateTime? LastUpdatedDateTime { get; set; }
    
    public DateTime? DeletedDateTime { get; set; }
    #endregion

    #region ForeignKeyProperties
    public required int CreatedUserId { get; set; }
    public int? LastUpdatedUserId { get; set; }
    public int? DeletedUserId { get; set; }
    #endregion

    #region NavigationProperties
    public User CreatedUser { get; set; } = null!;
    public User? LastUpdatedUser { get; set; }
    public User? DeletedUser { get; set; }
    public List<Photo> Photos { get; set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public Photo? Thumbnail => Photos.FirstOrDefault(p => p.IsThumbnail);
    #endregion
}