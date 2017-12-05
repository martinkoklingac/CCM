using CCMAdmin.Areas.RegionManager.Models.RegionList;
using Npgsql;
using System.Collections.Generic;
using System.Data;

namespace CCMAdmin
{
    public interface IRegionService
    {
        IReadOnlyCollection<Region> GetRegionPrimogenitors();
        IReadOnlyCollection<Region> GetRegionChildren(int parentId);
    }

    public class RegionService :
        IRegionService
    {
        #region CONSTRUCTORS
        public RegionService(
            IUnitOfWorkProvider unitOfWorkProvider)
        {
            this.UnitOfWorkProvider = unitOfWorkProvider;
        }
        #endregion

        #region PRIVATE PROPERTIES
        private IUnitOfWorkProvider UnitOfWorkProvider { get; }
        #endregion

        #region PUBLIC METHODS
        public IReadOnlyCollection<Region> GetRegionPrimogenitors()
        {
            var uow = this.UnitOfWorkProvider.GetUnit();

            var regions = new List<Region>();

            using (var conn = new NpgsqlConnection("Host=localhost;Username=CCMAdmin;Password=password;Database=CCM").Init())
            using (var comm = conn.FunctionCommand("get_region_primogenitors"))
            using (var reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    var region = new Region();
                    region.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    region.Name = reader.GetString(reader.GetOrdinal("name"));

                    regions.Add(region);
                }
            }

            return regions;
        }
        public IReadOnlyCollection<Region> GetRegionChildren(int parentId)
        {
            var regions = new List<Region>();

            var paramParentId = new NpgsqlParameter("parent_id", DbType.Int32);
            paramParentId.Value = parentId;

            using (var conn = new NpgsqlConnection("Host=localhost;Username=CCMAdmin;Password=password;Database=CCM").Init())
            using (var comm = conn.FunctionCommand("get_region_children", paramParentId))
            using (var reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    var region = new Region();
                    region.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    region.Name = reader.GetString(reader.GetOrdinal("name"));

                    regions.Add(region);
                }
            }

            return regions;
        }
        #endregion
    }
}
