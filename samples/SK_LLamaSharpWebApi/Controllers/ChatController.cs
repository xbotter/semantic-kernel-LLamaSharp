using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
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
        var chat = _kernel.GetService<IChatCompletion>();
        var history = chat.CreateNewChat();
        history.AddMessage(ChatHistory.AuthorRoles.User, input.Input);
        var response = await chat.GenerateMessageAsync(history, new ChatRequestSettings() { MaxTokens = 256 }).ConfigureAwait(false);
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
