using MediatR;

namespace NATSInternal.Application.UseCases.Metadata;

public class MetadatagetRequestDto : IRequest<MetadataGetResponseDto>;