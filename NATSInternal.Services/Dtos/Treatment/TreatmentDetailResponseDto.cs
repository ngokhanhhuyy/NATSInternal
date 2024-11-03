namespace NATSInternal.Services.Dtos;

public class TreatmentDetailResponseDto
    : IProductExportableDetailResponseDto<
        TreatmentItemResponseDto,
        TreatmentPhotoResponseDto,
        TreatmentUpdateHistoryResponseDto,
        TreatmentItemUpdateHistoryDataDto,
        TreatmentExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public DateTime StatsDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public long ServiceAmountBeforeVat { get; set; }
    public long ServiceVatAmount { get; set; }
    public string Note { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public UserBasicResponseDto Therapist { get; set; }
    public List<TreatmentItemResponseDto> Items { get; set; }
    public List<TreatmentPhotoResponseDto> Photos { get; set; }
    public TreatmentExistingAuthorizationResponseDto Authorization { get; set; }
    public List<TreatmentUpdateHistoryResponseDto> UpdateHistories { get; set; }

    [JsonIgnore]
    public long ProductAmount => Items.Sum(i => i.ProductAmountPerUnit * i.Quantity);

    [JsonIgnore]
    public long ProductVatAmount => Items.Sum(i => i.VatAmountPerUnit * i.Quantity);

    [JsonIgnore]
    public long AmountBeforeVat => ProductAmount + ServiceAmountBeforeVat;

    [JsonIgnore]
    public long VatAmount => ProductVatAmount + ServiceVatAmount;

    [JsonIgnore]
    public long AmountAfterVat => ServiceAmountBeforeVat + ProductAmount;

    public string ThumbnailUrl => Photos
        .OrderBy(p => p.Id)
        .Select(p => p.Url)
        .FirstOrDefault();

    internal TreatmentDetailResponseDto(
            Treatment treatment,
            TreatmentExistingAuthorizationResponseDto authorization)
    {
        Id = treatment.Id;
        StatsDateTime = treatment.StatsDateTime;
        ServiceAmountBeforeVat = treatment.ServiceAmountBeforeVat;
        ServiceVatAmount = treatment.ServiceVatAmount;
        Note = treatment.Note;
        IsLocked = treatment.IsLocked;
        Customer = new CustomerBasicResponseDto(treatment.Customer);
        CreatedUser = new UserBasicResponseDto(treatment.CreatedUser);
        Therapist = new UserBasicResponseDto(treatment.Therapist);
        Items = treatment.Items?.Select(ti => new TreatmentItemResponseDto(ti)).ToList();
        Photos = treatment.Photos?.Select(tp => new TreatmentPhotoResponseDto(tp)).ToList();
        Authorization = authorization;
        UpdateHistories = treatment.UpdateHistories
            .Select(uh => new TreatmentUpdateHistoryResponseDto(uh))
            .ToList();
    }
}