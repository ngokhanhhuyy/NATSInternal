using System.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.DbContext;
using MySqlConnector;

namespace NATSInternal.Core.Handlers;

internal partial class MySqlExceptionHandler : IDbExceptionHandler
{
    #region Fields
    private readonly DatabaseContext _context;
    #endregion

    #region Constructors
    public MySqlExceptionHandler(DatabaseContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    public DbExceptionHandledResult? Handle(DbUpdateException exception)
    {
        if (exception is DbUpdateConcurrencyException)
        {
            return new()
            {
                IsConcurrencyConflict = true
            };
        }

        if (exception.InnerException is not DbException innerException)
        {
            return null;
        }

        return Handle(innerException);
    }
    
    public DbExceptionHandledResult? Handle(DbException exception)
    {
        if (exception.InnerException is not MySqlException mysqlException)
        {
            return null;
        }

        DbExceptionHandledResult handledResult = new();
        Match match;
        switch (mysqlException.Number)
        {
            case 1062:
                handledResult.IsUniqueConstraintViolation = true;
                match = GetUniqueConstraintRegex().Match(exception.Message);
                if (match.Success)
                {
                    string violatedConstraintName = match.Groups["constraintName"].Value;
                    handledResult.ViolatedTableName = match.Groups["tableName"].Value;
                    handledResult.ViolatedFieldName = violatedConstraintName.Split("__").Last();
                    handledResult.ViolatedEntityName = _context.Model
                        .GetEntityTypes()
                        .Select(type => type.GetTableName())
                        .Single(name => name == match.Groups["tableName"].Value);
                }

                break;

            case 1364:
                handledResult.IsNotNullConstraintViolation = true;
                match = GetNotNullConstraintRegex().Match(exception.Message);
                if (match.Success)
                {
                    handledResult.ViolatedFieldName = match.Groups["columnName"].Value;
                }

                break;
            case 1451:
                handledResult.IsForeignKeyConstraintViolation = true;
                match = GetDeleteOrUpdateRestrictedRegex().Match(exception.Message);
                if (match.Success)
                {
                    handledResult.ViolatedTableName = match.Groups["tableName"].Value;
                    handledResult.ViolatedFieldName = match.Groups["columnName"].Value;
                }

                break;

            case 1452:
                handledResult.IsForeignKeyConstraintViolation = true;
                match = GetForeignKeyNotFoundRegex().Match(exception.Message);
                if (match.Success)
                {
                    handledResult.ViolatedTableName = match.Groups["tableName"].Value;
                    handledResult.ViolatedFieldName = match.Groups["columnName"].Value;
                    handledResult.ViolatedEntityName = _context.Model
                        .GetEntityTypes()
                        .Select(type => type.GetTableName())
                        .Single(name => name == match.Groups["tableName"].Value);
                }

                break;
        }

        return handledResult;
    }
    #endregion

    #region StaticMethods
    
    [GeneratedRegex(@"Duplicate entry\s+\'(?<duplicatedKeyValue>.+)\'\s+for key\s+\'(?<tableName>\w+)\.(?<constraintName>\w+)'")]
    private static partial Regex GetUniqueConstraintRegex();

    [GeneratedRegex(@"Field\s+\'(?<columnName>.+)\'\s+doesn't have a default value")]
    private static partial Regex GetNotNullConstraintRegex();

    [GeneratedRegex(@"Data truncation: Data too long for column '(?<columnName>.+)' at row (?<rowNumber>.+)")]
    private static partial Regex GetMaxLengthConstraintRegex();

    [GeneratedRegex(@"(?<databaseName>.+?)\.`(?<tableName>.+)`, CONSTRAINT `(?<constraintName>.+)` FOREIGN KEY \(`(?<columnName>.+)`\) REFERENCES `(?<referencedTableName>.+)` \(`(?<referencedColumnName>.+)`\)")]
    private static partial Regex GetForeignKeyNotFoundRegex();

    [GeneratedRegex(@"Cannot delete or update a parent row: a foreign key constraint fails \(`(?<databaseName>.+)`\.`(?<tableName>.+)`, CONSTRAINT `(?<constraintName>.+)` FOREIGN KEY \(`(?<columnName>.+)`\) REFERENCES `(?<referenceTableName>.+)`\)")]
    private static partial Regex GetDeleteOrUpdateRestrictedRegex();
    #endregion
}