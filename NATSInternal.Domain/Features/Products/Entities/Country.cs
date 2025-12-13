using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Products;

internal class Country : AbstractEntity
{
    #region Constructors
    #nullable disable
    private Country() { }
    #nullable enable

    public Country(string name, string code)
    {
        Name = name;
        Code = code;
    }
    #endregion

    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string Code { get; private set; }
    #endregion
}