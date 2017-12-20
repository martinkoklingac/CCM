using CCM.Data.SchemaUtils;

namespace CCMAdmin.Areas.RegionManager.Models.RegionList
{
    public class Region
    {
        [Meta(Name = "id")]
        public int Id { get; set; }
        [Meta(Name = "name")]
        public string Name { get; set; }
    }
}
