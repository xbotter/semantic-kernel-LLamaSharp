using static LLama.LLamaTransforms;

namespace Connectors.AI.LLamaSharp.ChatCompletion
{
    /// <summary>
    /// Default HistoryTransform Patch
    /// </summary>
    public class HistoryTransform : DefaultHistoryTransform
    {
        /// <inheritdoc/>
        public override string HistoryToText(LLama.Common.ChatHistory history)
        {
            var prompt = base.HistoryToText(history);
            return prompt + "\nAssistant:";

        }
    }
}
