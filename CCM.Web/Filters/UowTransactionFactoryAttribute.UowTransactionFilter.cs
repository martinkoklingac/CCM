using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CCM.Data.Web.Filters
{
    public partial class UowTransactionFactoryAttribute
    {
        #region INTERNAL TYPES
        private class UowTransactionFilter :
            IAsyncResourceFilter
        {
            #region PRIVATE FIELDS
            private readonly ILogger<UowTransactionFilter> _logger;
            private readonly ITransactionContextProvider _unitOfWorkProvider;
            #endregion

            #region CONSTRUCTORS
            public UowTransactionFilter(
                ILogger<UowTransactionFilter> logger,
                ITransactionContextProvider unitOfWorkProvider)
            {
                _logger = logger;
                _unitOfWorkProvider = unitOfWorkProvider;
            }
            #endregion

            #region PUBLIC METHODS
            public async Task OnResourceExecutionAsync(
                ResourceExecutingContext context,
                ResourceExecutionDelegate next)
            {
                this._logger.LogTrace($"[{context.ActionDescriptor.DisplayName}] -> Starting resource execution");
                
                using (var work = this._unitOfWorkProvider.CreateTransactionContext())
                {
                    try
                    {
                        //If UoW cannot even start, this request will be short circuited right away
                        work.Begin();

                        //Attempt to execute request that uses unitOfWork resources
                        var result = await next();

                        if (result.Exception != null)
                        {
                            //Attempt to roll back
                            work.Rollback();
                        }
                        else
                        {
                            //Attempt to commit any pending transaction
                            //If this breaks then we will move to catch block that will attempt to roll back
                            work.Commit();
                        }
                    }
                    catch
                    {
                        //Attempt to roll back
                        work.Rollback();
                        throw;  //Throw the originating exception
                    }
                }//End using

                this._logger.LogTrace($"[{context.ActionDescriptor.DisplayName}] -> Resource execution complete");
            }
            #endregion
        }
        #endregion
    }
}
