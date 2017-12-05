using Microsoft.AspNetCore.Http;

namespace CCMAdmin
{
    public interface IUnitOfWorkProvider
    {
        UnitOfWork GetUnit();
    }

    public class UnitOfWorkProvider :
        IUnitOfWorkProvider
    {
        #region CONSTRUCTORS
        public UnitOfWorkProvider(
            IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region PRIVATE PROPERTIES
        private IHttpContextAccessor HttpContextAccessor { get; }
        #endregion

        #region PUBLIC METHODS
        public UnitOfWork GetUnit()
        {
            var uow = this.HttpContextAccessor
                .HttpContext
                .Items[typeof(UnitOfWork)];

            return uow as UnitOfWork;
        }
        #endregion
    }

}
