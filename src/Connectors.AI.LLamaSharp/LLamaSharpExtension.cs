using Microsoft;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectors.AI.LLamaSharp
{
    /// <summary>
    /// LLamaSharp Extension
    /// </summary>
    public static class LLamaSharpExtension
    {

        /// <summary>
        /// Convert ChatHistory to LLamaSharp ChatHistory
        /// </summary>
        /// <param name="chatHistory"></param>
        /// <returns></returns>
        public static LLama.Common.ChatHistory ToLLamaSharpChatHistory(this ChatHistory chatHistory)
        {
            Requires.NotNull(chatHistory, nameof(chatHistory));

            var history = new LLama.Common.ChatHistory();

            foreach (var chat in chatHistory)
            {
                var role = Enum.TryParse<LLama.Common.AuthorRole>(chat.Role.Label, out var _role) ? _role : LLama.Common.AuthorRole.Unknown;
                history.AddMessage(role, chat.Content);
            }

            return history;
        }

        /// <summary>
        /// Convert ChatRequestSettings to LLamaSharp InferenceParams
        /// </summary>
        /// <param name="requestSettings"></param>
        /// <returns></returns>
        public static LLama.Common.InferenceParams ToLLamaSharpInferenceParams(this ChatRequestSettings requestSettings)
        {
            Requires.NotNull(requestSettings, nameof(requestSettings));

            var antiPrompts = new List<string>(requestSettings.StopSequences)
            {
                AuthorRole.User.ToString() + ":"
            };
            return new LLama.Common.InferenceParams
            {
                Temperature = (float)requestSettings.Temperature,
                TopP = (float)requestSettings.TopP,
                PresencePenalty = (float)requestSettings.PresencePenalty,
                FrequencyPenalty = (float)requestSettings.FrequencyPenalty,
                AntiPrompts = antiPrompts,
                MaxTokens = requestSettings.MaxTokens ?? -1
            };
        }

        /// <summary>
        /// Convert CompleteRequestSettings to LLamaSharp InferenceParams
        /// </summary>
        /// <param name="requestSettings"></param>
        /// <returns></returns>
        public static LLama.Common.InferenceParams ToLLamaSharpInferenceParams(this CompleteRequestSettings requestSettings)
        {
            Requires.NotNull(requestSettings, nameof(requestSettings));

            return new LLama.Common.InferenceParams
            {
                Temperature = (float)requestSettings.Temperature,
                TopP = (float)requestSettings.TopP,
                PresencePenalty = (float)requestSettings.PresencePenalty,
                FrequencyPenalty = (float)requestSettings.FrequencyPenalty,
                AntiPrompts = requestSettings.StopSequences,
                MaxTokens = requestSettings.MaxTokens ?? -1
            };
        }
    }
}
