using FluentValidation;
using MediatR;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Photos;

namespace NATSInternal.Application.UseCases.Photos;

internal class PhotoGetMultipleByProductIdsHandler
    : IRequestHandler<PhotoGetMultipleByProductIdsRequestDto, ICollection<PhotoBasicResponseDto>>
{
    #region Fields
    private readonly IPhotoRepository _repository;
    private readonly IValidator<PhotoGetMultipleByProductIdsRequestDto> _validator;
    #endregion

    #region Constructors
    public PhotoGetMultipleByProductIdsHandler(
        IPhotoRepository repository,
        IValidator<PhotoGetMultipleByProductIdsRequestDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    #endregion

    #region Methods
    public async Task<ICollection<PhotoBasicResponseDto>> Handle(
        PhotoGetMultipleByProductIdsRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(requestDto);

        ICollection<Photo> photos = await _repository.GetMultiplePhotosByProductIdsAsync(
            requestDto.ProductIds,
            cancellationToken);

        return photos.Select(p => new PhotoBasicResponseDto(p)).ToList();
    }
    #endregion
}
