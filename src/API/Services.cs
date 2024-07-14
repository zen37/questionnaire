namespace Survey;

public static class Services
{
    public static void Add(WebApplicationBuilder? builder)
    {
        string storageType = builder.Configuration["StorageType"];

        //register services based on configuration value
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

        builder.Services.AddScoped<IQuestion, QuestionHandler>();
        builder.Services.AddScoped<IAnswer, AnswerHandler>();

    }
}