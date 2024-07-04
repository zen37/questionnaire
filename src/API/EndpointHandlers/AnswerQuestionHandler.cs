using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Survey
{
    public class AnswerQuestionHandler
    {
        public static async Task AnswerQuestion(HttpContext context)
        {
            try
            {
                Console.WriteLine("AnswerQuestion invoked ...");

                // Extract id from the URL path
                if (!Guid.TryParse(context.Request.RouteValues["id"]?.ToString(), out var id))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid id format.");
                    return;
                }

                // Configure JSON serialization options
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // Ensure case insensitivity
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                };

                // Read the JSON payload from the request body
                using (var reader = new StreamReader(context.Request.Body))
                {
                    var json = await reader.ReadToEndAsync();
                    Console.WriteLine($"Payload: {json}");

                    // Deserialize JSON string into AnswerRequest object
                    var requestBody = JsonSerializer.Deserialize<AnswerRequest>(json, options);

                    if (requestBody == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Invalid request body format.");
                        return;
                    }

                    // Set the Id from route parameter
                   // requestBody.ClientId = id;

                    // Handle different types of questions
                    switch (requestBody.Type)
                    {
                        case "Rating":
                            await HandleRating(context, requestBody, options);
                            break;
                        case "SingleSelect":
                            await HandleSingleSelect(context, requestBody, options);
                            break;
                        case "MultiSelect":
                            await HandleMultiSelect(context, requestBody, options);
                            break;
                        default:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("Unsupported or invalid question type.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log and handle exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync($"An error occurred: {ex.Message}");
            }
        }

        private static async Task HandleRating(HttpContext context, AnswerRequest requestBody, JsonSerializerOptions options)
        {
            try
            {
                // Deserialize JSON into AnswerRating object
                var answerRating = new AnswerRating
                {
                    ClientId = requestBody.ClientId,
                    DateTimeResponse = requestBody.DateTimeResponse,
                    Rating =  (int)requestBody.Rating
                };

                // Process the Rating question logic
                Console.WriteLine($"Database ... Question ID: {requestBody.ClientId}, Rating: {answerRating.Rating}, ClientId: {requestBody.ClientId}, DateTimeResponse: {requestBody.DateTimeResponse}");

                // Return a response indicating the answer was received
                var responseBody = new
                {
                    Message = "Rating answer received",
                    QuestionId = requestBody.ClientId,
                    Rating = answerRating.Rating,
                    ClientId = requestBody.ClientId,
                    DateTimeResponse = requestBody.DateTimeResponse
                };

                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(responseBody);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                Console.WriteLine($"An error occurred: {ex.Message}");
                await context.Response.WriteAsync($"Error processing Rating question: {ex.Message}");
            }
        }

        private static async Task HandleSingleSelect(HttpContext context, AnswerRequest requestBody, JsonSerializerOptions options)
        {
            try
            {
                // Deserialize JSON into AnswerSelect object for SingleSelect
                var answerSelect = new AnswerSelect
                {
                    ClientId = requestBody.ClientId,
                    DateTimeResponse = requestBody.DateTimeResponse
                };

                // Handle SingleSelect logic (assuming SelectedId is directly in the request body)
                if (requestBody.SelectedId.HasValue)
                {
                    answerSelect.Ids = new List<int> { requestBody.SelectedId.Value };
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("SelectedId is required for SingleSelect.");
                    return;
                }

                // Process the SingleSelect question logic
                Console.WriteLine($"Database ... Question ID: {requestBody.ClientId}, SelectedId: {answerSelect.Ids[0]}, ClientId: {requestBody.ClientId}, DateTimeResponse: {requestBody.DateTimeResponse}");

                // Return a response indicating the answer was received
                var responseBody = new
                {
                    Message = "SingleSelect answer received",
                    QuestionId = requestBody.ClientId,
                    SelectedId = answerSelect.Ids[0],
                    ClientId = requestBody.ClientId,
                    DateTimeResponse = requestBody.DateTimeResponse
                };

                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(responseBody);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                Console.WriteLine($"An error occurred: {ex.Message}");
                await context.Response.WriteAsync($"Error processing SingleSelect question: {ex.Message}");
            }
        }

        private static async Task HandleMultiSelect(HttpContext context, AnswerRequest requestBody, JsonSerializerOptions options)
        {
            try
            {
                // Deserialize JSON into AnswerSelect object for MultiSelect
                var answerSelect = new AnswerSelect
                {
                    ClientId = requestBody.ClientId,
                    DateTimeResponse = requestBody.DateTimeResponse
                };

                // Handle MultiSelect logic (assuming SelectedIds are directly in the request body)
                if (requestBody.SelectedIds != null && requestBody.SelectedIds.Count > 0)
                {
                    answerSelect.Ids = requestBody.SelectedIds;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("SelectedIds are required for MultiSelect.");
                    return;
                }

                // Process the MultiSelect question logic
                Console.WriteLine($"Database ... Question ID: {requestBody.ClientId}, SelectedIds: [{string.Join(",", answerSelect.Ids)}], ClientId: {requestBody.ClientId}, DateTimeResponse: {requestBody.DateTimeResponse}");

                // Return a response indicating the answer was received
                var responseBody = new
                {
                    Message = "MultiSelect answer received",
                    QuestionId = requestBody.ClientId,
                    SelectedIds = answerSelect.Ids,
                    ClientId = requestBody.ClientId,
                    DateTimeResponse = requestBody.DateTimeResponse
                };

                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(responseBody);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                Console.WriteLine($"An error occurred: {ex.Message}");
                await context.Response.WriteAsync($"Error processing MultiSelect question: {ex.Message}");
            }
        }

        public class AnswerRequest
        {
            public string Type { get; set; }
            public int ClientId { get; set; }
            public DateTime DateTimeResponse { get; set; }
            public int? Rating { get; set; } // Rating for Rating type
            public int? SelectedId { get; set; } // Nullable for SingleSelect
            public List<int> SelectedIds { get; set; } // For MultiSelect
        }
    }
}
