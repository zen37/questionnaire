namespace Survey;

public enum QuestionType
{
    Rating,
    SingleSelect,
    MultiSelect
}


public abstract class Question
{
    public Guid Id { get; set; }
    // public in SurveyId { get; set; }
    public QuestionType Type { get; set; }
    public string Title { get; set; }
    //public string Language { get; set; }
    public string CreatedBy { get; set; }
    public DateTime DateTimeCreated { get; set; }

}