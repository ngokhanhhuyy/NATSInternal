using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Web.Extensions;
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
        return View("~/Views/Customer/CustomerList/CustomerListPage.cshtml", model);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Detail([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        CustomerGetDetailRequestDto requestDto = new() { Id = id };
        CustomerGetDetailResponseDto responseDto = await _mediator.Send(requestDto, cancellationToken);
        CustomerDetailModel model = new(responseDto);
        return View("~/Views/Customer/CustomerDetail/CustomerDetailPage.cshtml", model);
    }

    [HttpGet("tao-moi")]
    public IActionResult Create()
    {
        return View("~/Views/Customer/CustomerUpsert/CustomerCreatePage.cshtml");
    }

    [HttpPost("tao-moi")]
    public async Task<IActionResult> Create(
        [FromForm] CustomerUpsertModel model,
        CancellationToken cancellationToken = default)
    {
        try
        {
            CustomerCreateRequestDto requestDto = model.ToCreateRequestDto();
            Guid createdId = await _mediator.Send(requestDto, cancellationToken);
            return RedirectToAction("Detail", "Customer", new { Id = createdId });
        }
        catch (Exception exception)
        {
            switch (exception)
            {
                case ValidationException validationException:
                    ModelState.AddModelErrors(validationException);
                    break;
                case OperationException operationException:
                    ModelState.AddModelErrors(operationException);
                    break;
                default:
                    throw;
            }

            return View("~/Views/Customer/CustomerUpsert/CustomerCreatePage.cshtml", model);
        }
    }


    [HttpGet("{id:guid}/chinh-sua")]
    public async Task<IActionResult> Update([FromRoute] Guid id, CancellationToken cancellationToken = default)
    { 
        CustomerGetDetailRequestDto requestDto = new() { Id = id };
        CustomerGetDetailResponseDto responseDto = await _mediator.Send(requestDto, cancellationToken);
        CustomerUpsertModel model = new(responseDto);
        return View("~/Views/Customer/CustomerUpsert/CustomerCreatePage.cshtml", model);
    }

    [HttpPost("{id:guid}/chinh-sua")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromForm] CustomerUpsertModel model,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _mediator.Send(model.ToUpdateRequestDto(), cancellationToken);
            return RedirectToAction("Detail","Customer", new { Id = id });
        }
        catch (Exception exception)
        {
            CustomerGetDetailRequestDto requestDto = new() { Id = id };
            CustomerGetDetailResponseDto responseDto = await _mediator.Send(requestDto, cancellationToken);
            model.MapFromIntroducerResponseDto(responseDto.Introducer);
            switch (exception)
            {
                case ValidationException validationException:
                    ModelState.AddModelErrors(validationException);
                    break;
                case OperationException operationException:
                    ModelState.AddModelErrors(operationException);
                    break;
                default:
                    throw;
            }

            return View("~/Views/Customer/CustomerUpsert/CustomerCreatePage.cshtml", model);
        }
    }
    #endregion
}