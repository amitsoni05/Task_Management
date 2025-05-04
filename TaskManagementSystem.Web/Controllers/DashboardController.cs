using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Provider.IProvider;
using TaskManagementSystem.Provider.Provider;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(ISessionManager sessionManager, ICommonProvider commonProvider) : base(sessionManager, commonProvider)
        {
            _commonProvider = commonProvider;
        }

        public IActionResult Index()
        {
            TaskViewModel model = new TaskViewModel();
            var session=GetSessionProviderParameters();
            if (session.RoleId == 1)
            {
                TempData["Name"] = session.Username;
                model.HiteshTaskUserMasterModel.Id = session.UserId;
                model.HiteshTaskUserMasterModel.RoleId = session.RoleId;
                return RedirectToAction("SHowChart", "Dashboard");
            }
            else if (session.RoleId == 2)
            {
                model.HiteshTaskUserMasterModel.FullName = session.Username;
                model.HiteshTaskUserMasterModel.Id = session.UserId;
                model.HiteshTaskUserMasterModel.RoleId = session.RoleId;
                return RedirectToAction("SHowChart", "Dashboard");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
          
        }

        public IActionResult SHowChart()
        {
            TempData["title"] = "Chart";
            TaskViewModel model=new TaskViewModel();
            model.HiteshTaskProjectModel.UserMasterList = User();
            model.HiteshTaskUserMasterModel.Id = _sessionManager.UserId;
            model.RoleId = _sessionManager.RoleId;
            return View(model);
        }

        [HttpGet]
        public IActionResult GetChartData(DateTime? startDate, DateTime? endDate, int? userId)
        {
            TaskViewModel model = new TaskViewModel();

            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            DateTime actualStartDate = startDate ?? firstDayOfMonth;
            DateTime actualEndDate = endDate ?? lastDayOfMonth;



            model.StatusChartLabels= new[] { "Pending", "In Progress", "Completed","Unavailable" };
            model.StatusChartData = _commonProvider.GetStatusData(actualStartDate, actualEndDate, userId); // return int[]

            model.PriorityChartLabels= new[] { "Low", "Medium", "High" };
            model.PriorityChartData = _commonProvider.GetPriorityData(actualStartDate, actualEndDate, userId); // return int[]

            return Json(new
            {
                statusLabels = model.StatusChartLabels,
                statusData = model.StatusChartData,
                priorityLabels = model.PriorityChartLabels,
                priorityData = model.PriorityChartData
            });
        }


    }
}
