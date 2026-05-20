namespace Journey_of_faith.Api.types.resolvers;

public class Query
{
    public string GetDetailsTest() => "Hello anh em nhé";
    public Book Book()
    {
        return new Book
        {
            Id = 1, 
            Name = "book"
        };
    }
}

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
}