using LLama;
using Microsoft.SemanticKernel.AI.Embeddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.SemanticKernel.Connectors.AI.LLamaSharp.TextCompletion;

/// <summary>
/// IEmbeddingGeneration use LLamaSharp 
/// </summary>
public class LLamaSharpEmbeddingGeneration : IEmbeddingGeneration<string, float>, IDisposable
{
    readonly LLamaEmbedder _embedder;
    private bool isDisposed;
    /// <summary>
    /// Create LLamaSharpEmbedding generation instance
    /// </summary>
    /// <param name="modelPath"></param>
    public LLamaSharpEmbeddingGeneration(string modelPath)
    {
        this._embedder = new LLamaEmbedder(new LLamaParams(model: modelPath));
    }


    /// <inheritdoc/>
    public async Task<IList<Embedding<float>>> GenerateEmbeddingsAsync(IList<string> data, CancellationToken cancellationToken = default)
    {
        var result = data.Select(text => new Embedding<float>(_embedder.GetEmbeddings(text))).ToList();
        return await Task.FromResult(result).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {

        Dispose(true);
        GC.SuppressFinalize(this);
    }
    /// <summary>
    /// Dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed) return;

        if (disposing)
        {
            _embedder.Dispose();
        }
        isDisposed = true;
    }
}
