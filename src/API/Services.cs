namespace Survey;

public static class Services
{
    public static void Add(WebApplicationBuilder? builder)
    {
        string storageType = builder.Configuration["StorageType"];

        // Register services based on configuration value
        switch (storageType.ToLower())
        {
            case "sqlserver":
                builder.Services.AddSingleton<IStorage, SqlStorage>();
                break;
            case "nosql":
                //builder.Services.AddSingleton<IStorage, NoSqlStorage>();
                break;
            default:
                throw new ArgumentException($"Unsupported storage type: {storageType}");
        }

        // Register handlers
        builder.Services.AddScoped<CreateQuestionHandler>();
        builder.Services.AddScoped<AnswerQuestionHandler>();

    }
}