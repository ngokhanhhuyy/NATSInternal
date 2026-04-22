namespace NATSInternal.Core.Handlers;

internal interface IDbExceptionHandler
{
    #region Methods
    DbExceptionHandledResult? Handle(DbUpdateException exception);
    DbExceptionHandledResult? Handle(DbException exception);
    #endregion
}