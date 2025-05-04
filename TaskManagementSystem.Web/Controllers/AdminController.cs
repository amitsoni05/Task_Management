using System.Drawing;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Provider.IProvider;
using TaskManagementSystem.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Web.Controllers
{
    public class AdminController : BaseController
    {
        IAdminProvider _adminProvider;
        public AdminController(ISessionManager sessionManager, ICommonProvider commonProvider, IAdminProvider adminProvider) : base(sessionManager, commonProvider)
        {
            _adminProvider = adminProvider;
        }

        public IActionResult Index()
        {
            TempData["title"] = "Employee";
            return View();
        }
        [HttpPost]
        public JsonResult GetList()
        {
            return Json(_adminProvider.HiteshTaskUserMasterList(GetPagingRequestModel(), GetSessionProviderParameters()));
        }

        [HttpGet]
        public PartialViewResult _AddEmployee(string id)
        {
            TaskViewModel model = new TaskViewModel();
            if (!id.IsNullOrEmpty())
            {
                model.HiteshTaskUserMasterModel = _adminProvider.GetEmployeeById(_commonProvider.UnProtect(id));
            }

            return PartialView(model);
        }


        [HttpGet]
        public PartialViewResult _UploadExcel()
        {
            TaskViewModel ViewModel=new TaskViewModel();
            // model.HiteshTaskDocumentSaveModel.ProjectId = _commonProvider.UnProtect(id);
            return PartialView(ViewModel);
        }

        [HttpPost]
        public IActionResult SaveEmployee(TaskViewModel model)
        {
            var session = GetSessionProviderParameters();
            ResponseModel responseModel = new ResponseModel();
            responseModel = _adminProvider.SaveEmployee(model.HiteshTaskUserMasterModel, session);
            return Json(responseModel);
        }


        [HttpPost]
        public JsonResult DeleteEmployee(string id)
        {
            ResponseModel user = new ResponseModel();
            user = _adminProvider.DeleteEmployee(_commonProvider.UnProtect(id));

            if (user.IsSuccess)
            {
                user.IsSuccess = true;
                user.Message = "Delete successfull";
            }

            return Json(user);

        }



        [HttpPost]
        public IActionResult UploadExcel(IFormFile excelFile)
        {
               

            var successList = new List<HiteshTaskUserMasterModel>();
            var errorList = new List<HiteshTaskUserMasterModel>();
            TaskViewModel ViewModel = new TaskViewModel();
            HiteshTaskUserMasterModel model=new HiteshTaskUserMasterModel();
            int success = 0;
            int error = 0;
            using (var stream = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                 excelFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    // int rowCount = worksheet.Dimension.Rows;
                    var validRowIndices = new List<int>();
                    int totalRows = worksheet.Dimension.End.Row;
                    int totalColumns = worksheet.Dimension.End.Column;

                    // Identify non-empty rows
                    for (int row = 2; row <= totalRows; row++) // Assuming row 1 is header
                    {
                        bool isRowEmpty = true;

                        for (int col = 1; col <= totalColumns; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Text;

                            if (!string.IsNullOrWhiteSpace(cellValue))
                            {
                                isRowEmpty = false;
                                break;
                            }
                        }

                        if (!isRowEmpty)
                        {
                            validRowIndices.Add(row);
                        }
                    }

                    // Process only non-empty rows
                    foreach (var row in validRowIndices)
                    {
                         model = new HiteshTaskUserMasterModel
                        {
                            FullName = worksheet.Cells[row, 1].Text,
                            Email = worksheet.Cells[row, 2].Text,
                            Password = worksheet.Cells[row, 3].Text
                        };

                        var validationMessages = new List<string>();

                        if (string.IsNullOrWhiteSpace(model.FullName))
                            validationMessages.Add("FullName is required.");

                        if (string.IsNullOrWhiteSpace(model.Email))
                            validationMessages.Add("Email is required.");

                        if (string.IsNullOrWhiteSpace(model.Password))
                            validationMessages.Add("Password is required.");
                        else if (model.Password.Length < 6)
                            validationMessages.Add("Password must be at least 6 characters.");

                        var isEmailExists = _adminProvider.CheckEmail(model.Email);
                        if (isEmailExists.IsSuccess)
                        {
                            validationMessages.Add("Email already exists.");
                        }

                        if (validationMessages.Any())
                        {
                            model.Message = string.Join(" ", validationMessages);
                            errorList.Add(model);
                            error++;
                        }
                        else
                        {
                            success++;
                            _adminProvider.SaveEmployee(model, GetSessionProviderParameters());
                        }
                    }


                }

                // Save error list to Excel
                if (errorList.Any())
                {
                    var errorFilePath = Path.Combine("wwwroot/Documents", "ErrorData.xlsx");
                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("Errors");
                        ws.Cells[1, 1].Value = "Exception Message";
                        ws.Cells[1, 2].Value = "FullName";
                        ws.Cells[1, 3].Value = "Email";
                        ws.Cells[1, 4].Value = "Password";
                        

                        for (int i = 0; i < errorList.Count; i++)
                        {
                            ws.Cells[i + 2, 1].Value = errorList[i].Message ?? "";
                            ws.Cells[i + 2, 2].Value = errorList[i].FullName ?? null;
                            ws.Cells[i + 2, 3].Value = errorList[i].Email;
                            ws.Cells[i + 2, 4].Value = errorList[i].Password;
                           
                        }
                        ws.Cells.AutoFitColumns();
                        package.SaveAs(new FileInfo(errorFilePath));
                    }
                }

                // Send result back to view
                ViewModel.isSuccess = true;
                ViewModel.success = success;
                ViewModel.errorcount = error;
                ViewModel.total = success + error;
                //TempData["Total"] = successList.Count + errorList.Count;
                //TempData["Success"] = successList.Count;
                //TempData["Error"] = errorList.Count;
                //TempData["HasErrorFile"] = errorList.Any();

                

            }

            return Json(ViewModel);
        }
    }
}
