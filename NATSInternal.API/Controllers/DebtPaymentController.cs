namespace NATSInternal.Controllers;

[Route("Api/DebtPayment")]
[ApiController]
[Authorize]
public class DebtPaymentController : ControllerBase
{
    private readonly IDebtPaymentService _service;
    private readonly IValidator<DebtPaymentListRequestDto> _listValidator;
    private readonly IValidator<DebtPaymentUpsertRequestDto> _upsertValidator;
    private readonly INotifier _notifier;

    public DebtPaymentController(
            IDebtPaymentService service,
            IValidator<DebtPaymentListRequestDto> listValidator,
            IValidator<DebtPaymentUpsertRequestDto> upsertValidator,
            INotifier notifier)
    {
        _service = service;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
        _notifier = notifier;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetListAsync(
            [FromQuery] DebtPaymentListRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _listValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Fetch the list.
        return Ok(await _service.GetListAsync(requestDto));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DebtPaymentDetail(int id)
    {
        try
        {
            return Ok(await _service.GetDetailAsync(id));
        }
        catch (ResourceNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateDebtPayment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DebtPaymentCreate(
            [FromBody] DebtPaymentUpsertRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _upsertValidator.Validate(requestDto, options =>
            options.IncludeRuleSets("Create").IncludeRulesNotInRuleSet());
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform the creating operation.
        try
        {
            // Create the debt payment.
            int createdId = await _service.CreateAsync(requestDto);
            string createdResourceUrl = Url.Action(
                "DebtPaymentDetail",
                "DebtPayment",
                new { id = createdId });

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.DebtPaymentCreation, createdId);

            return Created(createdResourceUrl, createdId);
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (ResourceNotFoundException)
        {
            return NotFound();
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(ModelState);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "CanEditDebtPayment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DebtPaymentUpdate(
            int id,
            [FromBody] DebtPaymentUpsertRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _upsertValidator.Validate(requestDto, options =>
            options.IncludeRuleSets("Update").IncludeRulesNotInRuleSet());
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform the updating operation.
        try
        {
            // Update the debt payment.
            await _service.UpdateAsync(id, requestDto);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.DebtPaymentModification, id);

            return Ok();
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (ResourceNotFoundException)
        {
            return NotFound();
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(ModelState);
        }
        catch (ConcurrencyException)
        {
            return Conflict();
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "CanDeleteDebtPayment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DebtPaymentDelete(int id)
    {
        try
        {
            // Delete the debt payment.
            await _service.DeleteAsync(id);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.DebtPaymentDeletion, id);

            return Ok();
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (ResourceNotFoundException)
        {
            return NotFound();
        }
        catch (ConcurrencyException)
        {
            return Conflict();
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(exception);
        }
    }
    
    [HttpGet("ListSortingOptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ListSortingOptions()
    {
        return Ok(_service.GetListSortingOptions());
    }

    [HttpGet("ListMonthYearOptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListMonthYearOptions()
    {
        return Ok(await _service.GetListMonthYearOptionsAsync());
    }

    [HttpGet("CreatingPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCreatingPermission()
    {
        return Ok(_service.GetCreatingPermission());
    }

    [HttpGet("CreatingAuthorization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCreatingAuthorization()
    {
        return Ok(_service.GetCreatingAuthorization());
    }
}