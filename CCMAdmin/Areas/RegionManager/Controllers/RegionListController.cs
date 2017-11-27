using CCMAdmin.Areas.RegionManager.Models.RegionList;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        [HttpPut]
        [Route("add")]
        public JsonResult Add([FromBody] RegionRequest request)
        {
            if (this.ModelState.IsValid)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, name = $"Error: [{ModelState["Name"].Errors.FirstOrDefault()?.ErrorMessage}]" });
        }
    }

    public class RegionRequest
    {
        //[JsonProperty(PropertyName = "name")]
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
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