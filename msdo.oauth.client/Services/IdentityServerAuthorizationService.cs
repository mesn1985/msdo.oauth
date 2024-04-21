﻿using IdentityModel.Client;
using msdo.oauth.client.Interfaces;

namespace msdo.oauth.client.Services
{
    public class IdentityServerAuthorizationService : IAuthorizationService
    {
        private IConfiguration _configuration;
        private ILogger<IdentityServerAuthorizationService> _logger;

        public IdentityServerAuthorizationService(IConfiguration configuration, ILogger<IdentityServerAuthorizationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GetAccessToken(string clientId, string clientSecret)
        {
            var client = new HttpClient();
            
            var discoveryEndpointUrl
                = _configuration.GetValue<string>("Services:AuthorizationServerDiscoveryEndpoint");
            _logger.LogInformation($"Requesting metadata from: {discoveryEndpointUrl}");
            
            var metaDataFromDiscoveryEndpoint 
                = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = discoveryEndpointUrl,
                    Policy =
                    {
                        /// By default, GetDiscoveryDocumentAsync method, requires the protocol to be Https.
                        /// If Https is not used, no exception will be thrown. Instead, an error message will be returned
                        RequireHttps = false
                    }
                });

            if (metaDataFromDiscoveryEndpoint.IsError)
            {
                _logger.LogError($"Request for metadata from the endpoint failed with error: {metaDataFromDiscoveryEndpoint.Error}");
                throw new HttpRequestException(
                    $"An error occoured when trying to connect with discovery endpoint at: {discoveryEndpointUrl}, the error  code was: {metaDataFromDiscoveryEndpoint.HttpStatusCode}\nError message is: {metaDataFromDiscoveryEndpoint.Error}");
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = metaDataFromDiscoveryEndpoint.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "protectedResource"
            });

            return tokenResponse.AccessToken;
        }

    }
}
