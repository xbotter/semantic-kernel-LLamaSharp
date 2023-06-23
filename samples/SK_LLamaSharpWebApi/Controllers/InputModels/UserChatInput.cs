using Connectors.AI.LLamaSharp.ChatCompletion;

namespace SemanticKernel.LLamaSharp.WebApi.Controllers.InputModels;

/// <summary>
/// User Chat Input Model
/// </summary>
public class UserChatInput
{
    /// <summary>
    /// Chat Messages
    /// </summary>
    public IList<LLamaSharpChatMessage> Messages { get; } = new List<LLamaSharpChatMessage>();

    /// <summary>
    /// Max Tokens
    /// </summary>
    public int MaxTokens { get; } = 256;
}
