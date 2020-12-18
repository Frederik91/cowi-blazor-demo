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
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ISession _session;

        public QueryExecutor(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISession session)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _session = session;
        }
        
        public Task<TResult> HandleAsync<TResult>(IQuery<TResult> query)
        {
            return HandleAsync(query, Context.Revit, CancellationToken.None);
        }

        public Task<TResult> HandleAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            return HandleAsync(query, Context.Revit, cancellationToken);
        }

        public Task<TResult> HandleAsync<TResult>(IQuery<TResult> query, Context context)
        {
            return HandleAsync(query, context, CancellationToken.None);
        }

        public async Task<TResult> HandleAsync<TResult>(IQuery<TResult> query, Context context, CancellationToken cancellationToken)
        {
            var queryJson = JsonConvert.SerializeObject(query, _configuration.JsonSerializerSettings);
            var message = new QueryMessage
            {
                Context = context,
                QueryJson = queryJson
            };

            var httpClient = _httpClientFactory.CreateClient("cqrs");
            var result = await httpClient.PostAsJsonAsync($"http://localhost:{_session.SelectedPort}/api/messaging/Query/Execute", message, cancellationToken);
            result.EnsureSuccessStatusCode();
            var resultJson = await result.Content.ReadAsStringAsync();
            var resultMessage = JsonConvert.DeserializeObject<QueryResultMessage>(resultJson);

            if (resultMessage.IsSuccessful)
                return JsonConvert.DeserializeObject<TResult>(resultMessage.QueryResultJson, _configuration.JsonSerializerSettings);

            throw new Exception(resultMessage.Exception.Message);
        }
    }
}
