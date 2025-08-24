namespace NATSInternal.Core.Dtos;

public class CustomerUpsertRequestDto : IRequestDto {
    #region Properties
    public required string FirstName { get; set; }
    public required string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public required string? NickName { get; set; }
    public required Gender Gender { get; set; }
    public required DateOnly? Birthday { get; set; }
    public required string? PhoneNumber { get; set; }
    public required string? ZaloNumber { get; set; }
    public required string? FacebookUrl { get; set; }
    public required string? Email { get; set; }
    public required string? Address { get; set; }
    public required string? Note { get; set; }
    public required Guid? IntroducerId { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        MiddleName = MiddleName?.ToNullIfEmptyOrWhiteSpace();
        NickName = NickName?.ToNullIfEmptyOrWhiteSpace();
        PhoneNumber = PhoneNumber?.ToNullIfEmptyOrWhiteSpace();
        ZaloNumber = ZaloNumber?.ToNullIfEmptyOrWhiteSpace();
        FacebookUrl = FacebookUrl?.ToNullIfEmptyOrWhiteSpace();
        Email = Email?.ToNullIfEmptyOrWhiteSpace();
        Address = Address?.ToNullIfEmptyOrWhiteSpace();
        Note = Note?.ToNullIfEmptyOrWhiteSpace();
        IntroducerId = IntroducerId == Guid.Empty ? null : IntroducerId;
    }
    #endregion
}