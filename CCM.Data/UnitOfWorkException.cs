using System;

namespace CCM.Data
{
    public class UnitOfWorkException :
        Exception
    {
        #region CONSTRUCTORS
        public UnitOfWorkException(string message, Exception ex) :
            base(message, ex)
        { }
        #endregion
    }
}
