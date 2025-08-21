namespace NATSInternal.Core.Dtos;

public class CustomerUpsertRequestDto : IRequestDto {
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string NickName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly? Birthday { get; set; }
    public string PhoneNumber { get; set; }
    public string ZaloNumber { get; set; }
    public string FacebookUrl { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Note { get; set; }
    public int? IntroducerId { get; set; }

    public void TransformValues() {
        FirstName = FirstName?.ToNullIfEmptyOrWhiteSpace();
        MiddleName = MiddleName?.ToNullIfEmptyOrWhiteSpace();
        LastName = LastName?.ToNullIfEmptyOrWhiteSpace();
        NickName = NickName?.ToNullIfEmptyOrWhiteSpace();
        PhoneNumber = PhoneNumber?.ToNullIfEmptyOrWhiteSpace();
        ZaloNumber = ZaloNumber?.ToNullIfEmptyOrWhiteSpace();
        FacebookUrl = FacebookUrl?.ToNullIfEmptyOrWhiteSpace();
        Email = Email?.ToNullIfEmptyOrWhiteSpace();
        Address = Address?.ToNullIfEmptyOrWhiteSpace();
        Note = Note?.ToNullIfEmptyOrWhiteSpace();
        IntroducerId = !IntroducerId.HasValue || IntroducerId.Value == 0
            ? null : IntroducerId.Value;
    }
}