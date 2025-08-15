namespace NATSInternal.Controllers;

[Route("Api/ProductCategory")]
[ApiController]
[Authorize]
public class ProductCategoryController : ControllerBase
{
    private readonly IProductCategoryService _service;
    private readonly IValidator<ProductCategoryListRequestDto> _listValidator;
    private readonly IValidator<ProductCategoryRequestDto> _upsertValidator;
    private readonly INotifier _notifier;

    public ProductCategoryController(
            IProductCategoryService service,
            IValidator<ProductCategoryListRequestDto> listValidator,
            IValidator<ProductCategoryRequestDto> upsertValidator,
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
    public async Task<IActionResult> ProductCategoryList(
            [FromQuery] ProductCategoryListRequestDto requestDto)
    {
        // Validate data from the request.

        requestDto.TransformValues();
        ValidationResult validationResult = _listValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok(await _service.GetListAsync(requestDto));
    }

    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ProductCategoryAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProductCategoryDetail(int id)
    {
        try
        {
            return Ok(await _service.GetDetailAsync(id));
        }
        catch (NotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateProductCategory")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ProductCategoryCreate(
            [FromBody] ProductCategoryRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _upsertValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform the creating operation.
        try
        {
            // Create the product category.
            int createdId = await _service.CreateAsyns(requestDto);
            string createdResourceUrl = Url.Action(
                "ProductCategoryDetail",
                "ProductCategory",
                new { id = createdId });

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.ProductCategoryCreation, createdId);

            return Created(createdResourceUrl, createdId);
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(exception);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "CanEditProductCategory")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ProductCategoryUpdate(
            int id,
            [FromBody] ProductCategoryRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _upsertValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform the updating operation.
        try
        {
            // Update the product category.
            await _service.UpdateAsync(id, requestDto);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.ProductCategoryModification, id);

            return Ok();
        }
        catch (NotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(ModelState);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "CanDeleteProductCategory")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProductCategoryDelete(int id)
    {
        try
        {
            // Delete the product category.
            await _service.DeleteAsync(id);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.ProductCategoryDeletion, id);

            return Ok();
        }
        catch (NotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
    }

    [HttpGet("CreatingPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCreatingPermission()
    {
        return Ok(_service.GetCreatingPermission());
    }
}
