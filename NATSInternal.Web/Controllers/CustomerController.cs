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

    // [HttpGet("tao-moi")]
    // public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    // {
    //     CustomerUpsertModel model = new();
    //     model.CustomerList.ResultsPerPage = 8;
    //     CustomerGetListRequestDto listRequestDto = model.CustomerList.ToRequestDto();
    //     CustomerGetListResponseDto listResponseDto = await _mediator.Send(listRequestDto, cancellationToken);
    //     model.CustomerList.MapFromResponseDto(listResponseDto);
    //     
    //     return View("~/Views/Customer/CustomerUpsert/CustomerCreatePage.cshtml", model);
    // }
    //
    // [HttpPost("tao-moi")]
    // public async Task<IActionResult> Create(
    //     [FromForm] CustomerUpsertModel model,
    //     CancellationToken cancellationToken = default)
    // {
    //     try
    //     {
    //         CustomerCreateRequestDto requestDto = model.ToCreateRequestDto();
    //         Guid createdId = await _mediator.Send(requestDto, cancellationToken);
    //         return RedirectToAction("Detail", "Customer", new { Id = createdId });
    //     }
    //     catch (Exception exception)
    //     {
    //         switch (exception)
    //         {
    //             case ValidationException validationException:
    //                 ModelState.AddModelErrors(validationException);
    //                 break;
    //             case OperationException operationException:
    //                 ModelState.AddModelErrors(operationException);
    //                 break;
    //             default:
    //                 throw;
    //         }
    //
    //         return View("~/Views/Customer/CustomerUpsert/CustomerCreatePage.cshtml", model);
    //     }
    // }

    [HttpGet("{id:guid}/chinh-sua")]
    public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken)
    {
        CustomerGetDetailRequestDto requestDto = new() { Id = id };
        CustomerGetDetailResponseDto responseDto = await _mediator.Send(requestDto, cancellationToken);
        CustomerUpsertModel upsertModel = new(responseDto)
        {
            Id = id
        };
        
        CustomerListModel customerListModel = new();
        customerListModel.ResultsPerPage = 8;
        ViewBag.CustomerListModel = customerListModel;
        if (upsertModel.PickedIntroducer is null)
        {
            CustomerGetListRequestDto listRequestDto = customerListModel.ToRequestDto();
            CustomerGetListResponseDto listResponseDto = await _mediator.Send(listRequestDto, cancellationToken);
            customerListModel.MapFromResponseDto(listResponseDto);
        }
        
        return View("~/Views/Customer/CustomerUpsert/CustomerUpdatePage.cshtml", upsertModel);
    }

    [HttpPost("{id:guid}/chinh-sua")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] CustomerUpsertModel upsertModel,
        [FromQuery] CustomerListModel customerListModel,
        CancellationToken cancellationToken)
    {
        switch (upsertModel.Action)
        {
            case CustomerUpsertModel.SubmitAction.Reload:
                await LoadUpsertModelIntroducerOrCustomerListAsync(
                    upsertModel,
                    customerListModel,
                    cancellationToken);
                return View("~/Views/Customer/CustomerUpsert/CustomerUpdatePage.cshtml", upsertModel);
            case CustomerUpsertModel.SubmitAction.Submit:
                try
                {
                    CustomerUpdateRequestDto updateRequestDto = upsertModel.ToUpdateRequestDto();
                    await _mediator.Send(updateRequestDto, cancellationToken);
                    return RedirectToAction("Detail", "Customer", new { id });
                }
                catch (Exception exception)
                {
                    switch (exception)
                    {
                        case ValidationException validationException:
                            ModelState.AddModelErrors(validationException);
                            return View("~/Views/Customer/CustomerUpsert/CustomerUpdatePage.cshtml", upsertModel);
                        case OperationException operationException:
                            ModelState.AddModelErrors(operationException);
                            return View("~/Views/Customer/CustomerUpsert/CustomerUpdatePage.cshtml", upsertModel);
                        default:
                            throw;
                    }
                }
            default:
                ErrorModel errorModel = new()
                {
                    ReturningPageUrl = Url.Action("Update", new { id }),
                    ReturningPageName = "Chỉnh sửa khách hàng"
                };

                return RedirectToAction("Index", "Error", new { model = errorModel });
        }
    }

    // [HttpPost("{id:guid}/chinh-sua")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Update(Guid id, CustomerUpsertModel model, CancellationToken cancellationToken)
    // {
    //     model.Id = id;
    //     if (model.Action == CustomerUpsertModel.SubmitAction.RedirectionToIntroducerPicker)
    //     {
    //         try
    //         {
    //             await ValidateAsync(model, cancellationToken);
    //             return CustomerUpdateIntroducerView(model);
    //         }
    //         catch (ValidationException exception)
    //         {
    //             ModelState.AddModelErrors(exception);
    //             return CustomerUpdateIntroducerView(model);
    //         }
    //     }
    //     else if (model.Action == CustomerUpsertModel.SubmitAction.IntroducerPickingOrUnpicking)
    //     {
    //         await ValidateAsync(model, cancellationToken);
    //
    //         if (model.PickedIntroducerId.HasValue)
    //         {
    //             CustomerGetDetailRequestDto introducerRequestDto = new() { Id = model.PickedIntroducerId.Value };
    //             CustomerGetDetailResponseDto introducerResponseDto = await _mediator.Send(
    //                 introducerRequestDto,
    //                 cancellationToken);
    //             
    //             model.MapFromPickedIntroducerResponseDto(introducerResponseDto);
    //         }
    //         else
    //         {
    //             CustomerGetListRequestDto listRequestDto = model.CustomerList.ToRequestDto();
    //             CustomerGetListResponseDto listResponseDto = await _mediator.Send(listRequestDto, cancellationToken);
    //             model.CustomerList.MapFromResponseDto(listResponseDto);
    //         }
    //     }
    //     else if (model.Action == CustomerUpsertModel.SubmitAction.FinalSubmit)
    //     {
    //         try
    //         {
    //             await _mediator.Send(model.ToUpdateRequestDto(), cancellationToken);
    //             return RedirectToAction("Detail","Customer", new { Id = id });
    //         }
    //         catch (Exception exception)
    //         {
    //             if (model.PickedIntroducerId.HasValue)
    //             {
    //                 CustomerGetDetailResponseDto introducerResponseDto = await GetIntroducerDetailAsync(
    //                     model.PickedIntroducerId.Value,
    //                     cancellationToken);
    //
    //                 model.MapFromPickedIntroducerResponseDto(introducerResponseDto);
    //             }
    //             switch (exception)
    //             {
    //                 case ValidationException validationException:
    //                     ModelState.AddModelErrors(validationException);
    //                     break;
    //                 case OperationException operationException:
    //                     ModelState.AddModelErrors(operationException);
    //                     break;
    //                 default:
    //                     throw;
    //             }
    //
    //             return View(
    //                 "~/Views/Customer/CustomerUpsert/CustomerUpdatePage_Step2_Introducer.cshtml",
    //                 model);
    //         }
    //     }
    //     else
    //     {
    //         throw new Exception($"{model.Action} is not a valid value for {nameof(model.Action)}");
    //     }
    // }
    #endregion

    #region PrivateMethods
    private async Task LoadUpsertModelIntroducerOrCustomerListAsync(
        CustomerUpsertModel upsertModel,
        CustomerListModel customerListModel,
        CancellationToken cancellationToken)
    {
        if (upsertModel.PickedIntroducerId.HasValue)
        {
            CustomerGetDetailRequestDto introducerRequestDto = new() { Id = upsertModel.PickedIntroducerId.Value };
            CustomerGetDetailResponseDto introducerResponseDto = await _mediator.Send(
                introducerRequestDto,
                cancellationToken);
                    
            upsertModel.MapFromPickedIntroducerResponseDto(introducerResponseDto);
        }
        else
        {
            customerListModel.ResultsPerPage = 8;
            CustomerGetListRequestDto listRequestDto = customerListModel.ToRequestDto();
            CustomerGetListResponseDto listResponseDto = await _mediator.Send(
                listRequestDto,
                cancellationToken);
            customerListModel.MapFromResponseDto(listResponseDto);
        }
    }
    #endregion
}