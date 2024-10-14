namespace NATSInternal.Services.Dtos
{
    public class OrderUpsertRequestDto
        : IProductExportableUpsertRequestDto<OrderItemRequestDto, OrderPhotoRequestDto>
    {
        public DateTime? StatsDateTime { get; set; }
        public string Note { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItemRequestDto> Items { get; set; }
        public List<OrderPhotoRequestDto> Photos { get; set; }
        public string UpdatedReason { get; set; }
        
        public void TransformValues()
        {
        }
    }
}