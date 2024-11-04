namespace NATSInternal.Controllers;

[Route("Api/Expense")]
[ApiController]
[Authorize]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _service;
    private readonly IValidator<ExpenseListRequestDto> _listValidator;
    private readonly IValidator<ExpenseUpsertRequestDto> _upsertValidator;
    private readonly INotifier _notifier;

    public ExpenseController(
            IExpenseService service,
            IValidator<ExpenseListRequestDto> listValidator,
            IValidator<ExpenseUpsertRequestDto> upsertValidator,
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
    public async Task<IActionResult> ExpenseList(
            [FromQuery] ExpenseListRequestDto requestDto)
    {
        // Validate data from request.
        requestDto.TransformValues();
        ValidationResult validationResult = _listValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Fetch the list of data.
        return Ok(await _service.GetListAsync(requestDto));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExpenseDetail(int id)
    {
        try
        {
            return Ok(await _service.GetDetailAsync(id));
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateExpense")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ExpenseCreate(
            [FromBody] ExpenseUpsertRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult = _upsertValidator.Validate(
            requestDto,
            options => options
                .IncludeRuleSets("Create").IncludeRulesNotInRuleSet());
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform creating operation.
        try
        {
            // Create the expense.
            int createdId = await _service.CreateAsync(requestDto);
            string createdUrl = Url.Action(
                "ExpenseDetail",
                "Expense",
                new { id = createdId });

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.ExpenseCreation, createdId);

            return Created(createdUrl, createdId);
        }
        catch (ConcurrencyException)
        {
            return Conflict();
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "CanEditExpense")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ExpenseUpdate(
            int id,
            [FromBody] ExpenseUpsertRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult = _upsertValidator.Validate(
            requestDto,
            options => options
                .IncludeRuleSets("Update").IncludeRulesNotInRuleSet());
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform updating operation.
        try
        {
            // Update the expense.
            await _service.UpdateAsync(id, requestDto);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.ExpenseModification, id);

            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (ConcurrencyException)
        {
            return Conflict();
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "CanDeleteExpense")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ExpenseDelete(int id)
    {
        try
        {
            // Delete the expense.
            await _service.DeleteAsync(id);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.ExpenseDeletion, id);

            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (ConcurrencyException)
        {
            return Conflict();
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