namespace NATSInternal.Controllers.Api;

[Route("/Api/Customer")]
[ApiController]
[Authorize]
public class CustomerController : ControllerBase
{
    #region Fields
    private readonly ICustomerService _service;
    private readonly INotifier _notifier;
    #endregion

    #region Constructors
    public CustomerController(ICustomerService service, INotifier notifier)
    {
        _service = service;
        _notifier = notifier;
    }
    #endregion

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(
            [FromQuery] CustomerListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        return Ok(await _service.GetListAsync(requestDto, cancellationToken));
    }

    [HttpGet("{id:guid}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Basic(Guid id, CancellationToken cancellationToken = default)
    {
        return Ok(await _service.GetBasicAsync(id, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "CanGetCustomerDetail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Detail(Guid id, CancellationToken cancellationToken = default)
    {
        return Ok(await _service.GetDetailAsync(id, cancellationToken));
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateCustomer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
            [FromBody] CustomerUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        Guid id = await _service.CreateAsync(requestDto, cancellationToken);
        return CreatedAtAction(nameof(Detail), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "CanEditCustomer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
            Guid id,
            [FromBody] CustomerUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await _service.UpdateAsync(id, requestDto, cancellationToken);
        await _notifier.Notify(NotificationType.CustomerModification, id);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "CanDeleteCustomer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        await _notifier.Notify(NotificationType.CustomerDeletion, id);

        return Ok();
    }

    [HttpGet("ListSortingOptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetListSortingOptionsAsync()
    {
        return Ok(_service.GetListSortingOptions());
    }

    [HttpGet("CreatingPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCreatingPermission()
    {
        return Ok(_service.GetCreatingPermission());
    }

    [HttpGet("NewStatistics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNewStatistics()
    {
        return Ok(await _service.GetNewCustomerSummaryThisMonthAsync());
    }
}
