using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

namespace SemanticKernel.LLamaSharp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemoryController : ControllerBase
    {
        private readonly IKernel _kernel;
        private const string Collection = nameof(Collection);

        public MemoryController(IKernel kernel)
        {
            this._kernel = kernel;
        }
        [HttpPost("save")]
        public async Task<string> StoreMemory([FromBody] UserInput input)
        {
            var id = Guid.NewGuid().ToString("N");
            var uid = await _kernel.Memory.SaveInformationAsync(Collection, input.Input, id).ConfigureAwait(false);
            return uid;
        }
        [HttpPost("search")]
        public async Task<IActionResult> MemorySearch([FromBody] UserInput input)
        {
            var result = await _kernel.Memory.SearchAsync(Collection, input.Input).FirstAsync().ConfigureAwait(false);
            return Ok(result);
        }
    }
}
