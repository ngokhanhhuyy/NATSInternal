namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductExportableDetailResponseDto<
        TItem,
        TPhoto,
        TUpdateHistory,
        TAuthorization>
    :
        IProductEngageableDetailResponseDto<
            TItem,
            TPhoto,
            TUpdateHistory,
            TAuthorization>,
        IRevuenueBasicResponseDto<TAuthorization>
    where TItem : IProductEngageableItemResponseDto
    where TPhoto : IPhotoResponseDto
    where TUpdateHistory : IUpdateHistoryResponseDto
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto;