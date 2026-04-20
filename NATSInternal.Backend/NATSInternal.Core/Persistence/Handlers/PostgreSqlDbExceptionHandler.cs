using System.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NATSInternal.Core.Persistence.DbContext;
using Npgsql;

namespace NATSInternal.Core.Persistence.Handlers;

internal partial class PostgreSqlDbExceptionHandler : IDbExceptionHandler
{
    #region Fields
    private readonly AppDbContext _context;
    #endregion

    #region Constructors
    public PostgreSqlDbExceptionHandler(AppDbContext context)
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
        if (exception is not PostgresException postgreSqlException)
        {
            return null;
        }

        string? tableName = null;
        string? columnName = null;
        DbExceptionHandledResult convertedException = new()
        {
            ViolatedEntityName = GetEntityNameFromTableName(postgreSqlException.TableName)
        };

        Match match;
        switch (postgreSqlException.SqlState)
        {
            case "23505":
                convertedException.IsUniqueConstraintViolation = true;
                match = GetUniqueConstraintNameRegex().Match(exception.Message);
                if (match.Success)
                {
                    tableName = match.Groups["tableName"].Value;
                    columnName = match.Groups["columnName"].Value;
                }

                break;
            case "23502":
                convertedException.IsNotNullConstraintViolation = true;
                break;
            case "23503":
                convertedException.IsForeignKeyConstraintViolation = true;
                match = GetForeignKeyConstraintNameRegex().Match(exception.Message);
                if (match.Success)
                {
                    tableName = match.Groups["referencingTableName"].Value;
                    columnName = match.Groups["referencingColumnName"].Value;
                }

                break;
        }
        
        IEntityType? entityType = _context.Model
            .GetEntityTypes()
            .SingleOrDefault(et => et.GetTableName() == tableName);

        if (entityType is not null)
        {
            convertedException.ViolatedEntityName = entityType.ClrType.Name;
            convertedException.ViolatedPropertyName = entityType
                .GetProperties()
                .Where(p => p.GetColumnName() == columnName)
                .Select(p => p.Name)
                .SingleOrDefault();
        }

        return convertedException;
    }
    #endregion

    #region PrivateMethods
    private string? GetEntityNameFromTableName(string? tableName)
    {
        return _context.Model
            .GetEntityTypes()
            .Select(type => type.GetTableName())
            .SingleOrDefault(name => name == tableName);
    }
    #endregion

    #region StaticMethods
    [GeneratedRegex(@"IX_(?<tableName>[A-Z][a-zA-Z]+)_(?<columnName>[A-Z][a-zA-Z0-9_]+)")]
    private static partial Regex GetUniqueConstraintNameRegex();

    [GeneratedRegex(@"FK_(?<referencingTableName>[A-Z][a-zA-Z]+)_(?<referencedTableName>[A-Z][a-zA-Z]+)_(?<referencingColumnName>[A-Z][a-zA-Z0-9_]+)")]
    private static partial Regex GetForeignKeyConstraintNameRegex();
    #endregion
}