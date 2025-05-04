using Azure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Provider.IProvider;
using TaskManagementSystem.Provider.Provider;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    public class TaskController : BaseController
    {
        ITaskProvider _taskprovider;
        IWebHostEnvironment  _webHostEnvironment;
        public TaskController(ISessionManager sessionManager, ICommonProvider commonProvider, ITaskProvider taskprovider, IWebHostEnvironment webHostEnvironment) : base(sessionManager, commonProvider)
        {
            _taskprovider = taskprovider;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            TempData["title"] = "Task";
            TaskViewModel taskViewModel = new TaskViewModel();
            return View(taskViewModel);
        }
        [HttpPost]
        public JsonResult GetList()
        {
            return Json(_taskprovider.HiteshTaskList(GetPagingRequestModel()));
        }

        [HttpGet]
        public PartialViewResult _ShowUser(string id)
        {
            TaskViewModel model = new TaskViewModel();
            var data = _commonProvider.UnProtect(id);
            model.HiteshTaskAssignTaskModel.Id = data;
            model.HiteshTaskAssignTaskModel.Users = _taskprovider.GetUserMasterList(data);
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult _AddTask(string id)
        {
            TaskViewModel model = new TaskViewModel();
            model.ProjectList = ProjectList();
            
            if (!id.IsNullOrEmpty())
            {
               model.HiteshTaskAssignTaskModel = _taskprovider.GetTaskById(_commonProvider.UnProtect(id));

            }
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult SaveTask(TaskViewModel model)
        {

            List<string> savedFilePaths = new List<string>();
            ResponseModel responseModel = new ResponseModel();
            var session = GetSessionProviderParameters();

            if (model.HiteshTaskAssignTaskModel.Files != null && model.HiteshTaskAssignTaskModel.Files.Count > 0)
            {
                var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Documents");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                foreach (var file in model.HiteshTaskAssignTaskModel.Files)
                {
                    // Optional: Validate file size (uncomment if needed)
                    // if (file.Length > 5 * 1024 * 1024)
                    // {
                    //     return Json(new { success = false, message = "File size exceeds the 5MB limit." });
                    // }

                    string extension = Path.GetExtension(file.FileName).ToLower();
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".xls", ".xlsx" };

                    if (!allowedExtensions.Contains(extension))
                    {
                        return Json(new { success = false, message = "Unsupported file type." });
                    }

                    // Generate unique file name
                    string uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    string filePath = Path.Combine(directoryPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    savedFilePaths.Add(uniqueFileName); // Save only file name or full path
                }

                model.HiteshTaskAssignTaskModel.DocName = savedFilePaths;

                // Send to provider for DB insert (adjust this if needed)
              
            }
            responseModel = _taskprovider.SaveTask(model.HiteshTaskAssignTaskModel, session);
            return Json(new
            {

                success = responseModel.IsSuccess,
                message = responseModel.IsSuccess ? "Data Insert successfully!" : "Data Not Inserted.",
                paths = savedFilePaths
            });
        }

        [HttpPost]
        public JsonResult GetUSer(int PId)
        {
            return Json(_commonProvider.GetSelectedUser(PId));
        }
        [HttpPost]
        public JsonResult DeleteTask(string id)
        {
            ResponseModel user = new ResponseModel();
            user = _taskprovider.DeleteTask(_commonProvider.UnProtect(id));

            if (user.IsSuccess)
            {
                user.IsSuccess = true;
                user.Message = "Delete successfull";
            }

            return Json(user);

        }




        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
               
                return Json(new { success = true, fileName = file.FileName });
            }

            return Json(new { success = false, message = "No file selected." });
        }

       
    }
}
