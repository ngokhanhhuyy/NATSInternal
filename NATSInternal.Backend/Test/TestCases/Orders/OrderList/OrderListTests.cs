using Bogus;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Test.Common;
using NATSInternal.Test.Extensions;
using NATSInternal.Test.Fixtures;
using NATSInternal.Test.Mock;
using System.Reflection;

namespace NATSInternal.Test.TestCases;

public class OrderListTests : IClassFixture<Fixture>
{
    #region Fields
    private readonly Fixture _fixture;
    private static readonly Faker _faker = new();
    private static readonly Random _random = new();
    private static readonly IEnumerable<string> _roleNames;
    private static readonly IEnumerable<string> _fieldNames;
    #endregion

    #region Constructors
    static OrderListTests()
    {
        _roleNames = typeof(RoleNames)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .Select(f => f.Name);

        _fieldNames = Enum.GetNames<OrderListRequestDto.FieldToSort>();
    }

    public OrderListTests(Fixture fixture)
    {
        _fixture = fixture;
    }
    #endregion

    #region StaticProperties
    public static TheoryData<OrderListRequestDto> CombinatedTestData
    {
        get
        {
            IEnumerable<string> sortByFieldNameData = Enumerable
                .Range(1, 100)
                .Select(length => _faker.Random.String(length))
                .Concat(_fieldNames);
            IEnumerable<int> pageData = Enumerable.Range(-1, 3);
            IEnumerable<int> resultsPerPageData = new[] { -1, 0, 5, 50, 51 };
            IEnumerable<int> monthData = Enumerable.Range(-1, 13);
            IEnumerable<int> yearData = Enumerable.Range(2025, 3);
            IEnumerable<ListMonthYearRequestDto?> statsMonthYearData =
                (
                    from month in monthData
                    from year in yearData
                    select new ListMonthYearRequestDto { Year = year, Month = month }
                )
                .Prepend(null);

            IEnumerable<OrderListRequestDto> requestDtoData =
                from name in sortByFieldNameData
                from page in pageData
                from resultsPerPage in resultsPerPageData
                from statsMonthYear in statsMonthYearData
                select new OrderListRequestDto
                {
                    SortByFieldName = name,
                    Page = page,
                    ResultsPerPage = resultsPerPage,
                    StatsMonthYear = statsMonthYear
                };

            TheoryData<OrderListRequestDto> requestDtos = new();
            foreach (OrderListRequestDto requestDto in requestDtoData)
            {
                requestDtos.Add(requestDto);
            }

            return requestDtos;
        }
    }
    #endregion

    #region Methods
    [Theory]
    [MemberData(nameof(CombinatedTestData))]
    public async Task GetOrderList_CombinatedParameters(OrderListRequestDto requestDto)
    {
        using IServiceScope scope = _fixture.RootProvider.CreateScope();
        await scope.InitializeDependenciesAsync(RoleNames.Developer);

        Clock clock = scope.ServiceProvider.GetRequiredService<Clock>();
        clock.SetCurrentDateTime(new(2026, 5, 10, 12, 0, 0));

        IOrderService orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        ExecutionResult<OrderListResponseDto> result = new();

        try
        {
            result.ResponseDto = await orderService.GetListAsync(requestDto);
        }
        catch (Exception exception)
        {
            result.Exception = exception;
        }

        AssertFieldName(result, requestDto.SortByFieldName);
        AssertPage(result, requestDto.Page);
        AssertResultsPerPage(result, requestDto.ResultsPerPage);
        AssertStatsMonthYear(result, requestDto.StatsMonthYear, clock);
    }
    #endregion

    #region PrivateStaticMethods
    private static void AssertFieldName(ExecutionResult<OrderListResponseDto> result, string sortByFieldName)
    {
        if (_fieldNames.Contains(sortByFieldName))
        {
            return;
        }
        
        Assert.Null(result.ResponseDto);
        Assert.IsValidationExceptionAndErrorsContainFieldName(result, nameof(OrderListRequestDto.SortByFieldName));
    }

    private static void AssertPage(ExecutionResult<OrderListResponseDto> result, int page)
    {
        if (page >= 1)
        {
            return;
        }
        
        Assert.Null(result.ResponseDto);
        Assert.IsValidationExceptionAndErrorsContainFieldName(result, nameof(OrderListRequestDto.SortByFieldName));
    }
    
    private static void AssertResultsPerPage(ExecutionResult<OrderListResponseDto> result, int resultsPerPage)
    {
        if (resultsPerPage is >= 5 and <= 50)
        {
            Assert.Null(result.ResponseDto);
            Assert.IsValidationExceptionAndErrorsContainFieldName(result, nameof(OrderListRequestDto.ResultsPerPage));
        }
    }

    private static void AssertStatsMonthYear(
        ExecutionResult<OrderListResponseDto> result,
        ListMonthYearRequestDto? statsMonthYear,
        Clock clock)
    {
        if (statsMonthYear is null)
        {
            return;
        }

        DateOnly today = clock.Today;
        bool isValid = true;
        if (statsMonthYear.Year <= 0 || statsMonthYear.Year >= today.Year)
        {
            isValid = false;
        }

        if (statsMonthYear.Month < 1 || statsMonthYear.Month > 12)
        {
            isValid = false;
        }

        if (statsMonthYear.Year == today.Year && statsMonthYear.Month > today.Month)
        {
            isValid = false;
        }

        if (!isValid)
        {
            string[] fieldNames = new[]
            {
                $"{nameof(OrderListRequestDto.StatsMonthYear)}.{nameof(OrderListRequestDto.StatsMonthYear.Month)}",
                $"{nameof(OrderListRequestDto.StatsMonthYear)}.{nameof(OrderListRequestDto.StatsMonthYear.Year)}"
            };

            Assert.IsValidationExceptionAndErrorsContainOneOfFieldNames(result, fieldNames);
        }
    }
    #endregion

    #region PrivateMethods
    private async Task InitializeAndRunTestAsync(Func<OrderListRequestDto, IServiceScope, Func<Task>, Task> runner)
    {
        using IServiceScope scope = _fixture.RootProvider.CreateScope();
        await scope.InitializeDependenciesAsync(RoleNames.Developer);

        Clock clock = scope.ServiceProvider.GetRequiredService<Clock>();
        clock.SetCurrentDateTime(new(2026, 5, 10, 12, 0, 0));

        IOrderService orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        OrderListRequestDto requestDto = new();
        await runner(requestDto, scope, async () => await orderService.GetListAsync(requestDto));
    }
    #endregion
}