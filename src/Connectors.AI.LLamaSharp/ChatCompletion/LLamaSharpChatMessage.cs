using Microsoft.SemanticKernel.AI.ChatCompletion;

namespace Connectors.AI.LLamaSharp.ChatCompletion
{
    /// <summary>
    /// LLamaSharp Chat Message
    /// </summary>
    public class LLamaSharpChatMessage : ChatMessageBase
    {
        /// <inheritdoc/>
        public LLamaSharpChatMessage(AuthorRole role, string content) : base(role, content)
        {
        }
    }
}
