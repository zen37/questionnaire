using Survey;

var builder = WebApplication.CreateBuilder(args);

//enables the API Explorer, which is a service that provides metadata about the HTTP API, used by Swagger to generate the Swagger document.
builder.Services.AddEndpointsApiExplorer();
//adds the Swagger OpenAPI document generator to the application services and configures it to provide more information about the API,
builder.Services.AddOpenApiDocument(config =>
    {
        config.DocumentName = "QuestionsAPI";
        config.Title = "QuestionsAPI v1";
        config.Version = "v1";
    });

var app = builder.Build();

ConfigureEndpoints(app);

app.Run();


void ConfigureEndpoints(WebApplication app)
{
    app.MapPut("/question/{id:guid}", CreateQuestionHandler.CreateQuestion);
    app.MapPost("/question/{id:guid}", AnswerQuestionHandler.AnswerQuestion);
}