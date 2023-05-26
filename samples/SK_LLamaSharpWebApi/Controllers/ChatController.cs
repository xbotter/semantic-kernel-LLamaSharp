using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;

namespace SemanticKernel.LLamaSharp.WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IKernel _kernel;

    /// <summary>
    /// Crete ChatController Instance
    /// </summary>
    /// <param name="kernel"></param>
    public ChatController(IKernel kernel)
    {
        this._kernel = kernel;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost()]
    public async Task<IActionResult> ChatAsync([FromBody] UserInput input)
    {
        if (string.IsNullOrWhiteSpace(input?.Input))
        {
            return NoContent();
        }
        var completion = _kernel.GetService<ITextCompletion>();
        var response = await completion.CompleteAsync(input.Input, new CompleteRequestSettings()).ConfigureAwait(false);

        return Ok(response);
    }
}

/// <summary>
/// UserInput Model
/// </summary>
public class UserInput
{

    /// <summary>
    /// User Input
    /// </summary>
    public string Input { get; set; } = string.Empty;
}
