using System.Runtime.CompilerServices;

namespace NATSInternal.Core.Entities;

internal abstract class AbstractEntity<TEntity> : IEntity<TEntity> where TEntity : class
{
    #region ProtectedMethods
    protected static TField GetFieldOrThrowIfNull<TField>(
            TField? field,
            [CallerMemberName] string propertyName = "") where TField : class
    {
        return field ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(propertyName));
    }
    #endregion
}
