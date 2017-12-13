using System;

namespace CCM.Data.SchemaUtils
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, 
        AllowMultiple = false, 
        Inherited = false)]
    public class MetaAttribute :
        Attribute
    {
        #region PUBLIC PROPERTIES
        public string Name { get; set; }
        #endregion
    }
}
