using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using SemanticKernel.LLamaSharp.WebApi.Controllers.InputModels;

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
    /// Simple Chat
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
        var history = new ChatHistory();
        history.AddUserMessage(input.Input);
        var response = await chat.GenerateMessageAsync(history, new ChatRequestSettings() { MaxTokens = input.MaxTokens }).ConfigureAwait(false);
        return Ok(response);
    }

    /// <summary>
    /// Chat With History
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("history")]
    public async Task<IActionResult> ChatHistoryAsync([FromBody] UserChatInput input)
    {
        if (input is null)
        {
            return NoContent();
        }

        var chat = _kernel.GetService<IChatCompletion>();
        var history = new ChatHistory();

        history.AddRange(input.Messages);

        var response = await chat.GenerateMessageAsync(history, new ChatRequestSettings() { MaxTokens = input.MaxTokens }).ConfigureAwait(false);
        return Ok(response);
    }

    /// <summary>
    /// Chat With Stream
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("stream")]
    public async Task ChatStreamAsync([FromBody] UserInput input)
    {
        if (string.IsNullOrWhiteSpace(input?.Input))
        {
            await Response.CompleteAsync().ConfigureAwait(false);
            return;
        }
        var chat = _kernel.GetService<IChatCompletion>();
        var history = new ChatHistory();
        history.AddUserMessage(input.Input);
        var response = chat.GenerateMessageStreamAsync(history, new ChatRequestSettings() { MaxTokens = input.MaxTokens });

        Response.ContentType = "text/event-stream";

        await foreach (var r in response)
        {
            await Response.WriteAsync("data: " + r + "\n").ConfigureAwait(false);
            await Response.Body.FlushAsync().ConfigureAwait(false);
        }

        await Response.CompleteAsync().ConfigureAwait(false);
    }
}
