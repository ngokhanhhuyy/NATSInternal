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
    private readonly IValidator<TopSoldProductListRequestDto> _topSoldProductListValidator;
    private readonly IValidator<TopPurchasedCustomerListRequestDto> _topPurchasedCustomerListValidator;
    private readonly IValidator<LastestTransactionsRequestDto> _lastestTransactionsValidator;

    public StatsController(
            IStatsService service,
            IValidator<MonthlyStatsRequestDto> monthlyValidator,
            IValidator<LastestMonthlyStatsRequestDto> lastestMonthlyStatsValidator,
            IValidator<LastestDailyStatsRequestDto> lastestDailyStatsValidator,
            IValidator<TopSoldProductListRequestDto> topSoldProductListValidator,
            IValidator<TopPurchasedCustomerListRequestDto> topPurchasedCustomerListValidator,
            IValidator<LastestTransactionsRequestDto> lastestTransactionsValidator)
    {
        _service = service;
        _monthlyValidator = monthlyValidator;
        _lastestMonthlyStatsValidator = lastestMonthlyStatsValidator;
        _lastestDailyStatsValidator = lastestDailyStatsValidator;
        _topSoldProductListValidator = topSoldProductListValidator;
        _topPurchasedCustomerListValidator = topPurchasedCustomerListValidator;
        _lastestTransactionsValidator = lastestTransactionsValidator;
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

    [HttpGet("StatsDateOptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatsDateOptions()
    {
        return Ok(await _service.GetStatsDateOptionsAsync());
    }

    [HttpGet("TopSoldProductList")]
    [ResponseCache(Duration=300)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTopSoldProductList(
            [FromQuery] TopSoldProductListRequestDto requestDto)
    {
        // Validate data from the request.
        ValidationResult validationResult;
        validationResult = _topSoldProductListValidator.Validate(requestDto.TransformValues());
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok(await _service.GetTopSoldProductListAsync(requestDto));
    }

    [HttpGet("TopPurchasedCustomerList")]
    [ResponseCache(Duration=300)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTopPurchasedCustomerList(
            [FromQuery] TopPurchasedCustomerListRequestDto requestDto)
    {
        // Validate data from the request.
        ValidationResult validationResult = _topPurchasedCustomerListValidator
            .Validate(requestDto.TransformValues());
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok(await _service.GetTopPurchasedCustomerListAsync(requestDto));
    }

    [HttpGet("LastestTransactions")]
    [ResponseCache(Duration=60)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLastestTransactionsAsync(
            [FromQuery] LastestTransactionsRequestDto requestDto)
    {
        // Validate data from the request.
        ValidationResult validationResult;
        validationResult = _lastestTransactionsValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok(await _service.GetLastestTransactionsAsync(requestDto));
    }

    [HttpGet("TopSoldProductRangeTypeOptions")]
    [ResponseCache(Duration=60 * 60 * 24)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetTopSoldProductRangeTypeOptions()
    {
        return Ok(_service.GetTopSoldProductRangeTypeOptions());
    }
}