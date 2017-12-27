using CCM.Data.Web.Filters;
using CCMAdmin.Areas.RegionManager.Models.RegionList;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CCMAdmin.Areas.RegionManager.Controllers
{
    [Area("RegionManager")]
    [Route("region-manager/region-list")]
    [UowTransactionFactory]
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
        [Route("add-root")]
        public JsonResult AddRoot(
            [FromBody] RegionRequest request)
        {
            if(this.ModelState.IsValid)
            {
                var region = this.RegionService
                    .InsertRegion(new NewRegion(request.Name, null));

                return Json(new { success = true, data = region });
            }

            return Json(new { success = false, name = $"Error: [{ModelState["Name"].Errors.FirstOrDefault()?.ErrorMessage}]" });
        }

        [HttpPut]
        [Route("add-child")]
        public JsonResult AddChild(
            [FromBody] ChildRegionRequest request)
        {
            if (this.ModelState.IsValid)
            {
                var region = this.RegionService
                    .InsertRegion(new NewRegion(request.Name, request.ParentId));


                return Json(new { success = true, data = region });
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
                var regions = this.RegionService
                    .DeleteRegion(new DeleteRegion(request.Id, request.DeleteChildren));

                return Json(new { success = true, data = regions });
            }

            return Json(new { success = false });
        }
    }

    public class ChildRegionRequest :
        RegionRequest
    {
        [Range(1, int.MaxValue)]
        public int ParentId { get; set; }
    }

    public class RegionRequest
    {
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