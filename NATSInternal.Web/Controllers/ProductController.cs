using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Application.UseCases.Shared;
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
    public async Task<IActionResult> Create(CancellationToken token)
    {
        IEnumerable<ProductCategoryBasicResponseDto> categoryOptions;
        IEnumerable<BrandBasicResponseDto> brandOptions;
        (categoryOptions, brandOptions) = await LoadCategoryAndBrandOptionsAsync(token);

        ProductUpsertModel model = new(categoryOptions, brandOptions);
        return CreateView(model);
    }

    [HttpPost("tao-moi")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] ProductUpsertModel model, CancellationToken token)
    {
        try
        {
            ProductCreateRequestDto requestDto = model.ToCreateRequestDto();
            Guid id = await _mediator.Send(requestDto, token);

            return this.SuccessfulSubmissionConfirmationView(Url.Action("Detail", new { id }));
        }
        catch (Exception exception) when (exception is ValidationException or OperationException)
        {
            IEnumerable<ProductCategoryBasicResponseDto> categoryOptions;
            IEnumerable<BrandBasicResponseDto> brandOptions;
            (categoryOptions, brandOptions) = await LoadCategoryAndBrandOptionsAsync(token);
            model.MapFromCategoryOptionResponseDtos(categoryOptions);
            model.MapFromBrandOptionResponseDtos(brandOptions);

            if (exception is ValidationException validationException)
            {
                ModelState.AddModelErrors(validationException);
            }
            else if (exception is OperationException operationException)
            {
                ModelState.AddModelErrors(operationException);
            }

            return UpdateView(model);
        }
    }

    [HttpGet("{id:guid}/chinh-sua")]
    public async Task<IActionResult> Update([FromRoute] Guid id, CancellationToken token)
    {
        IEnumerable<ProductCategoryBasicResponseDto> categoryOptions;
        IEnumerable<BrandBasicResponseDto> brandOptions;
        (categoryOptions, brandOptions) = await LoadCategoryAndBrandOptionsAsync(token);

        ProductGetDetailRequestDto productRequestDto = new() { Id = id };
        ProductGetDetailResponseDto productResponseDto = await _mediator.Send(productRequestDto, token);

        ProductUpsertModel model = new(productResponseDto, categoryOptions, brandOptions);
        return UpdateView(model);
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

    #region PrivateMethods
    private ViewResult CreateView(ProductUpsertModel model)
    {
        return View("~/Views/Product/ProductUpsert/ProductCreatePage.cshtml", model);
    }

    private ViewResult UpdateView(ProductUpsertModel model)
    {
        return View("~/Views/Product/ProductUpsert/ProductUpdatePage.cshtml", model);
    }

    private async Task<(IEnumerable<ProductCategoryBasicResponseDto>, IEnumerable<BrandBasicResponseDto>)>
        LoadCategoryAndBrandOptionsAsync(CancellationToken token)
    {
        ProductCategoryGetAllRequestDto categoryOptionsRequestDto = new();
        IEnumerable<ProductCategoryBasicResponseDto> categoryOptions = await _mediator.Send(
            categoryOptionsRequestDto,
            token);

        BrandGetAllRequestDto brandOptionsRequestDto = new();
        IEnumerable<BrandBasicResponseDto> brandOptions = await _mediator.Send(brandOptionsRequestDto, token);

        return (categoryOptions, brandOptions);
    }
    #endregion
}