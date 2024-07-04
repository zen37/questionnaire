using System.Text.Json;

namespace Survey;
public static class DataRepository
{
    private static readonly IConfiguration Configuration;

    static DataRepository()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }

    public static async Task Save<T>(T question) where T : Question
    {
        // Read storage type from configuration
        string storageType = Configuration["StorageType"];

        if (string.IsNullOrEmpty(storageType))
        {
            throw new ArgumentException("StorageType not configured.");
        }

        switch (storageType.ToLower())
        {
            case "sqlserver":
                await SaveSQL(question);
                break;
            case "nosql":
                await SaveNoSQL(question);
                break;
            default:
                throw new ArgumentException($"Unsupported storage type: {storageType}");
        }
    }

    // Simulate saving in a SQL database
    private static async Task SaveSQL<T>(T q) where T : Question
    {
        if (q is QuestionFiveStarRating ratingQuestion)
        {
            Console.WriteLine($"SQL Database ... Question ID: {q.Id}, Type: {q.Type}, Title: {q.Title}, MinValue: {ratingQuestion.MinValue}, MaxValue: {ratingQuestion.MaxValue}, CreatedBy: {q.CreatedBy}, DateTimeCreated: {q.DateTimeCreated}");
        }
        else if (q is QuestionSelect selectQuestion)
        {
            Console.WriteLine($"SQL Database ... Single or Multi select question");
        }
    }
    // Simulate saving in a NoSQL database
    private static async Task SaveNoSQL<T>(T question) where T : Question
    {
        string serializedQuestion = JsonSerializer.Serialize(question);
        Console.WriteLine($"NoSQL Database ... Serialized Question: {serializedQuestion}");
    }
}