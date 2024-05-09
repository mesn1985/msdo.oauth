using IdentityServer4.Configuration;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace msdo.oauth.identityServer.Services
{
    public class TokenLogService : DefaultTokenService
    {
        public TokenLogService(
            IClaimsService claimsProvider, 
            IReferenceTokenStore referenceTokenStore, 
            ITokenCreationService creationService, 
            IHttpContextAccessor contextAccessor, 
            ISystemClock clock, 
            IKeyMaterialService keyMaterialService, 
            IdentityServerOptions options, 
            ILogger<DefaultTokenService> logger) 
            : base(claimsProvider, referenceTokenStore, creationService, contextAccessor, clock, keyMaterialService, options, logger)
        {
        }
    }
}
