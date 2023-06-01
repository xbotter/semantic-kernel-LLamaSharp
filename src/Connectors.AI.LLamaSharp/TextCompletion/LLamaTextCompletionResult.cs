using Microsoft.SemanticKernel.AI.TextCompletion;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Connectors.AI.LLamaSharp.TextCompletion
{
    internal class LLamaTextCompletionResult : ITextCompletionStreamingResult
    {
        private readonly IEnumerable<string> _text = null;

        public LLamaTextCompletionResult(IEnumerable<string> text)
        {
            _text = text;
        }

        public LLamaTextCompletionResult(string text)
        {
            _text = new List<string>() { text };
        }

        public Task<string> GetCompletionAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(string.Join("", _text));
        }

        public async IAsyncEnumerable<string> GetCompletionStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (string word in _text)
            {
                yield return word;
            }
        }
    }
}
