using System.Text.Json.Serialization;

namespace SemanticKernel.LLamaSharp.WebApi.Controllers.InputModels;

/// <summary>
/// UserInput Model
/// </summary>
public class UserInput
{

    /// <summary>
    /// User Input
    /// </summary>
    public string Input { get; set; } = string.Empty;

    /// <summary>
    /// Stop Sequences
    /// </summary>
    [JsonPropertyName("stop")]
    public IList<string> StopSequences { get; } = new List<string> { };

    /// <summary>
    /// Max Tokens
    /// </summary>
    public int MaxTokens { get; } = 256;
}
