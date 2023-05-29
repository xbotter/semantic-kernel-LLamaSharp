using Connectors.AI.LLamaSharp.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.LLamaSharp.TextCompletion;
using Microsoft.SemanticKernel.Connectors.Memory.Sqlite;
using Microsoft.SemanticKernel.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMemoryStore, VolatileMemoryStore>();
builder.Services.AddScoped<IKernel>((sp) =>
{
    var kernel = new KernelBuilder()
    .Configure(cfg =>
    {
        cfg.AddChatCompletionService((_) => new LLamaSharpChatCompletion(builder.Configuration["ModelPath"]));
        cfg.AddTextCompletionService((_) => new LLamaSharpTextCompletion(builder.Configuration["ModelPath"]));
        cfg.AddTextEmbeddingGenerationService((_) => new LLamaSharpEmbeddingGeneration(builder.Configuration["ModelPath"]));
    })
    .WithMemoryStorage(sp.GetRequiredService<IMemoryStore>())
    .Build();
    return kernel;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
