namespace Survey;

public static class Endpoints
{
    public static void Configure(WebApplication app)
    {
    var createQuestionHandler = app.Services.GetRequiredService<CreateQuestionHandler>();
    var answerQuestionHandler = app.Services.GetRequiredService<AnswerQuestionHandler>();

    app.MapPut("/question/{id:guid}", createQuestionHandler.CreateQuestion);
    app.MapPost("/question/{id:guid}", answerQuestionHandler.AnswerQuestion);
    }
}