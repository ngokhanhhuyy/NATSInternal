using FluentValidation;
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
        CustomerGetListResponseDto responseDto;
        try
        {
            responseDto = await _mediator.Send(requestDto, cancellationToken);
        }
        catch (ValidationException)
        {
            return RedirectToAction("List");
        }
        
        model.MapFromResponseDto(responseDto);
        return View(model);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Detail([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        CustomerGetDetailRequestDto requestDto = new() { Id = id };
        CustomerGetDetailResponseDto responseDto = await _mediator.Send(requestDto, cancellationToken);
        CustomerDetailModel model = new(responseDto);
        return View(model);
    }

    [HttpGet("tao-moi")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet("{id:guid}/chinh-sua")]
    public IActionResult Update([FromRoute] Guid id)
    {
        return View();
    }
    #endregion
}