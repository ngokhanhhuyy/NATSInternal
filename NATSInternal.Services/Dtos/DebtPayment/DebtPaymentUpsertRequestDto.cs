namespace NATSInternal.Services.Dtos
{
    public class DebtPaymentUpsertRequestDto : ICustomerEngageableUpsertRequestDto
    {
        public long Amount { get; set; }
        public string Note { get; set; }
        public DateTime? PaidDateTime { get; set; }
        public int CustomerId { get; set; }
        public string UpdatingReason { get; set; }

        public DateTime? StatsDateTime
        {
            get => PaidDateTime;
            set => PaidDateTime = value;
        }
        
        public void TransformValues()
        {
            Note = Note?.ToNullIfEmpty();
            UpdatingReason?.ToNullIfEmpty();
        }
    }
}