using CI.TMS.Claim.API.Persistence;
using myTree.MicroService.Helper;

namespace CI.TMS.Claim.API.Services
{
    public class BaseService
    {
        public readonly ClaimContext context;
        public string userId = string.Empty;
        private IHttpContextAccessor httpContextAccessor;
        public readonly ILogger<BaseService> log;

        #region Constructor
        public BaseService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<BaseService> log)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.log = log;
            userId = (this.httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "name")?.Value ?? "").ToString();
        }

        public void ErrorServiceHandler(Exception ex)
        {
            log.LogError(ex.Detail());
        }
        #endregion
    }
}
