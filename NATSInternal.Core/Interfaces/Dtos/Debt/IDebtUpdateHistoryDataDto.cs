namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IDebtUpdateHistoryDataDto
{
    long Amount { get; set; }
    string Note { get; set; }
    DateTime StatsDateTime { get; set; }
}