using LLama;
using Microsoft.SemanticKernel.AI.TextCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.SemanticKernel.Connectors.AI.LLamaSharp.TextCompletion;

// TODO: This is a copy of the ChatCompletion version.  It needs to be updated to use the TextCompletion version of the LLamaSharp model.

/// <summary>
/// Text Completion use LLamaSharp
/// </summary>
public class LLamaSharpTextCompletion : ITextCompletion, IDisposable
{
    readonly ChatSession<LLamaModel> _session;
    readonly LLamaModel _model;
    private bool isDisposed;

    /// <summary>
    /// Create LLamaSharpTextCompletion Instance
    /// </summary>
    /// <param name="modelPath">the llama model path</param>
    public LLamaSharpTextCompletion(string modelPath)
    {
        this._model = new LLamaModel(new LLamaParams(model: modelPath));
        this._session = new ChatSession<LLamaModel>(this._model);
    }
    /// <inheritdoc/>
    public Task<string> CompleteAsync(string text, CompleteRequestSettings requestSettings, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(string.Join("", _session.Chat(text)));
    }
    /// <inheritdoc/>
    public IAsyncEnumerable<string> CompleteStreamAsync(string text, CompleteRequestSettings requestSettings, CancellationToken cancellationToken = default)
    {
        return _session.Chat(text).ToAsyncEnumerable();
    }
    /// <summary>
    /// Dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                _model.Dispose();
            }
            isDisposed = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
