using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Test.Extensions;
using NATSInternal.Test.Fixtures;
using NATSInternal.Test.Mock;

namespace NATSInternal.Test.TestCases;

public class OrderDetailTests : IClassFixture<Fixture>
{
    #region Fields
    private readonly Fixture _fixture;
    #endregion

    #region Constructors
    public OrderDetailTests(Fixture fixture)
    {
        _fixture = fixture;
    }
    #endregion

    #region Methods
    [Fact]
    public async Task GetOrderDetail_ExistingId_ShouldWork()
    {
        
    }
    #endregion
}