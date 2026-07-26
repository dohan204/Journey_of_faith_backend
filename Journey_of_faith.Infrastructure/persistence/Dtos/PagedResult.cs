namespace Journey_of_faith.Infrastructure.persistence.Dtos;

public class PagedResult<T> where T : class
{
    public List<T> Data {get; set;}
    public int Page {get; set;}
    public int PageSize {get; set;}
    public int TotalCount {get; set;}
}