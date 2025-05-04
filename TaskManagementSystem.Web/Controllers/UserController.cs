using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Provider.IProvider;
using TaskManagementSystem.Provider.Provider;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    public class UserController : BaseController
    {
        IUserProvider _userProvider;
        public UserController(ISessionManager sessionManager, ICommonProvider commonProvider, IUserProvider userProvider) : base(sessionManager, commonProvider)
        {
            _userProvider = userProvider;
        }

        [HttpGet]
        public IActionResult Index()
        {
            TempData["title"] = "Project";
            TaskViewModel taskViewModel = new TaskViewModel();
            return View(taskViewModel);
        }
        [HttpPost]
        public JsonResult GetList()
        {
            return Json(_userProvider.HiteshTaskProjectList(GetPagingRequestModel(),GetSessionProviderParameters()));
        }

        [HttpGet]
        public IActionResult Task()
        {
            TempData["title"] = "Task";
            TaskViewModel taskViewModel = new TaskViewModel();
            return View(taskViewModel);
        }

        [HttpPost]
        public JsonResult GetTaskList()
        {
            return Json(_userProvider.HiteshTaskList(GetPagingRequestModel(),GetSessionProviderParameters()));
        }


        [HttpGet]
        public IActionResult Calender()
        {
            TempData["title"] = "Calender";
            TaskViewModel taskViewModel = new TaskViewModel();
            return View(taskViewModel);
        }

        [HttpPost]
        public JsonResult GetToDoCount(int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            TaskViewModel taskViewModel = new TaskViewModel();
            taskViewModel.HiteshAssignTaskModelList=_userProvider.GetToDoCount(startDate, endDate,GetSessionProviderParameters());
             
            return Json(taskViewModel.HiteshAssignTaskModelList);
        }



        [HttpGet]
        public PartialViewResult _GetTaskByDate(string date)
        {
            TaskViewModel model = new TaskViewModel();
            if (date !=null)
            {

                model.HiteshTaskAssignTaskModel.SlotDate = date;
                model.HiteshAssignTaskModelList = _userProvider.GetDateData(date,GetSessionProviderParameters());
                model.HiteshTaskAssignTaskModel.roleid = _sessionManager.RoleId;

            }

            return PartialView(model);
        }


        [HttpPost]
        public JsonResult StatusChange(string Encid, int id)
        {

            ResponseModel responseModel = new ResponseModel();
            HiteshTaskAssignTaskModel model = new HiteshTaskAssignTaskModel();
            int UserId = _commonProvider.UnProtect(Encid);

            var data = _userProvider.StatusChange(UserId, id,GetSessionProviderParameters());
            return Json(new { isSuccess = true, status = id });




        }





        [HttpGet]
        public PartialViewResult _ShowMessage(int id)
        {
            TaskViewModel model = new TaskViewModel();
            if (id>0)
            {
                
                model.HIteshTaskMessageModel.SendId = _sessionManager.UserId;
                model.HIteshTaskMessageModel.ReceiveId = id;
                model.HiteshTaskUserMasterModel = _userProvider.GetUserById(id);
               
                
                model.HIteshTaskMessageModel.AllMessage=_userProvider.GetMessage(_sessionManager.UserId,id);
                
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult MsgSave(TaskViewModel model)
        {
            HIteshTaskMessageModel user = new HIteshTaskMessageModel();

            user = _userProvider.SaveMessage(model.HIteshTaskMessageModel);

            if (user.IsSuccess)
            {
                
                user.IsSuccess = true;
                user.Message = "save successfull";
            }

            return Json(new
            {
                isSuccess = user.IsSuccess,
                recId = user.ReceiveId,
               
                message = "save successful"
            });

        }


    }
}
