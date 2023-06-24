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
        private const string SystemMessage = "Transcript of a dialog, where the User interacts with an Assistant named Assistant. Assistant is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.";

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
            //if (chatHistory.Last().Role == AuthorRole.User)
            //{
            //    // add empty assistant message to trigger the model
            //    history.AddMessage(LLama.Common.AuthorRole.Assistant, string.Empty);
            //}

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
                MaxTokens = requestSettings.MaxTokens,
                InputPrefix = AuthorRole.User.ToString() + ":",
                InputSuffix = "\n" + AuthorRole.Assistant.ToString() + ":"
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
                MaxTokens = requestSettings.MaxTokens
            };
        }
    }
}
