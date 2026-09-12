using Journey_of_faith.Infrastructure.persistence.entities.faith_notifications;

namespace Journey_of_faith.Infrastructure.persistence.entities.location;

public class Liturgy
{
    public int Id {get; set;}
    public int MassScheduleId {get; set;}
    public MassSchedule MassSchedule {get; set;}
    public string ReadingOne {get;set;}
    public string ResponsorialPsalm {get; set;}
    public string GoodNew {get; set;}
    public string EndWord {get; set;}
    public DateTime DateActive {get; set;}
}