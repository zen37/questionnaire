using System.Text.Json;
using System.Text.Json.Serialization;

namespace Survey;
public class QuestionHandler : IQuestion
{
    public async Task Create(HttpContext context)
    {
        try
        {
            // Extract id from the URL path
            if (!Guid.TryParse(context.Request.RouteValues["id"]?.ToString(), out var id))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid id format.");
                return;
            }
            Console.WriteLine($"id: {id}");

            using var reader = new StreamReader(context.Request.Body);
            var json = await reader.ReadToEndAsync();
            Console.WriteLine($"Payload: {json}");

            var responseBody = await HandleQuestionAsync(json, id, context);

            if (responseBody != null)
            {
                context.Response.StatusCode = StatusCodes.Status201Created;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(responseBody);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Unsupported or invalid question type.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync($"An error occurred: {ex.Message}");
        }
    }

    private static async Task<object> HandleQuestionAsync(string json, Guid id, HttpContext context)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), new QuestionConverter() }
            };

            Question requestBody = JsonSerializer.Deserialize<Question>(json, options);

            if (requestBody == null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return null;
            }

            requestBody.Id = id;
            requestBody.DateTimeCreated = DateTime.UtcNow;

            switch (requestBody)
            {
                case QuestionFiveStarRating q when requestBody is QuestionFiveStarRating:
                    if (!ValidateQuestion.IsFiveStarRatingValid(q))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Invalid rating question.");
                        return null;
                    }
                    await DataRepository.Save(q);
                    return q;

                case QuestionSelect q when requestBody is QuestionSelect:
                    if (!ValidateQuestion.IsSelectValid(q))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Invalid select question.");
                        return null;
                    }
                    await DataRepository.Save(q);
                    return q;

                default:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return null;
            }
        }
        catch (JsonException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            await context.Response.WriteAsync($"Error deserializing JSON: {ex.Message}");
            return null;
        }
    }
}