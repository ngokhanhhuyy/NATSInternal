using System.Text.RegularExpressions;

namespace NATSInternal.Services;

/// <summary>
/// A handler to handle the exception thrown by the database during the saving operation.
/// </summary>
internal partial class SqlExceptionHandler
{

    private string _violatedTableName;
    private string _violatedFieldName;
    private string _violatedConstraintName;
    private object _violatedValue;

    public SqlExceptionHandler(MySqlException exception)
    {
        Match match;
        switch (exception.Number)
        {
            case 1062:
                IsUniqueConstraintViolated = true;
                match = UniqueConstraintRegex().Match(exception.Message);
                if (match.Success)
                {
                    _violatedTableName = match.Groups["tableName"].Value;
                    _violatedConstraintName = match.Groups["constraintName"].Value;
                    _violatedFieldName = ViolatedConstraintName.Split("__").Last();
                    _violatedValue = match.Groups["duplicatedKeyValue"].Value;
                }

                break;

            case 1364:
                IsNotNullConstraintViolated = true;
                match = NotNullConstraintRegex().Match(exception.Message);
                if (match.Success)
                {
                    _violatedFieldName = match.Groups["columnName"].Value;
                }

                break;

            case 1406:
                IsMaxLengthExceeded = true;
                match = MaxLengthConstraintRegex().Match(exception.Message);
                if (match.Success)
                {
                    _violatedFieldName = match.Groups["columnName"].Value;
                }

                break;

            case 1451:
                IsDeleteOrUpdateRestricted = true;
                match = DeleteOrUpdateRestrictedRegex().Match(exception.Message);
                if (match.Success)
                {
                    _violatedConstraintName = match.Groups["constraintName"].Value;
                    _violatedTableName = match.Groups["tableName"].Value;
                    _violatedFieldName = match.Groups["columnName"].Value;
                }

                break;

            case 1452:
                IsForeignKeyNotFound = true;
                match = ForeignKeyNotFoundRegex().Match(exception.Message);
                if (match.Success)
                {
                    _violatedConstraintName = match.Groups["constraintName"].Value;
                    _violatedTableName = match.Groups["tableName"].Value;
                    _violatedFieldName = match.Groups["columnName"].Value;
                }

                break;
        }
    }

    public bool IsUniqueConstraintViolated { get; protected set; }

    public bool IsNotNullConstraintViolated { get; protected set; }

    public bool IsMaxLengthExceeded { get; protected set; }

    public bool IsForeignKeyNotFound { get; protected set; }

    public bool IsDeleteOrUpdateRestricted { get; protected set; }

    public string ViolatedTableName
    {
        get => _violatedTableName;
        protected set => _violatedTableName = value.SnakeCaseToPascalCase();
    }

    public string ViolatedFieldName
    {
        get => _violatedFieldName;
        set => _violatedFieldName = value
            .SnakeCaseToPascalCase()
            .Replace("Username", "UserName");
    }

    public string ViolatedConstraintName
    {
        get => _violatedConstraintName;
        set => _violatedConstraintName = value
            .SnakeCaseToPascalCase()
            .Replace("Username", "UserName");
    }

    public object ViolatedValue => _violatedValue;

    [GeneratedRegex(@"Duplicate entry\s+\'(?<duplicatedKeyValue>.+)\'\s+for key\s+\'(?<tableName>\w+)\.(?<constraintName>\w+)'")]
    private static partial Regex UniqueConstraintRegex();

    [GeneratedRegex(@"Field\s+\'(?<columnName>.+)\'\s+doesn't have a default value")]
    private static partial Regex NotNullConstraintRegex();

    [GeneratedRegex(@"Data truncation: Data too long for column '(?<columnName>.+)' at row (?<rowNumber>.+)")]
    private static partial Regex MaxLengthConstraintRegex();

    [GeneratedRegex(@"(?<databaseName>.+?)\.`(?<tableName>.+)`, CONSTRAINT `(?<constraintName>.+)` FOREIGN KEY \(`(?<columnName>.+)`\) REFERENCES `(?<referencedTableName>.+)` \(`(?<referencedColumnName>.+)`\)")]
    private static partial Regex ForeignKeyNotFoundRegex();

    [GeneratedRegex(@"Cannot delete or update a parent row: a foreign key constraint fails \(`(?<databaseName>.+)`\.`(?<tableName>.+)`, CONSTRAINT `(?<constraintName>.+)` FOREIGN KEY \(`(?<columnName>.+)`\) REFERENCES `(?<referenceTableName>.+)`\)")]
    private static partial Regex DeleteOrUpdateRestrictedRegex();
}
