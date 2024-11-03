namespace NATSInternal.Controllers;

[Route("Api/Product")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    private readonly IValidator<ProductListRequestDto> _listValidator;
    private readonly IValidator<ProductUpsertRequestDto> _upsertValidator;
    private readonly INotifier _notifier;

    public ProductController(
            IProductService productService,
            IValidator<ProductListRequestDto> listValidator,
            IValidator<ProductUpsertRequestDto> upsertValidator,
            INotifier notifier)
    {
        _service = productService;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
        _notifier = notifier;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProductList(
            [FromQuery] ProductListRequestDto requestDto)
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

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProductDetail(
            int id,
            [FromQuery] ProductDetailRequestDto requestDto)
    {
        try
        {
            ProductDetailResponseDto responseDto;
            responseDto = await _service.GetDetailAsync(id, requestDto);
            return Ok(responseDto);
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateProduct")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ProductCreate(
            [FromBody] ProductUpsertRequestDto requestDto)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _upsertValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Performing creating operation.
        try
        {
            // Create the product.
            int createdId = await _service.CreateAsync(requestDto);
            string createdResourceUrl = Url.Action(
                "ProductDetail",
                "Product",
                new { id = createdId });

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.ProductCreation, createdId);

            return Created(createdResourceUrl, createdId);
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(ModelState);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "CanEditProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ProductUpdate(
            int id,
            [FromBody] ProductUpsertRequestDto requestDto)
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

        // Perform updating operation.
        try
        {
            // Update the product.
            await _service.UpdateAsync(id, requestDto);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.ProductModification, id);

            return Ok();
        }
        catch (ResourceNotFoundException exception)
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
    [Authorize(Policy = "CanDeleteProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProductDelete(int id)
    {
        try
        {
            // Delete the product.
            await _service.DeleteAsync(id);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.ProductDeletion, id);

            return NoContent();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
    }

    [HttpGet("GetCreatingPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCreatingPermission()
    {
        return Ok(_service.GetCreatingPermission());
    }
}