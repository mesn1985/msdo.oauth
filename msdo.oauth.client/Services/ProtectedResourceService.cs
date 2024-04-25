using IdentityModel.Client;
using msdo.oauth.client.Interfaces;

namespace msdo.oauth.client.Services
{
    public class ProtectedResourceService : IProtectedResourceService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProtectedResourceService> _logger;
        private readonly IConfiguration _configuration;

        public ProtectedResourceService(HttpClient httpClient, ILogger<ProtectedResourceService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<string> GetProtectedResource(string accessToken, string correlationId)
        {

            SetBearerToken(accessToken);
            AddCorrelationIdHeader(correlationId);

            _logger.LogInformation($"Request for protected resource sent to: {_httpClient.BaseAddress}");

            var responseFromProtectedResourceServer =
                await _httpClient.GetAsync( "/Resource");


            if (!responseFromProtectedResourceServer.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unable to retrieve Protected resource from the resource  server");
            }

            return await responseFromProtectedResourceServer.Content.ReadAsStringAsync();
        }

        private void SetBearerToken(string accessToken)
        {
            _httpClient.SetBearerToken(accessToken);
        }

        private void AddCorrelationIdHeader(string correlationId)
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                "X-Correlation-Id",
                correlationId
                );
        }


    }
}
