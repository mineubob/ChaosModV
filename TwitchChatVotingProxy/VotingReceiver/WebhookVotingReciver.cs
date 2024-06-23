using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared;
using TwitchChatVotingProxy.ChaosPipe;

namespace TwitchChatVotingProxy.VotingReceiver
{
    class WebhookAddVoteRequest {
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; } = null;
        [JsonPropertyName("username")]
        public string? Username { get; set; } = null;
        [JsonPropertyName("option")]
        public string? Option { get; set; } = null;
    }

    class WebhookVotingReciver : IVotingReceiver, IDisposable
    {
        public event EventHandler<OnMessageArgs>? OnMessage = null;

        private readonly int? m_Port;

        private readonly IPAddress? m_Address;

        private readonly ChaosPipeClient m_ChaosPipe;

        private WebApplication m_app; 

        private readonly Serilog.ILogger m_Logger = Log.Logger.ForContext<WebhookVotingReciver>();

        public WebhookVotingReciver(OptionsFile config, ChaosPipeClient chaosPipe)
        {
            m_Port = config.ReadValueInt("WebhookPort", 8080);

            string? address = config.ReadValue("WebhookAddress", "127.0.0.1");
            if (address != null)
            {
                if (!IPAddress.TryParse(address, out m_Address))
                {
                    m_Logger.Warning("Webhook Address failed to parse as ip. Failing back to 127.0.0.1");
                }
            }

            var builder = WebApplication.CreateBuilder();
            builder.WebHost.ConfigureKestrel((context, serverOptions) =>
            {
                serverOptions.Listen(m_Address ?? IPAddress.Loopback, m_Port ?? 8080);
            });

            m_app = builder.Build();
            m_app.MapPost("/", PostHandler);

            m_ChaosPipe = chaosPipe;
        }

        public async Task<bool> Init()
        {
            await m_app.StartAsync();

            return true;
        }

        private async Task<IResult> PostHandler(HttpContent context)
        {
            WebhookAddVoteRequest? request = await context.ReadFromJsonAsync<WebhookAddVoteRequest>();
            if (request == null)
            {
                return Results.BadRequest("");
            }

            OnMessage?.Invoke(this, new OnMessageArgs() {
                ClientId = request.ClientId,
                Username = request.Username,
                Message = request.Option,
            });

            return Results.CreatedAtRoute();
        }

        public Task SendMessage(string message)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            m_app.StopAsync().Wait();
        }
    }
}