using IdentityModel.Client;
using msdo.oauth.client.Interfaces;

namespace msdo.oauth.client.Services
{
    public class ProtectedResourceHttpService : IProtectedResourceService
    {
        private ILogger<ProtectedResourceHttpService> _logger;
        private IConfiguration _configuration;

        public ProtectedResourceHttpService(ILogger<ProtectedResourceHttpService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GetProtectedResource(string accessToken)
        {

            var protectedResourceServerHttpClient = new HttpClient();

            protectedResourceServerHttpClient.SetBearerToken(accessToken);

            var protectedResourceServerUrl 
                = _configuration.GetValue<string>("Services:ProtectedResourceServer");

            _logger.LogInformation($"Request for protected resource sent to: {protectedResourceServerUrl}");

            var responseFromProtectedResourceServer =
                await protectedResourceServerHttpClient.GetAsync(protectedResourceServerUrl);

            if (!responseFromProtectedResourceServer.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unable to retrieve Protected resource from the resource  server");
            }

            return await responseFromProtectedResourceServer.Content.ReadAsStringAsync();
        }

    }
}
