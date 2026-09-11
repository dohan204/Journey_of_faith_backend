namespace Journey_of_faith.Domain.entities.masslive;


public class Liturgy : AuditableEntity
{
    public string ReadingOne {get; private set;}
    public string ResponsorialPsalm {get; private set;}
    public int MassScheduleId {get; private set;}
    public string GoodNew {get; private set;}
    public string? EndWord {get; private set;}


    public Liturgy() {}

    public Liturgy(string readingOne, string responsorialPsalm, int massScheduleId, string goodNew, string? endWord)
    {   
        if(!InputValid(readingOne, responsorialPsalm, massScheduleId, goodNew))
            throw new ArgumentException("Tham số truyền vào không hợp lệ, vui lòng kiểm tra lại");
        this.ReadingOne = readingOne;
        this.ResponsorialPsalm = responsorialPsalm;
        this.MassScheduleId = massScheduleId;
        this.GoodNew = goodNew;
        this.EndWord = endWord;
    }

    static bool InputValid(string readone, string responsorialPsalm, int massScheduleId, string goodNew)
    {
        if(
            !string.IsNullOrEmpty(readone) && !string.IsNullOrEmpty(responsorialPsalm) && !string.IsNullOrEmpty(goodNew) && massScheduleId > 0
        )
            return true;

        return false;
    }
}