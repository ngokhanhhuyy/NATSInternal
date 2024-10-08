namespace NATSInternal.Services.Dtos
{
    public class OrderUpsertRequestDto : IRequestDto
    {
        public DateTime? PaidDateTime { get; set; }
        public string Note { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItemRequestDto> Items { get; set; }
        public List<OrderPhotoRequestDto> Photos { get; set; }
        public string UpdateReason { get; set; }
        
        public void TransformValues()
        {
        }
    }
}