using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Photos;

namespace NATSInternal.Application.UseCases.Photos;

internal class PhotoGetSingleHandler : IRequestHandler<PhotoGetSingleRequestDto, PhotoBasicResponseDto>
{
    #region Fields
    private readonly IPhotoRepository _repository;
    #endregion
    
    #region Constructors
    public PhotoGetSingleHandler(IPhotoRepository repository)
    {
        _repository = repository;
    }
    #endregion
    
    #region Methods
    public async Task<PhotoBasicResponseDto> Handle(
        PhotoGetSingleRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        Photo photo = await _repository
            .GetSinglePhotoByIdAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        return new(photo);
    }
    #endregion
}