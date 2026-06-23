using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Features.Customers;

namespace NATSInternal.Api.Controllers;

[Route("api/customers")]
[ApiController]
[Authorize]
public class CustomerController : ControllerBase
{
    #region Fields
    private readonly ICustomerService _service;
    #endregion

    #region Constructors
    public CustomerController(ICustomerService service)
    {
        _service = service;
    }
    #endregion
    
    #region Methods
    [HttpGet]
    [ProducesResponseType<CustomerListResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetList([FromQuery] CustomerListRequestDto requestDto)
    {
        return Ok(await _service.GetListAsync(requestDto));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<CustomerDetailResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Detail([FromRoute] int id)
    {
        return Ok(await _service.GetDetailAsync(id));
    }

    [HttpPost]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CustomerUpsertRequestDto requestDto)
    {
        int createdId = await _service.CreateAsync(requestDto);
        return CreatedAtAction(nameof(Detail), new { id = createdId }, createdId);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CustomerUpsertRequestDto requestDto)
    {
        await _service.UpdateAsync(id, requestDto);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("count")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Count()
    {
        return Ok(await _service.GetCountAsync());
    }

    [HttpGet("having-debt-count")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> HavingDebtCount()
    {
        return Ok(await _service.GetHavingDebtCountAsync());
    }

    [HttpGet("refund-needed-count")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefundNeededCount()
    {
        return Ok(await _service.GetRefundNeededCountAsync());
    }

    [HttpGet("new-count")]
    [ProducesResponseType<CountOverTimeRangeResponseDto<int>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> NewCount([FromQuery] CountOverTimeRangeRequestDto requestDto)
    {
        return Ok(await _service.GetNewCountAsync(requestDto));
    }

    [HttpGet("purchased-count")]
    [ProducesResponseType<CountOverTimeRangeResponseDto<int>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PurchasedCount([FromQuery] CountOverTimeRangeRequestDto requestDto)
    {
        return Ok(await _service.GetPurchasedCountAsync(requestDto));
    }
    #endregion
}
