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
    public class ProjectController : BaseController
    {
        IProjectProvider _projectProvider;
        IWebHostEnvironment _webHostEnvironment;
        public ProjectController(ISessionManager sessionManager, ICommonProvider commonProvider, IProjectProvider projectProvider , IWebHostEnvironment webHostEnvironment) : base(sessionManager, commonProvider)
        {
            _projectProvider = projectProvider;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            TempData["title"] = "Project";
            TaskViewModel model = new TaskViewModel();
            return View(model);
        }

        [HttpPost]
        public JsonResult GetList()
        {
            return Json(_projectProvider.HiteshTaskProjectList(GetPagingRequestModel()));
        }

        [HttpGet]
        public PartialViewResult _ShowUser(string id)
        {
            TaskViewModel model = new TaskViewModel();
            var data=_commonProvider.UnProtect(id);
            model.HiteshTaskProjectModel.Users = _projectProvider.GetUserMasterList(data);      
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult _AddProject(string id)
        {
            TaskViewModel model = new TaskViewModel();
            model.HiteshTaskProjectModel.UserMasterList = User();
            if (!id.IsNullOrEmpty())
            {
                               
                model.HiteshTaskProjectModel = _projectProvider.GetProjectById(_commonProvider.UnProtect(id));
                
            }
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult _AddDocument(string id)
        {
            TaskViewModel model = new TaskViewModel();
            model.HiteshTaskDocumentSaveModel.ProjectId = _commonProvider.UnProtect(id);          
            return PartialView(model);
        }



        [HttpGet]
        public PartialViewResult _DowDocument(string id)
        {
            TaskViewModel model = new TaskViewModel();
            model.HiteshTaskDocumentSaveModel.ProjectId= _commonProvider.UnProtect(id);
            model.hiteshTaskDocumentSaveModelList = _projectProvider.GetDocumentList(_commonProvider.UnProtect(id));
            
            
            return PartialView(model);
        }


        [HttpPost]
        public IActionResult SaveProject(TaskViewModel model)
        {
            var session = GetSessionProviderParameters();
            ResponseModel responseModel = new ResponseModel();
            responseModel = _projectProvider.SaveProject(model.HiteshTaskProjectModel, session);
            return Json(responseModel);
        }

        [HttpPost]
        public JsonResult DeleteProject(string id)
        {
            ResponseModel user = new ResponseModel();
            user = _projectProvider.DeleteProject(_commonProvider.UnProtect(id));

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
                List<string> fileList = TempData["UploadedFiles"] as List<string> ?? new List<string>();
                fileList.Add(file.FileName);

                TempData["UploadedFiles"] = fileList;
                TempData.Keep("UploadedFiles"); // Persist TempData across requests

                return Json(new { success = true, fileName = file.FileName });
            }

            return Json(new { success = false, message = "No file selected." });
        }

        [HttpGet]
        public IActionResult UploadDocumentModal()
        {
            ViewBag.UploadedFiles = TempData["UploadedFiles"] as List<string> ?? new List<string>();
            TempData.Keep("UploadedFiles");
            return View();
        }


        [HttpPost]
        public JsonResult DocumentSave(TaskViewModel model)
        {
            List<string> savedFilePaths = new List<string>();
            ResponseModel response = new ResponseModel();

            if (model.HiteshTaskDocumentSaveModel.Files != null && model.HiteshTaskDocumentSaveModel.Files.Count > 0)
            {
                var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Documents");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                foreach (var file in model.HiteshTaskDocumentSaveModel.Files)
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

                model.HiteshTaskDocumentSaveModel.DocName = savedFilePaths;

                // Send to provider for DB insert (adjust this if needed)
                response = _projectProvider.DocumentSave(model.HiteshTaskDocumentSaveModel); // Or HiteshCricketImagesModel, depending on your model
            }

            return Json(new
            {
                success = response.IsSuccess,
                message = response.IsSuccess ? "Files uploaded successfully!" : "Upload failed.",
                paths = savedFilePaths
            });
        }




        [HttpPost]
        public JsonResult DownloadDocData(int id)
        {

            var data = _projectProvider.GetDocId(id);
            if (data == null || string.IsNullOrEmpty(data.DocumentName))
            {

                return Json("");
            }
            return Json(data.DocumentName);
        }

        [HttpGet]
        public IActionResult Download(string documentName)
        {

            string downloadPath = Path.Combine(_webHostEnvironment.WebRootPath, "Documents");
            string newDocFilePath = Path.Combine(downloadPath, documentName);
            if (System.IO.File.Exists(newDocFilePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(newDocFilePath);
                var contentType = "application/octet-stream";
                return File(fileBytes, contentType, documentName);
            }
            else
            {
                //  return RedirectToAction("EventDetails", "Organizor", new {HiteshEventDetailModel.UserId }); // Redirect if file is missing
            }
            return View();
        }


        [HttpPost]
        public IActionResult DownloadAllDocuments(int Pid, List<int> documentIds)
        {
       
            var documents = _projectProvider.GetDocumentList(Pid).Where(doc => documentIds.Contains(doc.Id)).ToList();



            string downloadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Documents");
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
                {
                    foreach (var doc in documents)
                    {
                        string filePath = Path.Combine(downloadFolder, doc.DocumentName);
                        if (System.IO.File.Exists(filePath))
                        {
                            var fileBytes = System.IO.File.ReadAllBytes(filePath);
                            var zipEntry = archive.CreateEntry(doc.DocumentName, System.IO.Compression.CompressionLevel.Fastest);
                            using (var zipStream = zipEntry.Open())
                            {
                                zipStream.Write(fileBytes, 0, fileBytes.Length);
                            }
                        }
                    }
                }

                memoryStream.Position = 0;
                string zipName = $"AllDocuments_{DateTime.Now:yyyyMMddHHmmss}.zip";
                return File(memoryStream.ToArray(), "application/zip", zipName);
            }
        }


        [HttpPost]
        public JsonResult DeleteDocData(int id)
        {
            ResponseModel user = new ResponseModel();
            user = _projectProvider.DeleteDocData(id);

            if (user.IsSuccess)
            {
                user.IsSuccess = true;
                user.Message = "Delete successfull";
            }

            return Json(user);

        }


    }
}
