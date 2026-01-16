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
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        CustomerUpsertModel model = new();
        model.CustomerList.ResultsPerPage = 8;
        CustomerGetListRequestDto listRequestDto = model.CustomerList.ToRequestDto();
        CustomerGetListResponseDto listResponseDto = await _mediator.Send(listRequestDto, cancellationToken);
        model.CustomerList.MapFromResponseDto(listResponseDto);
        
        return View("~/Views/Customer/CustomerUpsert/CustomerCreatePage.cshtml", model);
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
    public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken)
    {
        CustomerGetDetailRequestDto requestDto = new() { Id = id };
        CustomerGetDetailResponseDto responseDto = await _mediator.Send(requestDto, cancellationToken);
        CustomerUpsertModel model = new(responseDto)
        {
            Id = id
        };
        
        model.CustomerList.ResultsPerPage = 8;
        CustomerGetListRequestDto listRequestDto = model.CustomerList.ToRequestDto();
        CustomerGetListResponseDto listResponseDto = await _mediator.Send(listRequestDto, cancellationToken);
        model.CustomerList.MapFromResponseDto(listResponseDto);
        
        return View("~/Views/Customer/CustomerUpsert/CustomerUpdatePage_Step1_PersonalInformation.cshtml", model);
    }

    [HttpPost("{id:guid}/chinh-sua")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Guid id, CustomerUpsertModel model, CancellationToken cancellationToken)
    {
        model.Id = id;
        if (model.Action == CustomerUpsertModel.SubmitAction.RedirectionToIntroducerPicker)
        {
            try
            {
                await ValidateAsync(model, cancellationToken);
                return CustomerUpdateIntroducerView(model);
            }
            catch (ValidationException exception)
            {
                ModelState.AddModelErrors(exception);
                return CustomerUpdateIntroducerView(model);
            }
        }
        else if (model.Action == CustomerUpsertModel.SubmitAction.IntroducerPickingOrUnpicking)
        {
            await ValidateAsync(model, cancellationToken);

            if (model.PickedIntroducerId.HasValue)
            {
                CustomerGetDetailRequestDto introducerRequestDto = new() { Id = model.PickedIntroducerId.Value };
                CustomerGetDetailResponseDto introducerResponseDto = await _mediator.Send(
                    introducerRequestDto,
                    cancellationToken);
                
                model.MapFromPickedIntroducerResponseDto(introducerResponseDto);
            }
            else
            {
                CustomerGetListRequestDto listRequestDto = model.CustomerList.ToRequestDto();
                CustomerGetListResponseDto listResponseDto = await _mediator.Send(listRequestDto, cancellationToken);
                model.CustomerList.MapFromResponseDto(listResponseDto);
            }
        }
        else if (model.Action == CustomerUpsertModel.SubmitAction.FinalSubmit)
        {
            try
            {
                await _mediator.Send(model.ToUpdateRequestDto(), cancellationToken);
                return RedirectToAction("Detail","Customer", new { Id = id });
            }
            catch (Exception exception)
            {
                if (model.PickedIntroducerId.HasValue)
                {
                    CustomerGetDetailResponseDto introducerResponseDto = await GetIntroducerDetailAsync(
                        model.PickedIntroducerId.Value,
                        cancellationToken);

                    model.MapFromPickedIntroducerResponseDto(introducerResponseDto);
                }
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

                return View(
                    "~/Views/Customer/CustomerUpsert/CustomerUpdatePage_Step2_Introducer.cshtml",
                    model);
            }
        }
        else
        {
            throw new Exception($"{model.Action} is not a valid value for {nameof(model.Action)}");
        }
    }
    #endregion

    #region PrivateMethods
    private async Task<CustomerGetDetailResponseDto> GetIntroducerDetailAsync(
        Guid introducerId,
        CancellationToken cancellationToken = default)
    {
        CustomerGetDetailRequestDto requestDto = new() { Id = introducerId };
        CustomerGetDetailResponseDto responseDto = await _mediator.Send(requestDto, cancellationToken);
        return responseDto;
    }

    private async Task ValidateAsync(CustomerUpsertModel model, CancellationToken cancellationToken = default)
    {
        CustomerValidateRequestDto requestDto = model.ToValidateRequestDto();
        await _mediator.Send(requestDto, cancellationToken);
    }

    private ViewResult CustomerUpdatePersonalInformationView(CustomerUpsertModel model)
    {
        return View("~/Views/Customer/CustomerUpsert/CustomerUpdatePersonalInformationPage.cshtml", model);
    }

    private ViewResult CustomerUpdateIntroducerView(CustomerUpsertModel model)
    {
        return View("~/Views/Customer/CustomerUpsert/CustomerUpdateIntroducerPage.cshtml", model);
    }

    private ViewResult CustomerUpdateConfirmationView(CustomerUpsertModel model)
    {
        return View("~/Views/Customer/CustomerUpsert/CustomerUpdateConfirmationPage.cshtml", model);
    }
    #endregion
}