namespace NATSInternal.Services.Dtos;

public class TreatmentDetailResponseDto
    : IProductExportableDetailResponseDto<
        TreatmentItemResponseDto,
        TreatmentPhotoResponseDto,
        TreatmentUpdateHistoryResponseDto,
        TreatmentAuthorizationResponseDto>
{
    public int Id { get; set; }
    public DateTime PaidDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public long ServiceAmount { get; set; }
    public int ServiceVat { get; set; }
    public decimal ServiceVatAmount { get; set; }
    public long ProductAmount { get; set; }
    public long TotalAmountAfterVAT { get; set; }
    public string Note { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public UserBasicResponseDto Therapist { get; set; }
    public List<TreatmentItemResponseDto> Items { get; set; }
    public List<TreatmentPhotoResponseDto> Photos { get; set; }
    public TreatmentAuthorizationResponseDto Authorization { get; set; }
    public List<TreatmentUpdateHistoryResponseDto> UpdateHistories { get; set; }

    public long Amount => ServiceAmount + ProductAmount;
    public string ThumbnailUrl => Photos
        .OrderBy(p => p.Id)
        .Select(p => p.Url)
        .FirstOrDefault();

    internal TreatmentDetailResponseDto(
            Treatment treatment,
            TreatmentAuthorizationResponseDto authorization,
            bool mapUpdateHistories = false)
    {
        Id = treatment.Id;
        PaidDateTime = treatment.PaidDateTime;
        ServiceAmount = treatment.ServiceAmountBeforeVat;
        ServiceVatAmount = treatment.ServiceVatAmount;
        ProductAmount = treatment.ProductAmountBeforeVat;
        TotalAmountAfterVAT = treatment.AmountAfterVat;
        Note = treatment.Note;
        IsLocked = treatment.IsLocked;
        Customer = new CustomerBasicResponseDto(treatment.Customer);
        CreatedUser = new UserBasicResponseDto(treatment.CreatedUser);
        Therapist = new UserBasicResponseDto(treatment.Therapist);
        Items = treatment.Items?.Select(ti => new TreatmentItemResponseDto(ti)).ToList();
        Photos = treatment.Photos?.Select(tp => new TreatmentPhotoResponseDto(tp)).ToList();
        Authorization = authorization;

        if (mapUpdateHistories)
        {
            UpdateHistories = treatment.UpdateHistories
                .Select(uh => new TreatmentUpdateHistoryResponseDto(uh))
                .ToList();
        }
    }
}