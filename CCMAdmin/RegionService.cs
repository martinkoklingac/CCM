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
        Region InsertRegion(NewRegion region);
        IReadOnlyCollection<Region> DeleteRegion(DeleteRegion region);
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

    public struct DeleteRegion
    {
        #region CONSTRUCTORS
        public DeleteRegion(int id, bool deleteChildren)
        {
            this.Id = id;
            this.DeleteChildren = deleteChildren;
        }
        #endregion

        #region PUBLIC PROPERTIES
        [Meta(Name = "id")]
        public int Id { get; }
        [Meta(Name = "delete_children")]
        public bool DeleteChildren { get; }
        #endregion
    }

    public struct NewRegion
    {
        #region CONSTRUCTORS
        public NewRegion(string name, int? parentId)
        {
            this.Name = name;
            this.ParentId = parentId;
        }
        #endregion

        #region PUBLIC PROPERTIES
        [Meta(Name = "name")]
        public string Name { get; }
        [Meta(Name = "parent_id")]
        public int? ParentId { get; }
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
            this._sessionContextProvider
                .GetSessionContext()
                .ExecFunctionCollection("get_region_primogenitors", out IReadOnlyCollection<Region> result);

            return result;
        }
        public IReadOnlyCollection<Region> GetRegionChildren(ParentId parentId)
        {
            this._sessionContextProvider
                .GetSessionContext()
                .ExecFunctionCollection("get_region_children", out IReadOnlyCollection<Region> result, parentId);

            return result;
        }

        public Region InsertRegion(NewRegion region)
        {
            this._sessionContextProvider
                .GetSessionContext()
                .ExecFunctionSingle("insert_region", out Region result, region);

            return result;
        }

        public IReadOnlyCollection<Region> DeleteRegion(DeleteRegion region)
        {
            this._sessionContextProvider
                .GetSessionContext()
                .ExecFunctionCollection("delete_region", out IReadOnlyCollection<Region> result, region);

            return result;
        }
        #endregion
    }
}
