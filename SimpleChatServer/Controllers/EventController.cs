using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Channels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleChatServer.Constants;
using SimpleChatServer.Services;
using SimpleChatServer.Utils;
using static SimpleChatServer.Services.EventService;

namespace SimpleChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AuthRoles.User)]
    public class EventController : ControllerBase
    {
        public EventController(
            EventService eventService,
            IOptions<JsonSerializerOptions> jsonSerializerOptions)
        {
            EventService = eventService;
            EventChannel = Channel.CreateUnbounded<ServerEventArgs>();
            JsonSerializerOptions = jsonSerializerOptions.Value;

            EventService.ServerEvent += EventService_ServerEvent;
        }

        ~EventController()
        {
            EventService.ServerEvent -= EventService_ServerEvent;
        }

        private void EventService_ServerEvent(object? sender, ServerEventArgs e)
        {
            while (!EventChannel.Writer.TryWrite(e))
            {
            }
        }

        public EventService EventService { get; }
        public JsonSerializerOptions JsonSerializerOptions { get; }
        public Channel<ServerEventArgs> EventChannel { get; }

        [HttpGet]
        public async Task Get()
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            var response = Response;
            response.Headers.Add("Content-Type", "text/event-stream");

            while (true)
            {
                var serverEventArgs = await EventChannel.Reader.ReadAsync();

                if (userId != serverEventArgs.UserId && serverEventArgs.UserId != -1)
                    continue;

                JsonNode? node = JsonSerializer.SerializeToNode(serverEventArgs.EventData, options: JsonSerializerOptions);

                if (node is not JsonObject jobj)
                    continue;

                jobj["UserId"] = serverEventArgs.UserId;

                await response.WriteAsync($"event: {serverEventArgs.EventKind.ToString()}\r\ndata: {node.ToJsonString(JsonSerializerOptions)}\r\n\r\n");
                await response.Body.FlushAsync();
            }
        }
    }
}
