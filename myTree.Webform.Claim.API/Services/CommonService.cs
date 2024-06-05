using CI.TMS.Claim.API.Persistence;
using Microsoft.AspNetCore.Authentication;
using myTree.MicroService.Helper;

namespace CI.TMS.Claim.API.Services
{
    public class CommonService
    {
        private IConfiguration config;
        private ILogger<CommonService> log;
        public CommonService(IConfiguration config, ILogger<CommonService> log)
        {
            this.config = config;
            this.log = log;
        }

        public async Task<string> GetAccessToken(HttpContext ctx)
        {
            try
            {
                var accessToken = await ctx.GetTokenAsync("access_token");
                return accessToken ?? "";
            }
            catch (Exception e)
            {
                log.LogError(e.Detail());
                throw;
            }
        }
    }
}
