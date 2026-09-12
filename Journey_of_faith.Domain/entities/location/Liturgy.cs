using Journey_of_faith.Domain.entities.masslive;

namespace Journey_of_faith.Domain.entities.location;

public class Liturgy
{
    public int Id {get; set;}
    public int MassScheduleId {get; set;}
    public string ReadingOne {get; set;} = "";
    public string ResponsorialPsalm  {get; set;} = "";
    public string GoodNew {get; set;} = "";
    public string EndWord {get; set;} = "";
    public DateTime DateActive {get; set;} 
}