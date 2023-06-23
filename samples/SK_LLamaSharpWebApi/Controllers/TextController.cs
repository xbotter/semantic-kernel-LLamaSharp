using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;
using SemanticKernel.LLamaSharp.WebApi.Controllers.InputModels;

namespace SemanticKernel.LLamaSharp.WebApi.Controllers
{

    /// <summary>
    /// Text Completion Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TextController : ControllerBase
    {
        private readonly IKernel _kernel;

        /// <summary>
        /// Create TextCompletionController Instance
        /// </summary>
        /// <param name="kernel"></param>
        public TextController(IKernel kernel)
        {
            this._kernel = kernel;
        }

        /// <summary>
        /// Simple Text Completion
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> CompletionAsync([FromBody] UserInput input)
        {
            if (string.IsNullOrWhiteSpace(input?.Input))
            {
                return NoContent();
            }
            var completion = _kernel.GetService<ITextCompletion>();
            var response = await completion.CompleteAsync(input.Input, new CompleteRequestSettings() { MaxTokens = input.MaxTokens, StopSequences = input.StopSequences }).ConfigureAwait(false);
            return Ok(response);
        }

        /// <summary>
        /// Stream Text Completion
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("stream")]
        public async Task CompletionStreamAsync([FromBody] UserInput input)
        {
            if (string.IsNullOrWhiteSpace(input?.Input))
            {
                await Response.CompleteAsync().ConfigureAwait(false);
                return;
            }
            var completion = _kernel.GetService<ITextCompletion>();
            var response = completion.CompleteStreamAsync(input.Input, new CompleteRequestSettings() { MaxTokens = input.MaxTokens, StopSequences = input.StopSequences }).ConfigureAwait(false);
            Response.ContentType = "text/event-stream";

            await foreach (var r in response)
            {
                await Response.WriteAsync("data: " + r + "\n\n").ConfigureAwait(false);
                await Response.Body.FlushAsync().ConfigureAwait(false);
            }

            await Response.CompleteAsync().ConfigureAwait(false);
        }
    }
}
