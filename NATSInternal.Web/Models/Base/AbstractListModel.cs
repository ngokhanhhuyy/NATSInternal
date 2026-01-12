using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
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

    [DisplayName(DisplayNames.Page)]
    public int? Page { get; set; }

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

    [BindNever]
    public int LargeScreenStartingPage { get; protected set; }

    [BindNever]
    public int LargeScreenEndingPage { get; protected set; }

    [BindNever]
    public int SmallScreenStartingPage { get; protected set; }

    [BindNever]
    public int SmallScreenEndingPage { get; protected set; }

    [BindNever]
    public abstract IReadOnlyDictionary<string, string> SortByFieldNameOptions { get; }
    #endregion
    
    #region AbstractMethods
    public abstract string GetCreateRoutePath(IUrlHelper urlHelper);
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
    #region Fields
    private static readonly Dictionary<string, string> _sortByFieldNameOptions;
    #endregion
    #region Constructors
    static AbstractListModel()
    {
        _sortByFieldNameOptions = Enum
            .GetNames<TFieldToSort>()
            .ToDictionary(name => name, DisplayNames.Get);
    }
    #endregion
    
    #region Properties
    [BindNever]
    [DisplayName(DisplayNames.Results)]
    public IReadOnlyList<TItemModel> Items { get; protected set; } = new List<TItemModel>();

    public override IReadOnlyDictionary<string, string> SortByFieldNameOptions => _sortByFieldNameOptions;
    #endregion

    #region Methods
    public virtual TListRequestDto ToRequestDto()
    {
        TListRequestDto requestDto = new();
        
        // Map SortByAscending.
        if (SortByAscending.HasValue)
        {
            requestDto.SortByAscending = SortByAscending.Value;
        }
        else
        {
            SortByAscending = requestDto.SortByAscending;
        }

        // Map SortByFieldName.
        if (SortByFieldName is not null)
        {
            requestDto.SortByFieldName = SortByFieldName;
        }
        else
        {
            SortByFieldName = requestDto.SortByFieldName;
        }

        // Map Page.
        if (Page.HasValue)
        {
            requestDto.Page = Page.Value;
        }
        else
        {
            Page = requestDto.Page;
        }

        // Map ResultsPerPage.
        if (ResultsPerPage.HasValue)
        {
            requestDto.ResultsPerPage = ResultsPerPage.Value;
        }
        else
        {
            ResultsPerPage = requestDto.ResultsPerPage;
        }

        requestDto.SearchContent = SearchContent;

        return requestDto;
    }

    public virtual void MapFromResponseDto(TListResponseDto responseDto)
    {
        PageCount = responseDto.PageCount;
        ItemsCount = responseDto.ItemCount;
        (LargeScreenStartingPage, LargeScreenEndingPage) = CalculateRange(5);
        (SmallScreenStartingPage, SmallScreenEndingPage) = CalculateRange(3);
        MapItemsFromResponseDtos(responseDto.Items);
    }
    #endregion
    
    #region ProtectedMethods
    protected abstract void MapItemsFromResponseDtos(IEnumerable<TItemResponseDto> responseDtos);
    #endregion

    #region PrivateMethods
    private (int, int) CalculateRange(int visibleButtonCount)
    {
        int currentPage = Page ?? 1;
        int startingPage;
        int endingPage;
        
        if (PageCount >= visibleButtonCount)
        {
            if (currentPage - (int)Math.Floor((double)visibleButtonCount / 2) <= 1)
            {
                startingPage = 1;
                endingPage = startingPage + (visibleButtonCount - 1);
            }
            else if (currentPage + (int)Math.Floor((double)visibleButtonCount / 2) > PageCount)
            {
                endingPage = PageCount;
                startingPage = endingPage - (visibleButtonCount - 1);
            }
            else
            {
                startingPage = (int)Math.Ceiling(currentPage - (double)visibleButtonCount / 2);
                endingPage = (int)Math.Floor(currentPage + (double)visibleButtonCount / 2);
            }
        }
        else
        {
            startingPage = 1;
            endingPage = PageCount;
        }

        return (startingPage, endingPage);
    }
    #endregion
}