namespace NATSInternal.Controllers;

[ApiController]
[Route("Api/Stats")]
[Authorize]
public class StatsController : ControllerBase
{
    private readonly IStatsService _service;
    private readonly IValidator<MonthlyStatsRequestDto> _monthlyValidator;
    private readonly IValidator<LastestMonthlyStatsRequestDto> _lastestMonthlyStatsValidator;
    private readonly IValidator<LastestDailyStatsRequestDto> _lastestDailyStatsValidator;

    public StatsController(
            IStatsService service,
            IValidator<MonthlyStatsRequestDto> monthlyValidator,
            IValidator<LastestMonthlyStatsRequestDto> lastestMonthlyStatsValidator,
            IValidator<LastestDailyStatsRequestDto> lastestDailyStatsValidator)
    {
        _service = service;
        _monthlyValidator = monthlyValidator;
        _lastestMonthlyStatsValidator = lastestMonthlyStatsValidator;
        _lastestDailyStatsValidator = lastestDailyStatsValidator;
    }

    [HttpGet("Daily")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DailyStats([FromQuery] DateOnly? recordedDate)
    {
        try
        {
            return Ok(await _service.GetDailyDetailAsync(recordedDate));
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
    }

    [HttpGet("Monthly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MonthlyStats(
            [FromQuery] MonthlyStatsRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _monthlyValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Fetch the monthly stats.
        try
        {
            return Ok(await _service.GetMonthlyDetailAsync(requestDto));
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
    }

    [HttpGet("LastestMonthly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLastestMonthly(
            [FromQuery] LastestMonthlyStatsRequestDto requestDto)
    {
        // Validate data from the request.
        ValidationResult validationResult;
        validationResult = _lastestMonthlyStatsValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok(await _service.GetLastestMonthlyAsync(requestDto));
    }

    [HttpGet("LastestDailyBasic")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLastestDailyBasic(
            [FromQuery] LastestDailyStatsRequestDto requestDto)
    {
        // Validate data from the request.
        ValidationResult validationResult;
        validationResult = _lastestDailyStatsValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok(await _service.GetLastestDailyBasicAsync(requestDto));
    }

    [HttpGet("LastestDailyDetail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLastestDailyDetail(
            [FromQuery] LastestDailyStatsRequestDto requestDto)
    {
        // Validate data from the request.
        ValidationResult validationResult;
        validationResult = _lastestDailyStatsValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok(await _service.GetLastestDailyDetailAsync(requestDto));
    }
}