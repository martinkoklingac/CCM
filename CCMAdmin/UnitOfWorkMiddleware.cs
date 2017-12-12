using CCM.Data;
using CCM.Data.Web;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CCMAdmin
{
    public class UnitOfWorkMiddleware
    {
        #region PRIVATE FIELDS
        private readonly RequestDelegate _nextRequestDelegate;
        private readonly IUnitOfWorkProvider _unitOfWorkProvider;
        #endregion

        #region CONSTRUCTORS
        public UnitOfWorkMiddleware(
            RequestDelegate requestDelegate,
            IUnitOfWorkProvider unitOfWorkProvider)
        {
            this._nextRequestDelegate = requestDelegate;
            this._unitOfWorkProvider = unitOfWorkProvider;
        }
        #endregion

        #region PUBLIC METHODS
        public async Task Invoke(HttpContext httpContext)
        {
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
                    await this._nextRequestDelegate.Invoke(httpContext);

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
        }
        #endregion
    }
}
