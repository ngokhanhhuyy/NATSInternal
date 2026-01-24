using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Web.Extensions;
using NATSInternal.Web.Models;

namespace NATSInternal.Web.Controllers;

[Route("san-pham")]
[Authorize]
public class ProductController : Controller
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Methods
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ProductListModel model, CancellationToken token)
    {
        // BrandList.
        BrandGetListRequestDto brandListRequestDto = new() { ResultsPerPage = 7 };
        BrandGetListResponseDto brandListResponseDto = await _mediator.Send(brandListRequestDto, token);
        model.BrandList.MapFromBrandListResponseDto(brandListResponseDto);
        
        // ProductCategoryList.
        ProductCategoryGetListRequestDto categoryListRequestDto = new() { ResultsPerPage = 7 };
        ProductCategoryGetListResponseDto categoryListResponseDto = await _mediator.Send(categoryListRequestDto, token);
        model.CategoryList.MapFromProductCategoryListResponseDto(categoryListResponseDto);
        
        // ProductList.
        ProductGetListRequestDto requestDto = model.ToRequestDto();
        ProductGetListResponseDto responseDto = await _mediator.Send(requestDto, token);
        model.MapFromResponseDto(responseDto);

        return View("~/Views/Product/ProductList/ProductListPage.cshtml", model);
    }
    #endregion
}