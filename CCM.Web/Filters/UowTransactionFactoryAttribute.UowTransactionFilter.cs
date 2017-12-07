using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CCM.Data.Web.Filters
{
    public partial class UowTransactionFactoryAttribute
    {
        #region INTERNAL TYPES
        private class UowTransactionFilter :
            IAsyncActionFilter
        {
            private readonly ILogger<UowTransactionFilter> _logger;
            public UowTransactionFilter(ILogger<UowTransactionFilter> logger)
            {
                _logger = logger;
            }

            public async Task OnActionExecutionAsync(
                ActionExecutingContext context, 
                ActionExecutionDelegate next)
            {
                this._logger.LogTrace("before action");
                var resultContext = await next();
                this._logger.LogTrace("after action");
            }
        }
        #endregion
    }
}
