namespace msdo.oauth.client.Interfaces
{
    public interface IAuthorizationService
    {
        public  Task<string> GetAccessToken(string clientId, string clientSecret, string correlationId);
    }
}
