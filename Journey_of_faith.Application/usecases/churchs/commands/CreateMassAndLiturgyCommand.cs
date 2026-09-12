using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.commands;

public class CreateMassAndLiturgyCommand : IRequest<bool>
{
    public List<CreateMassAndLiturgyItem> Items { get; set; } = new();
}

public class CreateMassAndLiturgyItem
{
    public CreateMassScheduleCommand CreateMassSchedule { get; set; } = new();
    public CreateLiturgyCommand CreateLiturgy { get; set; } = new();
}

public class CreateMassScheduleCommand
{
    public int ChurchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Time { get; set; } = string.Empty;
}

public class CreateLiturgyCommand
{
    public string Reading { get; set; } = string.Empty;
    public string ResponsorialPsalm { get; set; } = string.Empty;
    public string Gospel { get; set; } = string.Empty;
    public string EndWord { get; set; } = string.Empty;
}