using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.commands;


public class UpdateChurchCommand : IRequest<int>
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Email {get; set;}
    public  string? Address {get; set;}
    public int DioceseId {get; set;}
    public float? Longitude {get; set;}
    public float? Latitude {get; set;}
    public string? Boss {get; set;}
    public string? Description {get; set;}
    public Guid UserId {get; set;}
    public List<UpdateMassScheduleCommand> MassSchedules {get; set;}
}

public class UpdateMassScheduleCommand
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Time {get; set;}
    public int MassTypeId {get; set;}
}