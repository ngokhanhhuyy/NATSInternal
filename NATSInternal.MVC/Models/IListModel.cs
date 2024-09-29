namespace NATSInternal.Models;

public class ListModel<TModel>
{
    public virtual string OrderByField { get; set; }
    public virtual bool OrderByAscending { get; set; }
    public virtual int Page { get; set; } = 1;
    public virtual int ResultsPerPage { get; set; } = 15;
    public virtual int PageCount { get; set; }
    public List<TModel> Items { get; set; }