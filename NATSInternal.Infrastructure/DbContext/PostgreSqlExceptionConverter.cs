using System.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NATSInternal.Application.UnitOfWork;
using Npgsql;

namespace NATSInternal.Infrastructure.DbContext;

internal partial class PostgreSqlExceptionConverter : IDbExceptionConverter
{
    #region Fields
    private readonly AppDbContext _context;
    #endregion

    #region Constructors
    public PostgreSqlExceptionConverter(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    public PersistenceException? Convert(DbUpdateException exception)
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

        return Convert(innerException);
    }

    public PersistenceException? Convert(DbException exception)
    {
        if (exception is not PostgresException postgreSqlException)
        {
            return null;
        }

        string? tableName = null;
        string? columnName = null;
        PersistenceException convertedException = new()
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
            .Where(et => et.GetTableName() == tableName)
            .SingleOrDefault();

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