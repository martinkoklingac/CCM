using CCMAdmin.Areas.RegionManager.Models.RegionList;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CCMAdmin.Areas.RegionManager.Controllers
{
    [Area("RegionManager")]
    [Route("region-manager/region-list")]
    public class RegionListController :
        Controller
    {
        #region CONSTRUCTORS
        public RegionListController(
            IRegionService regionService)
        {
            this.RegionService = regionService;
        }
        #endregion

        #region PRIVATE PROPERTIES
        private IRegionService RegionService { get; }
        #endregion

        [Route("")]
        public IActionResult Index()
        {
            var regions = this.RegionService
                .GetRegionPrimogenitors();
            var model = new RegionListModel(regions);

            return View(model);
        }

        [HttpGet]
        [Route("get-children")]
        public JsonResult GetChildren(int parentId)
        {
            var regions = this.RegionService
                .GetRegionChildren(parentId);

            return Json(regions);
        }

        [HttpPut]
        [Route("add")]
        public JsonResult Add(
            [FromBody] RegionRequest request)
        {
            if (this.ModelState.IsValid)
            {
                var regions = new List<Region>();

                var paramParentId = new NpgsqlParameter("parent_id", DbType.Int32);
                paramParentId.Value = request.ParentId;

                var paramName = new NpgsqlParameter("name", DbType.String);
                paramName.Value = request.Name;

                using (var conn = new NpgsqlConnection("Host=localhost;Username=CCMAdmin;Password=password;Database=CCM").Init())
                using (var comm = conn.FunctionCommand("insert_region", paramParentId, paramName))
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


                return Json(new { success = true, data = regions.First() });
            }

            return Json(new { success = false, name = $"Error: [{ModelState["Name"].Errors.FirstOrDefault()?.ErrorMessage}]" });
        }

        [HttpDelete]
        [Route("delete")]
        public JsonResult Delete(
            [FromBody] RegionDeleteRequest request)
        {
            if (this.ModelState.IsValid)
            {

                var regions = new List<Region>();

                var paramId = new NpgsqlParameter("id", DbType.Int32);
                paramId.Value = request.Id;

                var paramDeleteChildren = new NpgsqlParameter("delete_children", DbType.Boolean);
                paramDeleteChildren.Value = request.DeleteChildren;

                using (var conn = new NpgsqlConnection("Host=localhost;Username=CCMAdmin;Password=password;Database=CCM").Init())
                using (var comm = conn.FunctionCommand("delete_region", paramId, paramDeleteChildren))
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

                return Json(new { success = true, data = regions });
            }

            return Json(new { success = false });
        }
    }

    public class RegionRequest
    {
        [Range(1, int.MaxValue)]
        public int ParentId { get; set; }

        //[JsonProperty(PropertyName = "name")]
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
    }

    public class RegionDeleteRequest
    {
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        public bool DeleteChildren { get; set; }
    }
}