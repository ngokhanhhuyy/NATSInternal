using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Application.UnitOfWork;

namespace NATSInternal.Infrastructure.Converters;

internal interface IDbExceptionConverter
{
    #region Methods
    PersistenceException? Convert(DbUpdateException exception);
    PersistenceException? Convert(DbException exception);
    #endregion
}