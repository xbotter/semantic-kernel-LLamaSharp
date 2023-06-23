using Connectors.AI.LLamaSharp;
using Connectors.AI.LLamaSharp.TextCompletion;
using LLama;
using LLama.Common;
using Microsoft.SemanticKernel.AI.TextCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.SemanticKernel.Connectors.AI.LLamaSharp.TextCompletion;

/// <summary>
/// Text Completion use LLamaSharp
/// </summary>
public sealed class LLamaSharpTextCompletion : ITextCompletion, IDisposable
{
    private readonly string _modelPath;
    private LLamaModel? _model;


    /// <summary>
    /// Create LLamaSharpTextCompletion Instance
    /// </summary>
    /// <param name="modelPath"></param>
    public LLamaSharpTextCompletion(string modelPath)
    {
        this._modelPath = modelPath;
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _model?.Dispose();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ITextResult>> GetCompletionsAsync(string text, CompleteRequestSettings requestSettings, CancellationToken cancellationToken = default)
    {
        var executor = CreateExecutor();
        // TODO: InferAsync is not implemented in LLamaSharp, use Infer instead 
        var result = executor.Infer(text, requestSettings.ToLLamaSharpInferenceParams(), cancellationToken).ToAsyncEnumerable();
        return await Task.FromResult(new List<ITextResult> { new LLamaTextResult(result) }.AsReadOnly()).ConfigureAwait(false);
    }


    /// <inheritdoc/>
    public async IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, CompleteRequestSettings requestSettings, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var executor = CreateExecutor();
        // TODO: InferAsync is not implemented in LLamaSharp, use Infer instead 
        var result = executor.Infer(text, requestSettings.ToLLamaSharpInferenceParams(), cancellationToken).ToAsyncEnumerable();
        yield return new LLamaTextResult(result);
    }

    private StatelessExecutor CreateExecutor()
    {
        if (_model == null)
        {
            _model = new LLamaModel(new ModelParams(_modelPath, contextSize: 2049));
        }
        return new(this._model);
    }
}
