using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace NATSInternal.Core.Persistence.Handlers;

internal interface IDbExceptionHandler
{
    #region Methods
    DbExceptionHandledResult? Handle(DbUpdateException exception);
    DbExceptionHandledResult? Handle(DbException exception);
    #endregion
}