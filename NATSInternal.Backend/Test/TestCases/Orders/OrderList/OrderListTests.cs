using Bogus;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Test.Extensions;
using NATSInternal.Test.Fixtures;
using NATSInternal.Test.Mock;
using System.Reflection;
using ValidationException = FluentValidation.ValidationException;

namespace NATSInternal.Test.TestCases;

public class OrderListTests : IClassFixture<Fixture>
{
    #region Fields
    private readonly Fixture _fixture;
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
    public static IEnumerable<object[]> CombinatedTestData
    {
        get
        {
            IEnumerable<string> sortByFieldNameData = ;
            IEnumerable<int> pageData = Enumerable.Range(-100, 1000);
            IEnumerable<int> resultsPerPageData = Enumerable.Range(-100, 100);
            IEnumerable<int> monthData = Enumerable.Range(-1, 13);
            IEnumerable<int> yearData = Enumerable.Range(2020, 2030);
            IEnumerable<ListMonthYearRequestDto?> statsMonthYearData =
                (
                    from month in monthData
                    from year in yearData
                    select new ListMonthYearRequestDto { Year = year, Month = month }
                )
                .Prepend(null);

            return
                from name in sortByFieldNameData
                from page in pageData
                from resultsPerPage in resultsPerPageData
                from statsMonthYear in statsMonthYearData
                select new object[]
                {
                    new OrderListRequestDto
                    {
                        SortByFieldName = name,
                        Page = page,
                        ResultsPerPage = resultsPerPage,
                        StatsMonthYear = statsMonthYear
                    }
                };
        }
    }
    #endregion

    #region Methods
    [Fact]
    public async Task GetOrderList_NoParameter_ShouldWork()
    {
        await InitializeAndRunTestAsync(async (requestDto, _, getListAsync) =>
        {
            await getListAsync();
        });
    }

    [Theory]
    [InlineData(nameof(OrderListRequestDto.FieldToSort.StatsDate))]
    [InlineData(nameof(OrderListRequestDto.FieldToSort.CreatedDateTime))]
    [InlineData(nameof(OrderListRequestDto.FieldToSort.LastUpdatedDateTime))]
    [InlineData(nameof(OrderListRequestDto.FieldToSort.ProductItemsAmount))]
    [InlineData(nameof(OrderListRequestDto.FieldToSort.ServiceItemsAmount))]
    [InlineData(nameof(OrderListRequestDto.FieldToSort.TotalAmount))]
    public async Task GetOrderList_ValidSortByFieldName_ShouldWork(string sortByFieldName)
    {
        await InitializeAndRunTestAsync(async (requestDto, _, getListAsync) =>
        {
            requestDto.SortByFieldName = sortByFieldName;
            await getListAsync();
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("___")]
    [InlineData("123")]
    public async Task GetOrderList_InvalidSortByFieldName_ShouldThrowValidationException(string sortByFieldName)
    {
        await InitializeAndRunTestAsync(async (requestDto, _, getListAsync) =>
        {
            requestDto.SortByFieldName = sortByFieldName;

            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await getListAsync();
            });

            List<string> propertyNames = exception.Errors.Select(f => f.PropertyName).ToList();
            Assert.Single(propertyNames);
            Assert.Contains(nameof(OrderListRequestDto.SortByFieldName), propertyNames);
        });
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(30)]
    public async Task GetOrderList_ValidPage_ShouldWork(int page)
    {
        await InitializeAndRunTestAsync(async (requestDto, _, getListAsync) =>
        {
            requestDto.Page = page;
            await getListAsync();
        });
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-1.5)]
    [InlineData(0)]
    public async Task GetOrderList_InvalidPage_ShouldThrowValidationException(int page)
    {
        await InitializeAndRunTestAsync(async (requestDto, _, getListAsync) =>
        {
            requestDto.Page = page;

            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await getListAsync();
            });

            List<string> propertyNames = exception.Errors.Select(f => f.PropertyName).ToList();
            Assert.Single(propertyNames);
            Assert.Contains(nameof(OrderListRequestDto.Page), propertyNames);
        });
    }
    
    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(49)]
    [InlineData(50)]
    public async Task GetOrderList_ValidResultsPerPage_ShouldWork(int resultsPerPage)
    {
        await InitializeAndRunTestAsync(async (requestDto, _, getListAsync) =>
        {
            requestDto.ResultsPerPage = resultsPerPage;
            await getListAsync();
        });
    }

    [Theory]
    [InlineData(4)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(60)]
    [InlineData(51)]
    [InlineData(100)]
    public async Task GetOrderList_InvalidResultsPerPage_ShouldThrowValidationException(int resultsPerPage)
    {
        await InitializeAndRunTestAsync(async (requestDto, _, getListAsync) =>
        {
            requestDto.ResultsPerPage = resultsPerPage;

            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await getListAsync();
            });

            List<string> propertyNames = exception.Errors.Select(f => f.PropertyName).ToList();
            Assert.Single(propertyNames);
            Assert.Contains(nameof(OrderListRequestDto.ResultsPerPage), propertyNames);
        });
    }

    [Theory]
    [InlineData(2026, 5)]
    [InlineData(2025, 12)]
    [InlineData(1995, 2)]
    [InlineData(1000, 9)]
    [InlineData(0, 1)]
    public async Task GetOrderList_ValidStatsMonthYear_ShouldWork(int year, int month)
    {
        await InitializeAndRunTestAsync(async (requestDto, _, getListAsync) =>
        {
            requestDto.StatsMonthYear = new()
            {
                Year = year,
                Month = month
            };

            await getListAsync();
        });
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(2030, 5)]
    [InlineData(9990, 0)]
    [InlineData(-1, 12)]
    [InlineData(2026, -1)]
    [InlineData(2026, 0)]
    public async Task GetOrderList_InvalidStatsMonthYear_ShouldThrowValidationException(int year, int month)
    {
        await InitializeAndRunTestAsync(async (requestDto, _, getListAsync) =>
        {
            requestDto.StatsMonthYear = new()
            {
                Year = year,
                Month = month
            };

            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await getListAsync();
            });

            List<string> propertyNames = exception.Errors.Select(f => f.PropertyName).ToList();
            Assert.Contains(
                propertyNames,
                name => name is
                    $"{nameof(requestDto.StatsMonthYear)}.{nameof(requestDto.StatsMonthYear.Month)}" or
                    $"{nameof(requestDto.StatsMonthYear)}.{nameof(requestDto.StatsMonthYear.Year)}");
        });
    }

    [Theory]
    [MemberData(nameof(CombinatedTestData))]
    public async Task GetOrderList_CombinatedParameters(OrderListRequestDto requestDto)
    {
        using IServiceScope scope = _fixture.RootProvider.CreateScope();
        await scope.InitializeDependenciesAsync(RoleNames.Developer);

        Clock clock = scope.ServiceProvider.GetRequiredService<Clock>();
        clock.SetCurrentDateTime(new(2026, 5, 10, 12, 0, 0));

        IOrderService orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        bool isValid = false;
        if (!requestDto.SortByFieldName)

        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await getListAsync();
        });

        List<string> propertyNames = exception.Errors.Select(f => f.PropertyName).ToList();
        Assert.Contains(
            propertyNames,
            name => name is
                $"{nameof(originalRequestDto.StatsMonthYear)}.{nameof(originalRequestDto.StatsMonthYear.Month)}" or
                $"{nameof(originalRequestDto.StatsMonthYear)}.{nameof(originalRequestDto.StatsMonthYear.Year)}");
        });
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