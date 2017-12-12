using Microsoft.AspNetCore.Http;
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
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkProvider _unitOfWorkProvider;

            public UowTransactionFilter(
                ILogger<UowTransactionFilter> logger,
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkProvider unitOfWorkProvider)
            {
                _logger = logger;
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkProvider = unitOfWorkProvider;
            }

            public async Task OnActionExecutionAsync(
                ActionExecutingContext context, 
                ActionExecutionDelegate next)
            {
                this._logger.LogTrace("before action");
                
                var httpContext = this._httpContextAccessor.HttpContext;

                //Construct UnitOfWork for this request
                using (var unitOfWork = this._unitOfWorkProvider.CreateUnit())
                {
                    //Bind new UnitOfWork with current request
                    httpContext.Items[typeof(UnitOfWork)] = unitOfWork;

                    //If UoW cannot even start, this request will be short circuited right away
                    unitOfWork.Begin();

                    try
                    {
                        //Attempt to execute request that uses unitOfWork resources
                        var resultContext = await next();

                        //Attempt to commit any pending transaction
                        //If this breaks then we will move to catch block that will attempt to roll back
                        unitOfWork.Commit();
                    }
                    catch
                    {
                        //Attempt to roll back
                        unitOfWork.Rollback();
                        throw;  //Throw the originating exception
                    }
                }//End using

                this._logger.LogTrace("after action");
            }
        }
        #endregion
    }
}
