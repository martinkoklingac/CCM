using CCM.Data.SchemaUtils;
using Newtonsoft.Json;

namespace CCMAdmin.Areas.RegionManager.Models.RegionList
{
    public class Region
    {
        [Meta(Name = "id")]
        [JsonProperty("id")]
        public int Id { get; set; }
        [Meta(Name = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
