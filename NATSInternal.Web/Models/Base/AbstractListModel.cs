using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases;

namespace NATSInternal.Web.Models;

public abstract class AbstractListModel
{
    #region Properties
    [DisplayName(DisplayNames.SortByAscending)]
    public bool? SortByAscending { get; set; }

    [DisplayName(DisplayNames.SortByFieldName)]
    public string? SortByFieldName { get; set; }

    [DisplayName(DisplayNames.Page)] public int? Page { get; set; }

    [DisplayName(DisplayNames.ResultsPerPage)]
    public int? ResultsPerPage { get; set; }

    [DisplayName(DisplayNames.SearchContent)]
    public string? SearchContent { get; set; }

    [BindNever]
    [DisplayName(DisplayNames.PageCount)]
    public int PageCount { get; protected set; }

    [BindNever]
    [DisplayName(DisplayNames.ResultsCount)]
    public int ItemsCount { get; protected set; }

    public static IReadOnlyDictionary<string, string> SortByFieldNameOptions { get; protected set; } =
        new Dictionary<string, string>();
    #endregion
}

public abstract class AbstractListModel<
        TItemModel,
        TListRequestDto,
        TListResponseDto,
        TItemResponseDto,
        TFieldToSort>
    : AbstractListModel
    where TListRequestDto : IListRequestDto, new()
    where TListResponseDto : IListResponseDto<TItemResponseDto>
    where TFieldToSort : struct, Enum
{
    #region Constructors
    static AbstractListModel()
    {
        SortByFieldNameOptions = Enum
            .GetNames<TFieldToSort>()
            .ToDictionary(name => name, DisplayNames.Get);
    }
    #endregion
    
    #region Properties
    [BindNever]
    [DisplayName(DisplayNames.Results)]
    public IReadOnlyList<TItemModel> Items { get; protected set; } = new List<TItemModel>();
    #endregion

    #region Methods
    public virtual TListRequestDto ToRequestDto()
    {
        TListRequestDto requestDto = new();
        if (SortByAscending.HasValue)
        {
            requestDto.SortByAscending = SortByAscending.Value;
        }

        if (SortByFieldName is not null)
        {
            requestDto.SortByFieldName = SortByFieldName;
        }

        if (Page.HasValue)
        {
            requestDto.Page = Page.Value;
        }

        if (ResultsPerPage.HasValue)
        {
            requestDto.ResultsPerPage = ResultsPerPage.Value;
        }

        requestDto.SearchContent = SearchContent;

        return requestDto;
    }

    public virtual void MapFromResponseDto(TListResponseDto responseDto)
    {
        PageCount = responseDto.PageCount;
        ItemsCount = responseDto.ItemCount;
    }
    #endregion
    
    #region ProtectedMethods
    protected abstract void MapItemsFromResponseDtos(IEnumerable<TItemResponseDto> responseDtos);
    #endregion
}