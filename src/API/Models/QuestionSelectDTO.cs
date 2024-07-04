namespace Survey;

public class QuestionSelectDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public List<QuestionAnswerOptionDTO> AnswerOptions { get; set; }
}

public class QuestionAnswerOptionDTO
{
    public int Id { get; set; }
    public string Text { get; set; }
}
