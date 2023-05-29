using LLama;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connectors.AI.LLamaSharp.ChatCompletion
{
    public class LLamaSharpChatCompletion : IChatCompletion
    {
        private readonly string _modelPath;
        const string UserPrefix = "User:";
        const string BotPrefix = "Bot:";

        public LLamaSharpChatCompletion(string modelPath)
        {
            this._modelPath = modelPath;
        }
        private LLamaModel CreateModel(int max_tokens = 256)
        {
            return new LLamaModel(new LLamaParams(model: _modelPath, n_predict: max_tokens, interactive: true, verbose_prompt: true));
        }
        public ChatHistory CreateNewChat(string instructions = "")
        {
            var history = new ChatHistory();
            history.AddMessage(ChatHistory.AuthorRoles.System, instructions);
            return history;
        }

        public Task<string> GenerateMessageAsync(ChatHistory chat, ChatRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
        {
            chat ??= CreateNewChat();
            var maxTokens = requestSettings?.MaxTokens ?? 256;
            using var model = CreateModel(maxTokens);
            var antiPrompt = new List<string> { UserPrefix }.Concat(requestSettings?.StopSequences).ToArray();
            model.InitChatAntiprompt(antiPrompt);
            var response = model.Chat(HistoryToPrompt(chat));
            var sb = new StringBuilder();
            foreach (var res in response)
            {
                sb.Append(res);
            }
            var result = sb.ToString();
            foreach (var anti in antiPrompt)
            {
                if (result.EndsWith(anti, StringComparison.CurrentCultureIgnoreCase))
                {
                    result = result.Substring(0, result.Length - anti.Length);
                    break;
                }
            }
            return Task.FromResult(result);
        }

        public IAsyncEnumerable<string> GenerateMessageStreamAsync(ChatHistory chat, ChatRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
        {
            using var model = CreateModel(requestSettings.MaxTokens);
            model.InitChatAntiprompt(new[] { UserPrefix });
            var result = model.Chat(HistoryToPrompt(chat));
            return result.ToAsyncEnumerable();
        }

        private static string HistoryToPrompt(ChatHistory chat)
        {
            var sb = new StringBuilder();
            foreach (var item in chat.Messages.Take(chat.Messages.Count))
            {
                if (item.AuthorRole == ChatHistory.AuthorRoles.System)
                {
                    sb.AppendLine(item.Content);
                }
                else if (item.AuthorRole == ChatHistory.AuthorRoles.User)
                {
                    sb.Append(UserPrefix);
                    sb.AppendLine(item.Content);
                }
                else if (item.AuthorRole == ChatHistory.AuthorRoles.Assistant)
                {
                    sb.Append(BotPrefix);
                    sb.AppendLine(item.Content);
                }
            }
            sb.Append(BotPrefix);
            return sb.ToString();
        }
    }
}
