using MediatR;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

internal class CountryGetAllHandler : IRequestHandler<CountryGetAllRequestDto, ICollection<CountryBasicResponseDto>>
{
    #region Fields
    private readonly IProductRepository _repository;
    #endregion

    #region Constructors
    public CountryGetAllHandler(IProductRepository repository)
    {
        _repository = repository;
    }
    #endregion

    #region Methods
    public async Task<ICollection<CountryBasicResponseDto>> Handle(
        CountryGetAllRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        ICollection<Country> countries = await _repository.GetAllCountryAsync(cancellationToken);
        return countries.Select(c => new CountryBasicResponseDto(c)).ToList();
    }
    #endregion
}