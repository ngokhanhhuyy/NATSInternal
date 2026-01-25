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
        model.BrandList.MapFromResponseDto(brandListResponseDto);
        
        // ProductCategoryList.
        ProductCategoryGetListRequestDto categoryListRequestDto = new() { ResultsPerPage = 7 };
        ProductCategoryGetListResponseDto categoryListResponseDto = await _mediator.Send(categoryListRequestDto, token);
        model.CategoryList.MapFromResponseDto(categoryListResponseDto);
        
        // ProductList.
        ProductGetListRequestDto requestDto = model.ToRequestDto();
        ProductGetListResponseDto responseDto = await _mediator.Send(requestDto, token);
        model.MapFromResponseDto(responseDto);

        return View("~/Views/Product/ProductList/ProductListPage.cshtml", model);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Detail([FromRoute] Guid id, CancellationToken token)
    {
        ProductGetDetailRequestDto requestDto = new() { Id = id };
        ProductGetDetailResponseDto responseDto = await _mediator.Send(requestDto, token);
        ProductDetailModel model = new(responseDto);

        return View("~/Views/Product/ProductDetail/ProductDetailPage.cshtml", model);
    }

    [HttpGet("tao-moi")]
    public IActionResult Create()
    {
        return Ok();
    }

    [HttpGet("{id:guid}/chinh-sua")]
    public IActionResult Update([FromRoute] Guid id)
    {
        return Ok();
    }

    [HttpGet("{id:guid}/xoa-bo")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        DeleteConfirmationModel model = new()
        {
            CancelUrl = Url.Action("Detail", new { id })
        };

        return this.DeleteConfirmationView(model);
    }

    [HttpPost("{id:guid}/xoa-bo")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        ProductDeleteRequestDto requestDto = new() { Id = id };
        await _mediator.Send(requestDto, token);

        return RedirectToAction("List");
    }
    #endregion
}