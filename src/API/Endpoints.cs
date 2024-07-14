namespace Survey;

public static class Endpoints
{
    public static void Configure(WebApplication app)
    {
        var question = app.Services.GetRequiredService<IQuestion>();
        var answer = app.Services.GetRequiredService<IAnswer>();

        app.MapPut("/question/{id:guid}", question.Create);
        app.MapPost("/question/{id:guid}", answer.Create);
    }
}