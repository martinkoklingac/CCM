using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CCM.Data.Web
{
    public interface ITransactionContextProvider
    {
        IUowTransactionContext CreateTransactionContext();
        IUowTransactionContext GetTransactionContext();
    }

    public interface ISessionContextProvider
    {
        IUowSessionContext GetSessionContext();
    }


    public class UnitOfWorkProvider :
        ITransactionContextProvider,
        ISessionContextProvider
    {
        #region PRIVATE FIELDS
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWorkConfig _unitOfWorkConfig;
        private readonly ILoggerFactory _loggerFactory;
        #endregion

        #region CONSTRUCTORS
        public UnitOfWorkProvider(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWorkConfig unitOfWorkConfig,
            ILoggerFactory loggerFactory)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._unitOfWorkConfig = unitOfWorkConfig;
            this._loggerFactory = loggerFactory;
        }
        #endregion

        #region PUBLIC METHODS
        public IUowTransactionContext CreateTransactionContext()
        {
            var logger = this._loggerFactory
                .CreateLogger<UnitOfWork>();

            var uow = new UnitOfWork(
                this._httpContextAccessor.HttpContext.TraceIdentifier, 
                this._unitOfWorkConfig,
                logger);

            this._httpContextAccessor
                .HttpContext
                .Items[typeof(UnitOfWork)] = uow;

            return uow;
        }

        public IUowTransactionContext GetTransactionContext()
        {
            var uow = this._httpContextAccessor
                .HttpContext
                .Items[typeof(UnitOfWork)];

            return uow as IUowTransactionContext;
        }

        public IUowSessionContext GetSessionContext()
        {
            var uow = this._httpContextAccessor
                .HttpContext
                .Items[typeof(UnitOfWork)];

            return uow as IUowSessionContext;
        }
        #endregion
    }
}
