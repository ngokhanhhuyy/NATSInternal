namespace NATSInternal.Controllers;

[Route("Api/ProductCategory")]
[ApiController]
[Authorize]
public class ProductCategoryController : ControllerBase
{
    #region Fields
    private readonly IProductCategoryService _service;
    private readonly INotifier _notifier;
    #endregion

    #region Constructors
    public ProductCategoryController(IProductCategoryService service, INotifier notifier)
    {
        _service = service;
        _notifier = notifier;
    }
    #endregion

    #region Methods
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(
            [FromQuery] ProductCategoryListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        return Ok(await _service.GetListAsync(requestDto, cancellationToken));
    }

    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> All(CancellationToken cancellationToken = default)
    {
        return Ok(await _service.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Detail(Guid id, CancellationToken cancellationToken = default)
    {
        return Ok(await _service.GetDetailAsync(id, cancellationToken));
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateProductCategory")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
            [FromBody] ProductCategoryUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        Guid id = await _service.CreateAsync(requestDto, cancellationToken);
        await _notifier.Notify(NotificationType.ProductCategoryCreation, id);

        return CreatedAtAction(nameof(Detail), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "CanEditProductCategory")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
            Guid id,
            [FromBody] ProductCategoryUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await _service.UpdateAsync(id, requestDto, cancellationToken);
        await _notifier.Notify(NotificationType.ProductCategoryModification, id);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "CanDeleteProductCategory")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _service.DeleteAsync(id, cancellationToken);
        await _notifier.Notify(NotificationType.ProductCategoryDeletion, id);

        return Ok();
    }

    [HttpGet("CreatingPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCreatingPermission()
    {
        return Ok(_service.GetCreatingPermission());
    }
    #endregion
}
