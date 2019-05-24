using Newtonsoft.Json;

namespace CcmClient.Models
{
    public abstract class JsonResultModel
    {
        #region PUBLIC PROPERTIES
        [JsonProperty("isSuccess")]
        public abstract bool IsSuccess { get; }
        #endregion
    }
}
