using LLama;
using LLama.Common;
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
public sealed class LLamaSharpEmbeddingGeneration : ITextEmbeddingGeneration, IDisposable
{
    private LLamaEmbedder? _embedder;
    private readonly string _modelPath;

    /// <summary>
    /// Create LLamaSharpEmbedding generation instance
    /// </summary>
    /// <param name="modelPath"></param>
    public LLamaSharpEmbeddingGeneration(string modelPath)
    {
        this._modelPath = modelPath;
    }

    /// <inheritdoc/>
    public async Task<IList<Embedding<float>>> GenerateEmbeddingsAsync(IList<string> data, CancellationToken cancellationToken = default)
    {
        this._embedder = new LLamaEmbedder(new ModelParams(_modelPath, gpuLayerCount: 0));
        var result = data.Select(text => new Embedding<float>(_embedder.GetEmbeddings(text))).ToList();
        return await Task.FromResult(result).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _embedder?.Dispose();
    }
}
