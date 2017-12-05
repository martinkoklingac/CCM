using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CCMAdmin
{
    public class UnitOfWorkMiddleware
    {
        #region PRIVATE FIELDS
        private readonly RequestDelegate _nextRequestDelegate;
        #endregion

        #region CONSTRUCTORS
        public UnitOfWorkMiddleware(
            RequestDelegate requestDelegate)
        {
            _nextRequestDelegate = requestDelegate;
        }
        #endregion

        #region PUBLIC METHODS
        public async Task Invoke(HttpContext httpContext)
        {
            //Bind new UnitOfWork with current request
            httpContext.Items[typeof(UnitOfWork)] = new UnitOfWork(httpContext.TraceIdentifier);
            await this._nextRequestDelegate.Invoke(httpContext);
        }
        #endregion
    }
}
