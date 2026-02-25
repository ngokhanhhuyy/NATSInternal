using MediatR;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Products;

public class CountryGetAllRequestDto : IRequest<ICollection<CountryBasicResponseDto>>;