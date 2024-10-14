namespace NATSInternal.Services.Dtos;

internal class EntityListDto<T> where T : class, IUpsertableEntity<T>, new()
{
    public int PageCount { get; set; }
    public List<T> Items { get; set; }
}