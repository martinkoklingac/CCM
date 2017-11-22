using CCMAdmin.Areas.RegionManager.Models.RegionList;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;

namespace CCMAdmin.Areas.RegionManager.Controllers
{
    [Area("RegionManager")]
    [Route("region-manager/region-list")]
    public class RegionListController :
        Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            var regions = new List<Region>();
            var model = new RegionListModel(regions);

            using (var conn = new NpgsqlConnection("Host=localhost;Username=CCMAdmin;Password=password;Database=CCM").Init())
            using (var comm = new NpgsqlCommand(@"select ""Id"", ""Name"" from public.""Regions""", conn))
            using (var reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    var region = new Region();
                    region.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    region.Name = reader.GetString(reader.GetOrdinal("Name"));

                    regions.Add(region);
                }
            }

            return View(model);
        }
    }

    public static class NpgsqlExtensions
    {
        public static NpgsqlConnection Init(this NpgsqlConnection connection)
        {
            connection.Open();
            return connection;
        }
    }
}