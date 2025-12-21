using Microsoft.AspNetCore.Mvc.ModelBinding;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Domain.Features.Customers;
using System.ComponentModel;

namespace NATSInternal.Web.Models;

public class CustomerListModel : ISearchableListModel<CustomerListCustomerModel>
{
    #region Fields
    private static readonly IReadOnlyList<string> _sortByFieldNameOptions;
    #endregion
    
    #region Constructors
    static CustomerListModel()
    {
        _sortByFieldNameOptions = Enum.GetNames<CustomerGetListRequestDto.FieldToSort>().ToList().AsReadOnly();
    }
    #endregion
    
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
    [DisplayName(DisplayNames.Results)]
    public IReadOnlyList<CustomerListCustomerModel> Items { get; private set; } = new List<CustomerListCustomerModel>();
    
    [BindNever]
    [DisplayName(DisplayNames.PageCount)]
    public int PageCount { get; private set; }
    
    [BindNever]
    [DisplayName(DisplayNames.ResultsCount)]
    public int ItemsCount { get; private set; }
    #endregion

    public IReadOnlyList<string> SortByFieldNameOptions => _sortByFieldNameOptions;
    
    #region Methods
    public CustomerGetListRequestDto ToRequestDto()
    {
        CustomerGetListRequestDto requestDto = new();
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

    public void MapFromResponseDto(CustomerGetListResponseDto responseDto)
    {
        Items = responseDto.Items
            .Select(dto => new CustomerListCustomerModel(dto))
            .ToList()
            .AsReadOnly();
        PageCount = responseDto.PageCount;
        ItemsCount = responseDto.ItemCount;
    }
    #endregion
}

public class CustomerListCustomerModel
{
    #region Constructors
    public CustomerListCustomerModel(CustomerGetListCustomerResponseDto responseDto)
    {
        Id = responseDto.Id;
        FullName = responseDto.FullName;
        NickName = responseDto.NickName;
        Gender = responseDto.Gender;
        Birthday = responseDto.Birthday;
        PhoneNumber = responseDto.PhoneNumber;
        DebtRemainingAmount = responseDto.DebtRemainingAmount;
        Authorization = responseDto.Authorization;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string FullName { get; }
    public string? NickName { get; }
    public Gender Gender { get; }
    public DateOnly? Birthday { get; }
    public string? PhoneNumber { get; }
    public long DebtRemainingAmount { get; }
    public CustomerExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}