using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Web.Models;

namespace NATSInternal.Web.Controllers;

[Route("khach-hang")]
[Authorize]
public class CustomerController : Controller
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion
    
    #region Constructors
    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion
    
    #region Methods
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] CustomerListModel model, CancellationToken cancellationToken)
    {
        CustomerGetListRequestDto requestDto = model.ToRequestDto();
        CustomerGetListResponseDto responseDto = await _mediator.Send(requestDto, cancellationToken);
        model.MapFromResponseDto(responseDto);
        return View(model);
    }
    #endregion
}