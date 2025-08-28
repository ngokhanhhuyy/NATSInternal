namespace NATSInternal.Controllers;

[Route("Api/Brand")]
[ApiController]
[Authorize]
public class BrandController : ControllerBase
{
    #region Fields
    private readonly IBrandService _service;
    private readonly INotifier _notifier;
    #endregion

    #region Constructors
    public BrandController(IBrandService service, INotifier notifier)
    {
        _service = service;
        _notifier = notifier;
    }
    #endregion

    #region Methods
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
            [FromQuery] BrandListRequestDto requestDto,
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
    [Authorize(Policy = "CanCreateBrand")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
            [FromBody] BrandUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        Guid id = await _service.CreateAsync(requestDto, cancellationToken);
        await _notifier.Notify(NotificationType.BrandCreation, id);

        return CreatedAtAction(nameof(Detail), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "CanEditBrand")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
            Guid id,
            [FromBody] BrandUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await _service.UpdateAsync(id, requestDto, cancellationToken);
        await _notifier.Notify(NotificationType.BrandModification, id);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "CanDeleteBrand")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        await _notifier.Notify(NotificationType.BrandDeletion, id);

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
