using Microsoft.Xna.Framework.Content.Pipeline;
using System.Text.Json;

using TInput = System.String;
using TOutput = Tutorial_Pipeline.Level;

namespace Tutorial_Pipeline
{
    [ContentProcessor(DisplayName = "WavesProcessor")]
    internal class WavesProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return JsonSerializer.Deserialize<Level>(input);
        }
    }
}
