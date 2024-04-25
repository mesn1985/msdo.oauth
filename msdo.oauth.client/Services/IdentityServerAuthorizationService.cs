using System.Runtime.InteropServices;
using IdentityModel.Client;
using msdo.oauth.client.Interfaces;

namespace msdo.oauth.client.Services
{
    public class IdentityServerAuthorizationService : IAuthorizationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdentityServerAuthorizationService> _logger;

        public IdentityServerAuthorizationService(IConfiguration configuration, ILogger<IdentityServerAuthorizationService> logger, HttpClient httpClient)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessToken(string clientId, string clientSecret, string correlationId)
        {
            AddCorrelationIdHeader(correlationId);

            _logger.LogInformation($"Requesting metadata from: {_httpClient.BaseAddress}");

            string tokenEndpointAddress = await GetTokenEndpointAddressFromDiscoveryEndpoint();

            return await RequestToken(tokenEndpointAddress, clientId, clientSecret);
        }

        private void AddCorrelationIdHeader(string correlationId)
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                "X-Correlation-Id",
                correlationId
            );
        }
        private async Task<string> RequestToken(string tokenEndpointAdress, string clientId, string clientSecret)
        {
            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = tokenEndpointAdress,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = "protectedResource"
            });

            return tokenResponse.AccessToken;
        }

        private async Task<string> GetTokenEndpointAddressFromDiscoveryEndpoint()
        {
            var responseFromDiscoveryEndpoint
                = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Policy =
                    {
                        /// By default, GetDiscoveryDocumentAsync method, requires the protocol to be Https.
                        /// If Https is not used, no exception will be thrown. Instead, an error message will be returned
                        RequireHttps = false,
                        /// The issue name is validated against the URL used as discovery endpoint
                        /// In Docker compose, this causes some issues do to docker network dns 
                        ValidateIssuerName = false
                    }
                });

            if (responseFromDiscoveryEndpoint.IsError)
            {
                _logger.LogError($"Request for metadata from the discovery endpoint failed with error: {responseFromDiscoveryEndpoint.Error}");
                throw new HttpRequestException(
                    $"An error occoured when trying to connect with discovery endpoint at: {_httpClient.BaseAddress}, the error  code was: {responseFromDiscoveryEndpoint.HttpStatusCode}\nError message is: {responseFromDiscoveryEndpoint.Error}");
            }

            return responseFromDiscoveryEndpoint.TokenEndpoint;
        }

    }
}
