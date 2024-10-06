namespace NATSInternal.Services.Dtos;

public class UserListRequestDto : IRequestDto
{
    public string OrderByField { get; set; } = nameof(FieldToBeOrdered.LastName);
    public bool OrderByAscending { get; set; } = true;
    public int? RoleId { get; set; }
    public bool JoinedRencentlyOnly { get; set; } = false;
    public bool UpcomingBirthdayOnly { get; set; } = false;
    public string Content { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
        OrderByField = OrderByField?.ToNullIfEmpty()?.SnakeCaseToPascalCase();
        Content = Content?.ToNullIfEmpty();
    }

    public enum FieldToBeOrdered
    {
        LastName,
        FirstName,
        UserName,
        Birthday,
        Age,
        CreatedDateTime,
        Role,
    }
}