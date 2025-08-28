namespace NATSInternal.Controllers;

[Route("Api/DebtIncurrence")]
[ApiController]
[Authorize]
public class DebtIncurrenceController : ControllerBase
{
    private readonly IDebtService _service;
    private readonly IValidator<DebtListRequestDto> _listValidator;
    private readonly IValidator<DebtUpsertRequestDto> _upsertValidator;
    private readonly INotifier _notifier;

    public DebtIncurrenceController(
            IDebtService service,
            IValidator<DebtListRequestDto> listValidator,
            IValidator<DebtUpsertRequestDto> upsertValidator,
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
            [FromQuery] DebtListRequestDto requestDto)
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
    public async Task<IActionResult> DebtIncurrenceDetail(int id)
    {
        try
        {
            return Ok(await _service.GetDetailAsync(id));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateDebtIncurrence")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DebtIncurrenceCreate(
            [FromBody] DebtUpsertRequestDto requestDto)
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
            // Create the debt incurrence.
            int createdId = await _service.CreateAsync(requestDto);
            string createdUrl = Url.Action(
                "DebtIncurrenceDetail",
                "DebtIncurrence",
                new { id = createdId });

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.DebtIncurrenceCreation, createdId);

            return Created(createdUrl, createdId);
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(exception);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "CanEditDebtIncurrence")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DebtIncurrenceUpdate(
            int id,
            [FromBody] DebtUpsertRequestDto requestDto)
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
            // Update the debt incurrence.
            await _service.UpdateAsync(id, requestDto);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.DebtIncurrenceModification, id);

            return Ok();
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (NotFoundException)
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
    [Authorize(Policy = "CanDeleteDebtIncurrence")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DebtIncurrenceDelete(int id)
    {
        try
        {
            // Delete the debt incurrence.
            await _service.DeleteAsync(id);

            // Create and distribute the notification to the users.
            await _notifier.Notify( NotificationType.DebtIncurrenceDeletion, id);

            return Ok();
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (NotFoundException)
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
            return UnprocessableEntity(ModelState);
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