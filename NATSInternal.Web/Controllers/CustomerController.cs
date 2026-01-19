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

    [HttpGet("{id:guid}/chinh-sua")]
    public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken)
    {
        CustomerGetDetailRequestDto requestDto = new() { Id = id };
        CustomerGetDetailResponseDto responseDto = await _mediator.Send(requestDto, cancellationToken);
        CustomerUpsertModel model = new(responseDto)
        {
            Id = id
        };
        
        model.CustomerList.ResultsPerPage = 8;
        await LoadUpsertModelIntroducerOrCustomerListAsync(model, cancellationToken);
        
        return View("~/Views/Customer/CustomerUpsert/CustomerUpdatePage.cshtml", model);
    }

    [HttpPost("{id:guid}/chinh-sua")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Guid id, [FromForm] CustomerUpsertModel model, CancellationToken cancellationToken)
    {
        model.Id = id;
        try
        {
            CustomerUpdateRequestDto updateRequestDto = model.ToUpdateRequestDto();
            await _mediator.Send(updateRequestDto, cancellationToken);
            return RedirectToAction("Detail", "Customer", new { id, fragment = "" });
        }
        catch (Exception exception)
        {
            switch (exception)
            {
                case ValidationException validationException:
                    await LoadUpsertModelIntroducerOrCustomerListAsync(model, cancellationToken);
                    ModelState.AddModelErrors(validationException);
                    return View("~/Views/Customer/CustomerUpsert/CustomerUpdatePage.cshtml", model);
                case OperationException operationException:
                    await LoadUpsertModelIntroducerOrCustomerListAsync(model, cancellationToken);
                    ModelState.AddModelErrors(operationException);
                    return View("~/Views/Customer/CustomerUpsert/CustomerUpdatePage.cshtml", model);
                default:
                    throw;
            }
        }
    }

    [HttpGet("upsert-customer-list-partial")]
    public async Task<PartialViewResult> UpsertCustomerListPartial(
        [FromQuery] Guid? excludedId,
        [FromQuery] string? searchContent,
        [FromQuery] int? page,
        CancellationToken token)
    {
        CustomerListModel model = new()
        {
            SearchContent = searchContent,
            Page = page,
            ResultsPerPage = 8,
        };
        
        CustomerGetListRequestDto requestDto = model.ToRequestDto();
        if (excludedId.HasValue)
        {
            requestDto.ExcludedIds.Add(excludedId.Value);
        }
        
        CustomerGetListResponseDto responseDto = await _mediator.Send(requestDto, token);
        model.MapFromResponseDto(responseDto);

        return PartialView("~/Views/Customer/CustomerUpsert/_IntroducerPanelCustomerList.cshtml", model);
    }

    [HttpGet("upsert-picked-introducer-partial")]
    public async Task<PartialViewResult> UpsertPickedIntroducerPartial(
        [FromQuery] Guid pickedIntroducerId,
        CancellationToken token)
    {
        CustomerGetDetailRequestDto requestDto = new() { Id = pickedIntroducerId };
        CustomerGetDetailResponseDto responseDto = await _mediator.Send(requestDto, token);
        CustomerBasicModel model = new(responseDto);

        return PartialView("~/Views/Customer/CustomerUpsert/_IntroducerPanelPickedIntroducer.cshtml", model);
    }
    #endregion

    #region PrivateMethods
    private async Task LoadUpsertModelIntroducerOrCustomerListAsync(CustomerUpsertModel model, CancellationToken token)
    {
        if (model.PickedIntroducerId.HasValue)
        {
            CustomerGetDetailRequestDto introducerRequestDto = new() { Id = model.PickedIntroducerId.Value };
            CustomerGetDetailResponseDto introducerResponseDto = await _mediator.Send(introducerRequestDto, token);
                    
            model.MapFromPickedIntroducerResponseDto(introducerResponseDto);
        }
        else
        {
            model.CustomerList.ResultsPerPage = 8;
            CustomerGetListRequestDto listRequestDto = model.CustomerList.ToRequestDto();
            listRequestDto.ExcludedIds.Add(model.Id);
            CustomerGetListResponseDto listResponseDto = await _mediator.Send(listRequestDto, token);
            model.CustomerList.MapFromResponseDto(listResponseDto);
        }
    }
    #endregion
}