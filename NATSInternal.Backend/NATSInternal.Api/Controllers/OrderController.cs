using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Features.Orders;

namespace NATSInternal.Api.Controllers;

[Route("api/orders")]
[ApiController]
[Authorize]
public class OrderController : ControllerBase
{
    #region Fields
    private readonly IOrderService _service;
    #endregion

    #region Constructors
    public OrderController(IOrderService service)
    {
        _service = service;
    }
    #endregion

    #region Methods
    [HttpGet]
    [ProducesResponseType<OrderListResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> List([FromQuery] OrderListRequestDto requestDto)
    {
        return Ok(await _service.GetListAsync(requestDto));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<OrderDetailResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Detail([FromRoute] int id)
    {
        return Ok(await _service.GetDetailAsync(id));
    }

    [HttpPost]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] OrderUpsertRequestDto requestDto)
    {
        int id = await _service.CreateAsync(requestDto);
        return CreatedAtAction(nameof(Detail), new { id }, id);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] OrderUpsertRequestDto requestDto)
    {
        await _service.UpdateAsync(id, requestDto);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("revenue")]
    [ProducesResponseType<CountResponseDto<long>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Revenue([FromQuery] CountRequestDto requestDto)
    {
        return Ok(await _service.GetRevenueAsync(requestDto));
    }

    [HttpGet("count")]
    [ProducesResponseType<CountResponseDto<int>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Count([FromQuery] CountRequestDto requestDto)
    {
        return Ok(await _service.GetCountAsync(requestDto));
    }

    [HttpGet("consultant-count")]
    [ProducesResponseType<CountResponseDto<int>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ConsultantCount([FromQuery] CountRequestDto requestDto)
    {
        return Ok(await _service.GetConsultantCountAsync(requestDto));
    }

    [HttpGet("retail-count")]
    [ProducesResponseType<CountResponseDto<int>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RetailCount([FromQuery] CountRequestDto requestDto)
    {
        return Ok(await _service.GetRetailCountAsync(requestDto));
    }

    [HttpGet("treatment-count")]
    [ProducesResponseType<CountResponseDto<int>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> TreatmentCount([FromQuery] CountRequestDto requestDto)
    {
        return Ok(await _service.GetTreatmentCountAsync(requestDto));
    }

    [HttpGet("stats-month-year-series")]
    [ProducesResponseType<List<StatsMonthYearResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetStatsMonthYearSeries()
    {
        return Ok(await _service.GetStatsMonthYearSeriesAsync());
    }
    #endregion
}
