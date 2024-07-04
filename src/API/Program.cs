using Survey;

var builder = WebApplication.CreateBuilder(args);

// Call Services.Add to register all services and handlers
Services.Add(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

Endpoints.Configure(app);

app.Run();