﻿namespace NATSInternal.Services.Dtos;

public class BrandUpsertRequestDto : IRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Website { get; set; }
    public string SocialMediaUrl { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; }
    public int? CountryId { get; set; }

    public void TransformValues()
    {
        Name = Name?.ToNullIfEmpty();
        Website = Website?.ToNullIfEmpty();
        SocialMediaUrl = SocialMediaUrl?.ToNullIfEmpty();
        PhoneNumber = PhoneNumber?.ToNullIfEmpty();
        Email = Email?.ToNullIfEmpty();
        Address = Address?.ToNullIfEmpty();
        
        if (CountryId == 0)
        {
            CountryId = null;
        }
    }
}
