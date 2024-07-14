using Survey;

var builder = WebApplication.CreateBuilder(args);

//register all services
Services.Add(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

Endpoints.Configure(app);

app.Run();