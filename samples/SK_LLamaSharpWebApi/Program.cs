using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.LLamaSharp.TextCompletion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IKernel>((_) =>
{
    var kernel = new KernelBuilder()
    .Configure(cfg =>
    {
        cfg.AddTextCompletionService((_) => new LLamaSharpTextCompletion(builder.Configuration["ModelPath"]));
        cfg.AddTextEmbeddingGenerationService((_) => new LLamaSharpEmbeddingGeneration(builder.Configuration["ModelPath"]));
    }).Build();
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
