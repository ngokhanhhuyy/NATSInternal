using MediatR;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Application.UseCases.Users;
using NATSInternal.Domain.Features.Customers;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Metadata;

public class MetadataGetHandler : IRequestHandler<Unit, MetadataGetResponseDto>
{
    #region Methods
    public async Task<MetadataGetResponseDto> Handle(
        MetadatagetRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        Dictionary<string, MetadataListOptionsResponseDto> listOptionsList = new();

        UserGetListRequestDto userGetListRequestDto = new();
        listOptionsList.Add(
            nameof(User),
            new MetadataListOptionsResponseDto<UserGetListRequestDto.FieldToSort>(
                resourceName: nameof(User),
                defaultSortByField: UserGetListRequestDto.FieldToSort.CreatedDateTime,
                userGetListRequestDto.SortByAscending,
                userGetListRequestDto.ResultsPerPage
            ));

        CustomerGetListRequestDto customerGetListRequestDto = new();
        listOptionsList.Add(
            nameof(Customer),
            new MetadataListOptionsResponseDto<CustomerGetListRequestDto.FieldToSort>(
                resourceName: nameof(User),
                defaultSortByField: UserGetListRequestDto.FieldToSort.CreatedDateTime,
                userGetListRequestDto.SortByAscending,
                userGetListRequestDto.ResultsPerPage
            ));

        UserGetListRequestDto productGetListRequestDto = new();
        listOptionsList.Add(
            nameof(User),
            new MetadataListOptionsResponseDto<UserGetListRequestDto.FieldToSort>(
                resourceName: nameof(User),
                defaultSortByField: UserGetListRequestDto.FieldToSort.CreatedDateTime,
                userGetListRequestDto.SortByAscending,
                userGetListRequestDto.ResultsPerPage
            ));
    }
    #endregion
}