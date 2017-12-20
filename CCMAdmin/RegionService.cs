using CCM.Data.SchemaUtils;
using CCM.Data.Web;
using CCMAdmin.Areas.RegionManager.Models.RegionList;
using System.Collections.Generic;

namespace CCMAdmin
{
    public interface IRegionService
    {
        IReadOnlyCollection<Region> GetRegionPrimogenitors();
        IReadOnlyCollection<Region> GetRegionChildren(ParentId parentId);
    }

    public struct ParentId
    {
        #region CONSTRUCTORS
        private ParentId(int id)
        {
            this.Id = id;
        }
        #endregion

        #region PUBLIC PROPERTIES
        [Meta(Name = "parent_id")]
        public int Id { get; }
        #endregion

        #region PUBLIC METHODS
        public static implicit operator ParentId(int id) => new ParentId(id);
        public static implicit operator int(ParentId parentId) => parentId.Id;
        #endregion
    }

    public class RegionService :
        IRegionService
    {
        #region PRIVATE FIELDS
        private readonly ISessionContextProvider _sessionContextProvider;
        #endregion

        #region CONSTRUCTORS
        public RegionService(
            ISessionContextProvider sessionContextProvider)
        {
            this._sessionContextProvider = sessionContextProvider;
        }
        #endregion

        #region PUBLIC METHODS
        public IReadOnlyCollection<Region> GetRegionPrimogenitors()
        {
            var regions = new List<Region>();

            this._sessionContextProvider
                .GetSessionContext()
                .ExecFunctionCollection("get_region_primogenitors", regions);

            return regions;
        }
        public IReadOnlyCollection<Region> GetRegionChildren(ParentId parentId)
        {
            var regions = new List<Region>();

            this._sessionContextProvider
                .GetSessionContext()
                .ExecFunctionCollection("get_region_children", parentId, regions);

            return regions;
        }
        #endregion
    }
}
