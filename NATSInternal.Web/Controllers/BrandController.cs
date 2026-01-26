using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Web.Models;

namespace NATSInternal.Web.Controllers;

[Route("/san-pham/thuong-hieu")]
[Authorize]
public class BrandController : Controller
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public BrandController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Methods
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] BrandListModel model, CancellationToken token)
    {
        BrandGetListRequestDto requestDto = model.ToRequestDto();
        BrandGetListResponseDto responseDto = await _mediator.Send(requestDto, token);
        model.MapFromResponseDto(responseDto);

        return View("~/Views/Product/Brand/BrandList/BrandListPage.cshtml", model);
    }

    [HttpGet("{id:guid}")]
    public IActionResult Detail([FromRoute] Guid id)
    {
        return Ok();
    }
    

    [HttpGet("tao-moi")]
    public IActionResult Create()
    {
        return Ok();
    }
    #endregion
}