namespace Journey_of_faith.Domain.entities;


public class UserActive
{
    public int Id { get; set; }
    public string AccountId {get; set;}
    public bool Status {get; set;}
    public string? ActiveLocation {get; set;}
    public int Timespan {get; set;}
    public DateTime ActiveDate {get; set;}
}