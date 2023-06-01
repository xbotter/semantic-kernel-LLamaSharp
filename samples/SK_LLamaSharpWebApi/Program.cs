using Connectors.AI.LLamaSharp.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.LLamaSharp.TextCompletion;
using Microsoft.SemanticKernel.Connectors.Memory.Sqlite;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.Embeddings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMemoryStore, VolatileMemoryStore>();
builder.Services.AddSingleton<IKernel>((sp) =>
{
    string prompt = File.ReadAllText(builder.Configuration["PromptPath"]);
    var kernel = new KernelBuilder()
    .WithAIService<IChatCompletion>("llama_chat_completion", new LLamaSharpChatCompletion(builder.Configuration["ModelPath"], builder.Configuration["PromptPath"], new List<string>() { "User:" }), true)
    .WithAIService<ITextCompletion>("llama_text_completion", new LLamaSharpTextCompletion(builder.Configuration["ModelPath"], prompt, new string[] { "User:" }), true)
    .WithAIService<ITextEmbeddingGeneration>("llama_text_embedding", new LLamaSharpEmbeddingGeneration(builder.Configuration["ModelPath"]))
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
