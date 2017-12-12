using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CCMAdmin
{
    public interface IUnitOfWorkProvider
    {
        UnitOfWork CreateUnit();
        UnitOfWork GetUnit();
    }

    public class UnitOfWorkProvider :
        IUnitOfWorkProvider
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
        public UnitOfWork CreateUnit()
        {
            var logger = this._loggerFactory
                .CreateLogger<UnitOfWork>();

            return new UnitOfWork(
                this._httpContextAccessor.HttpContext.TraceIdentifier, 
                this._unitOfWorkConfig,
                logger);
        }

        public UnitOfWork GetUnit()
        {
            var uow = this._httpContextAccessor
                .HttpContext
                .Items[typeof(UnitOfWork)];

            return uow as UnitOfWork;
        }
        #endregion
    }
}
