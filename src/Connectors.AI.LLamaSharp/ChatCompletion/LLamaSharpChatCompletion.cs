using LLama;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connectors.AI.LLamaSharp.ChatCompletion
{
    public class LLamaSharpChatCompletion : IChatCompletion, IDisposable
    {
        private readonly string _modelPath;
        private readonly string _promptPath;
        private readonly List<string> _antiprompt;
        private LLamaModel _model = null;
        const string UserPrefix = "User:";

        public LLamaSharpChatCompletion(string modelPath, string promptPath, List<string> antiprompt = null)
        {
            this._modelPath = modelPath;
            this._promptPath = promptPath;
            this._antiprompt = antiprompt;
        }
        private void CreateModel(int max_tokens = 256)
        {
            if (_model is not null)
            {
                return;
            }
            _model = new LLamaModel(new LLamaParams(model: _modelPath, n_threads: 10, prompt: File.ReadAllText(_promptPath), antiprompt: _antiprompt, n_ctx: max_tokens, interactive: true, repeat_penalty: 1.0f));
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
            CreateModel(maxTokens);
            var antiPrompt = new List<string> { UserPrefix }.Concat(requestSettings?.StopSequences).ToArray();
            _model.InitChatAntiprompt(antiPrompt);
            var input = HistoryToPrompt(chat);
            var response = _model.Chat(input);
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
            CreateModel(requestSettings.MaxTokens);
            _model.InitChatAntiprompt(new[] { UserPrefix });
            var result = _model.Chat(HistoryToPrompt(chat));
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
                    //sb.Append(UserPrefix);
                    sb.AppendLine(item.Content);
                }
                else if (item.AuthorRole == ChatHistory.AuthorRoles.Assistant)
                {
                    //sb.Append(BotPrefix);
                    sb.AppendLine(item.Content);
                }
            }
            //sb.Append(BotPrefix);
            return sb.ToString();
        }

        public void Dispose()
        {
            if (_model is not null)
            {
                _model.Dispose();
            }
        }
    }
}
