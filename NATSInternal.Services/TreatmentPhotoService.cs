using Microsoft.AspNetCore.Hosting;

namespace NATSInternal.Services;

/// <inheritdoc cref="ITreatmentPhotoService" />
internal class TreatmentPhotoService
    :
        PhotoService<Treatment, TreatmentPhoto>,
        ITreatmentPhotoService
{
    public TreatmentPhotoService(IWebHostEnvironment environment) : base(environment) { }

    /// <inheritdoc />
    public virtual async Task CreateMultipleAsync<TRequestDto>(
            Treatment treatment,
            List<TRequestDto> requestDtos)
        where TRequestDto : TreatmentPhotoRequestDto
    {
        await CreateMultipleAsync(treatment, requestDtos, (photo, requestDto) =>
        {
            photo.Type = requestDto.Type;
        });
    }

    /// <inheritdoc />
    public virtual async Task<(List<string>, List<string>)> UpdateMultipleAsync<TRequestDto>(
            Treatment treatment,
            List<TRequestDto> requestDtos)
        where TRequestDto : TreatmentPhotoRequestDto
    {
        return await UpdateMultipleAsync(
            treatment,
            requestDtos,
            (photo, requestDto) => photo.Type = requestDto.Type,
            (photo, requestDto) => photo.Type = requestDto.Type);
    }
}