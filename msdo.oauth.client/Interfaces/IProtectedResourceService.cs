namespace msdo.oauth.client.Interfaces
{
    public interface IProtectedResourceService
    {
        public Task<string> GetProtectedResource(string accessToken, string correlationId);
    }
}
