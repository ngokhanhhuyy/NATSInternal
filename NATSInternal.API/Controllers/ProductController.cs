namespace NATSInternal.Controllers;

[Route("Api/Product")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    #region Fields
    private readonly IProductService _service;
    private readonly INotifier _notifier;
    #endregion

    #region Constructors
    public ProductController(IProductService productService, INotifier notifier)
    {
        _service = productService;
        _notifier = notifier;
    }
    #endregion

    #region Methods
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(
            [FromQuery] ProductListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        return Ok(await _service.GetListAsync(requestDto, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Detail(Guid id, CancellationToken cancellationToken = default)
    {
        return Ok(await _service.GetDetailAsync(id, cancellationToken));
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateProduct")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
            [FromBody] ProductUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        Guid id = await _service.CreateAsync(requestDto, cancellationToken);
        await _notifier.Notify(NotificationType.ProductCreation, id);

        return CreatedAtAction(nameof(Detail), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "CanEditProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ProductUpdate(
            Guid id,
            [FromBody] ProductUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await _service.UpdateAsync(id, requestDto, cancellationToken);
        await _notifier.Notify(NotificationType.ProductModification, id);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "CanDeleteProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProductDelete(Guid id, CancellationToken cancellationToken = default)
    {
        await _service.DeleteAsync(id, cancellationToken);
        await _notifier.Notify(NotificationType.ProductDeletion, id);

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