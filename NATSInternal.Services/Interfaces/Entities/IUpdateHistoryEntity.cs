namespace NATSInternal.Services.Interfaces.Entities;

internal interface IUpdateHistoryEntity : IEntity
{
    DateTime UpdatedDateTime { get; set; }
    string Reason { get; set; }
    string OldData { get; set; }
    string NewData { get; set; }

    // Foreign keys
    int UserId { get; set; }

    // Navigation properties
    User User { get; set; }
}