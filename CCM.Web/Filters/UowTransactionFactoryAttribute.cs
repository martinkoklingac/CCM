using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace CCM.Data.Web.Filters
{
    public partial class UowTransactionFactoryAttribute :
        Attribute,
        IFilterFactory
    {
        #region CONSTRUCTORS
        public UowTransactionFactoryAttribute() :
            base()
        { }
        #endregion

        #region PUBLIC PROPERTIES
        public bool IsReusable => false;
        #endregion

        #region PUBLIC METHODS
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var logger = (ILogger<UowTransactionFilter>)serviceProvider.GetService(typeof(ILogger<UowTransactionFilter>));
            var unitOfWorkProvider = (IUnitOfWorkProvider)serviceProvider.GetService(typeof(IUnitOfWorkProvider));

            return new UowTransactionFilter(logger, unitOfWorkProvider);
        }
        #endregion
    }
}
