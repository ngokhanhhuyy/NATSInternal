namespace NATSInternal.Controllers;

[ApiController]
[Route("Api/Stats")]
[Authorize]
public class StatsController : ControllerBase
{
    private readonly ISummaryService _service;
    private readonly IValidator<MonthlyStatsRequestDto> _monthlyValidator;
    private readonly IValidator<LatestMonthlyStatsRequestDto> _lastestMonthlyStatsValidator;
    private readonly IValidator<LatestDailyStatsRequestDto> _lastestDailyStatsValidator;
    private readonly IValidator<TopSoldProductListRequestDto> _topSoldProductListValidator;
    private readonly IValidator<TopPurchasedCustomerListRequestDto> _topPurchasedCustomerListValidator;
    private readonly IValidator<LatestTransactionsRequestDto> _latestTransactionValidator;

    public StatsController(
            ISummaryService service,
            IValidator<MonthlyStatsRequestDto> monthlyValidator,
            IValidator<LatestMonthlyStatsRequestDto> lastestMonthlyStatsValidator,
            IValidator<LatestDailyStatsRequestDto> lastestDailyStatsValidator,
            IValidator<TopSoldProductListRequestDto> topSoldProductListValidator,
            IValidator<TopPurchasedCustomerListRequestDto> topPurchasedCustomerListValidator,
            IValidator<LatestTransactionsRequestDto> lastestTransactionsValidator)
    {
        _service = service;
        _monthlyValidator = monthlyValidator;
        _lastestMonthlyStatsValidator = lastestMonthlyStatsValidator;
        _lastestDailyStatsValidator = lastestDailyStatsValidator;
        _topSoldProductListValidator = topSoldProductListValidator;
        _topPurchasedCustomerListValidator = topPurchasedCustomerListValidator;
        _latestTransactionValidator = lastestTransactionsValidator;
    }

    [HttpGet("Daily")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DailyStats([FromQuery] DateOnly? recordedDate)
    {
        return Ok(await _service.GetDailyDetailAsync(recordedDate));
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
        catch (NotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
    }

    [HttpGet("LatestMonthly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLatestMonthly(
            [FromQuery] LatestMonthlyStatsRequestDto requestDto)
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

    [HttpGet("LatestDailyBasic")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLatestDailyBasic(
            [FromQuery] LatestDailyStatsRequestDto requestDto)
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

    [HttpGet("LatestDailyDetail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLatestDailyDetail(
            [FromQuery] LatestDailyStatsRequestDto requestDto)
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

    [HttpGet("LatestTransactions")]
    [ResponseCache(Duration=60)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLastestTransactionsAsync(
            [FromQuery] LatestTransactionsRequestDto requestDto)
    {
        // Validate data from the request.
        ValidationResult validationResult;
        validationResult = _latestTransactionValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok(await _service.GetLatestTransactionsAsync(requestDto));
    }

    [HttpGet("TopSoldProductRangeTypeOptions")]
    [ResponseCache(Duration=60 * 60 * 24)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetTopSoldProductRangeTypeOptions()
    {
        return Ok(_service.GetTopSoldProductRangeTypeOptions());
    }
}