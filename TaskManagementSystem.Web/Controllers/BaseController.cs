using TaskManagementSystem.Common.CommonEntities;
using TaskManagementSystem.Common.Utility;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TaskManagementSystem.Common.BusinessEntities;

using TaskManagementSystem.Provider.IProvider;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TaskManagementSystem.Web.Controllers
{
    public class BaseController : Controller
    {
        #region Variables
        protected ICommonProvider _commonProvider;
        public ISessionManager _sessionManager;
        #endregion

        #region Constructor

        public BaseController(ISessionManager sessionManager, ICommonProvider commonProvider)
        {
            _sessionManager = sessionManager;
            _commonProvider = commonProvider;
        }

        #endregion

        [NonAction]
        protected SessionProviderModel GetSessionProviderParameters()
        {
            SessionProviderModel sessionProviderModel = new SessionProviderModel
            {
                UserId = _sessionManager.UserId,
                Username = _sessionManager.Username,
                Ip = _sessionManager.GetIP(),
                RoleId = _sessionManager.RoleId,
            };
            return sessionProviderModel;
        }
        [NonAction]
        public DatatablePageRequestModel GetPagingRequestModel()
        {
            DatatablePageRequestModel model = new DatatablePageRequestModel
            {
                StartIndex = AppCommon.ConvertToInt32(HttpContext.Request.Form["start"]),
                PageSize = AppCommon.ConvertToInt32(HttpContext.Request.Form["length"]),
                SearchText = HttpContext.Request.Form["search[value]"],
                SortColumnName = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"] + "][name]"],
                SortDirection = HttpContext.Request.Form["order[0][dir]"],
                Draw = HttpContext.Request.Form["draw"],
            };
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "extra_search"))
                model.ExtraSearch = HttpContext.Request.Form["extra_search"];
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "RoleId") && !string.IsNullOrEmpty(HttpContext.Request.Form["RoleId"]))
                model.RoleId = AppCommon.ConvertToInt32(HttpContext.Request.Form["RoleId"]);
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "status") && !string.IsNullOrEmpty(HttpContext.Request.Form["status"]))
                model.Status = AppCommon.ConvertToInt32(HttpContext.Request.Form["status"]);
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "category") && !string.IsNullOrEmpty(HttpContext.Request.Form["category"]))
                model.Category =HttpContext.Request.Form["category"];
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "courtid") && !string.IsNullOrEmpty(HttpContext.Request.Form["courtid"]))
                model.CourtId = AppCommon.ConvertToInt32(HttpContext.Request.Form["courtid"]);
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "ddlvalue") && !string.IsNullOrEmpty(HttpContext.Request.Form["ddlvalue"]))
                model.DdlStatus = AppCommon.ConvertToInt32(HttpContext.Request.Form["ddlvalue"]);
            return model;
        }
        //public List<HiteshCityModel> HiteshCityLists { get; set; } = new List<HiteshCityModel>();

        protected List<SelectListItem> ProjectList()
        {
            return (from s in _commonProvider.GetAllProject()
                    select new SelectListItem()
                    {
                        Text = s.Text,
                        Value = s.Value,

                    }).OrderBy(x => x.Text).ToList();
        }

        protected List<SelectListItem> UserList(int id)
        {
            return (from s in _commonProvider.GetAllUser(id)
                    select new SelectListItem()
                    {
                        Text = s.Text,
                        Value = s.Value,
                       
                    }).OrderBy(x => x.Text).ToList();
        }

        protected List<SelectListItem> User()
        {
            return (from s in _commonProvider.GetUser()
                    select new SelectListItem()
                    {
                        Text = s.Text,
                        Value = s.Value,

                    }).OrderBy(x => x.Text).ToList();
        }

        //[NonAction]
        //protected List<SelectListItem> HiteshStateList()
        //{
        //    return (from s in _commonProvider.GetAllStates()
        //            select new SelectListItem()
        //            {
        //                Text = s.Text,
        //                Value = s.Value,

        //            }).OrderBy(x => x.Text).ToList();
        //}

        //[NonAction]
        //protected List<SelectListItem> GetCitiesByState(int stateId)
        //{
        //    return (from s in _commonProvider.GetAllCity(stateId)

        //            select new SelectListItem()
        //            {
        //                Text = s.Text,
        //                Value = s.Value,

        //            }).OrderBy(x => x.Text).ToList();
        //}

        
        //[NonAction]
        //protected List<SelectListItem> GetAllSlotTime(int selectedDateID)
        //{
        //    return (from s in _commonProvider.GetAllCity(selectedDateID)

        //            select new SelectListItem()
        //            {
        //                Text = s.Text,
        //                Value = s.Value,

        //            }).OrderBy(x => x.Text).ToList();
        //}


        public void SetDataInTemp(string TempDataKey, string data)
        {
            TempData[TempDataKey] = null;
            TempData[TempDataKey] = data;
            KeepTempData(TempDataKey);
        }
        public string GetDataFromTemp(string TempDataKey)
        {
            string data = "";
            if (TempData[TempDataKey] != null)
            {
                data = TempData[TempDataKey].ToString();
                KeepTempData(TempDataKey);
            }
            return data;
        }
        public void KeepTempData(string TempDataKey)
        {
            if (TempData[TempDataKey] != null)
                TempData.Keep();
        }
        public void DeleteTempData(string TempDataKey)
        {
            TempData[TempDataKey] = null;
        }

      
    }
}
