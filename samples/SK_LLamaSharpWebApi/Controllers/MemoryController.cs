using Microsoft;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using SemanticKernel.LLamaSharp.WebApi.Controllers.InputModels;

namespace SemanticKernel.LLamaSharp.WebApi.Controllers
{

    /// <summary>
    /// Semantic Kernel Memory Sample Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MemoryController : ControllerBase
    {
        private readonly IKernel _kernel;
        private const string Collection = nameof(Collection);

        /// <summary>
        /// Create MemoryController Instance
        /// </summary>
        /// <param name="kernel"></param>
        public MemoryController(IKernel kernel)
        {
            this._kernel = kernel;
        }

        /// <summary>
        /// Save Memory
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<string> StoreMemory([FromBody] UserInput input)
        {
            Requires.NotNull(input, nameof(input));

            var id = Guid.NewGuid().ToString("N");
            var uid = await _kernel.Memory.SaveInformationAsync(Collection, input.Input, id).ConfigureAwait(false);
            return uid;
        }

        /// <summary>
        /// Search Memory
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<IActionResult> MemorySearch([FromBody] UserInput input)
        {
            Requires.NotNull(input, nameof(input));

            var result = await _kernel.Memory.SearchAsync(Collection, input.Input).FirstAsync().ConfigureAwait(false);
            return Ok(result);
        }
    }
}
