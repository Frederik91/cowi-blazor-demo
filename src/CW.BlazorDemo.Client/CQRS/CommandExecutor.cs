using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CW.BlazorDemo.Client.Models;
using CW.Contracts.Contracts;
using CW.Contracts.CQRS;
using CW.Contracts.Enums;
using CW.MessagingContracts.Models;
using Newtonsoft.Json;

namespace CW.BlazorDemo.Client.CQRS
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ISession _session;

        public CommandExecutor(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISession session)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _session = session;
        }

        public Task ExecuteAsync<TCommand>(TCommand command)
        {
            return ExecuteAsync(command, Context.Revit, CancellationToken.None);
        }

        public Task ExecuteAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        {
            return ExecuteAsync(command, Context.Revit, cancellationToken);
        }

        public Task ExecuteAsync<TCommand>(TCommand command, Context context)
        {
            return ExecuteAsync(command, context, CancellationToken.None);
        }

        public async Task ExecuteAsync<TCommand>(TCommand command, Context context, CancellationToken cancellationToken)
        {
            var commandJson = JsonConvert.SerializeObject(command, _configuration.JsonSerializerSettings);
            var message = new CommandMessage()
            {
                Context = context,
                CommandJson = commandJson
            };
            var httpClient = _httpClientFactory.CreateClient("cqrs");
            var result = await httpClient.PostAsJsonAsync("http://localhost:{_session.SelectedPort}/api/messaging/Command/Execute", message, cancellationToken);
            result.EnsureSuccessStatusCode();
            var resultJson = await result.Content.ReadAsStringAsync();
            var resultMessage = JsonConvert.DeserializeObject<CommandResultMessage>(resultJson);

            if (!resultMessage.IsSuccessful)
                throw new Exception(resultMessage.Exception.Message);
        }
    }
}
