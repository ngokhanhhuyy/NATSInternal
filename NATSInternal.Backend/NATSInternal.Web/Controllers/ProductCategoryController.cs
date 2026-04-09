using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Web.Models;

namespace NATSInternal.Web.Controllers;

[Route("/san-pham/phan-loai")]
[Authorize]
public class ProductCategoryController : Controller
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public ProductCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Methods
    [HttpGet]
    public IActionResult List([FromQuery] ProductCategoryListModel model)
    {
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public IActionResult Detail([FromRoute] Guid id)
    {
        return Ok();
    }
    #endregion
}