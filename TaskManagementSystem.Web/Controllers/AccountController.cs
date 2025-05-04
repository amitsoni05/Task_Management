using Microsoft.AspNetCore.DataProtection;
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
    public class AccountController : BaseController
    {
        IDataProtector _dataprotector;
        IAccountProvider _accountProvider;
        IUserProvider _userProvider;
        IWebHostEnvironment _webHostEnvironment;
        public AccountController(ISessionManager sessionManager, ICommonProvider commonProvider,IAccountProvider accountProvider, IDataProtectionProvider dataProtection,IUserProvider userProvider,IWebHostEnvironment webHostEnvironment) : base(sessionManager, commonProvider)
        {
            _accountProvider = accountProvider;
            _dataprotector = dataProtection.CreateProtector("TaskManagementSystem");
            _commonProvider = commonProvider;
            _userProvider = userProvider;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Login()
        {
            CaptchaResult captcha = Captcha.Generate(CaptchaType.Simple);
            _sessionManager.CaptchaCode = captcha.CatpchaCode;
            TaskViewModel taskViewModel = new TaskViewModel();
            taskViewModel.HiteshTaskUserMasterModel.CaptchaImage = captcha.CaptchaBase64;
            return View(taskViewModel);
        }

        public JsonResult RefreshCaptcha()
        {
            CaptchaResult captcha = Captcha.Generate(CaptchaType.Simple);
           _sessionManager.CaptchaCode = captcha.CatpchaCode;
            return Json(captcha.CaptchaBase64);
        }

        [HttpPost]
        public JsonResult CheckLogin(TaskViewModel model)
        {
            HiteshTaskUserMasterModel res = new HiteshTaskUserMasterModel();
            if (_sessionManager.CaptchaCode == model.HiteshTaskUserMasterModel.CaptchaCode)
            {
                res = _accountProvider.CheckLogin(model.HiteshTaskUserMasterModel);
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "invalide captcha";
            }
            if (res.IsSuccess)
            {
                _sessionManager.UserId = res.Id;
                _sessionManager.RoleId = res.RoleId;
                _sessionManager.Username = res.FullName;
                ViewBag.Roleid = res.RoleId;
                res.IsSuccess = true;
                res.Message = "Login Successfully";

            }
            else
            {
                res.Message = "Please Try Again";
            }
            CaptchaResult captcha = Captcha.Generate(CaptchaType.Simple);
            _sessionManager.CaptchaCode = captcha.CatpchaCode;
            model.HiteshTaskUserMasterModel.CaptchaImage = captcha.CaptchaBase64;
            return Json(res);
        }


        public IActionResult Logout()
        {           
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login");


        }


        [HttpGet]
        public PartialViewResult _ChangePassword(int id)
        {
            TaskViewModel model = new TaskViewModel();
            model.HiteshTaskUserMasterModel.Id = id;
            return PartialView(model);
        }


        [HttpPost]
        public JsonResult CheckPassword(TaskViewModel model)
        {
            HiteshTaskUserMasterModel res = new HiteshTaskUserMasterModel();
           res=_accountProvider.CheckPassword(model.HiteshTaskUserMasterModel);


            if (res.IsSuccess)
            {
                           
                res.IsSuccess = true;
                res.Message = "Password Change Successfully";
                res.RoleId=_sessionManager.RoleId;
            }
          
            
            return Json(res);
        }



        [HttpGet]
        public PartialViewResult _profile(int id)
        {
            TaskViewModel model = new TaskViewModel();
            model.HiteshTaskUserMasterModel= _userProvider.GetUserById(id);
            model.HiteshTaskUserMasterModel.RoleId = _sessionManager.RoleId;
            return PartialView(model);
        }


        [HttpPost]
        public IActionResult Profile(TaskViewModel model)
        {        
                string fileName = "";
                if (model.HiteshTaskUserMasterModel.Photo != null)
                {
                    if (model.HiteshTaskUserMasterModel.Photo.Length > 1 * 1024 * 1024)
                    {
                        TempData["imageop"] = "File size exceeds the limit of 1 MB.";
                        //    model.Result = false;
                        //return Json(returnresult);
                    }
                    fileName = Path.GetFileNameWithoutExtension(model.HiteshTaskUserMasterModel.Photo.FileName);
                    FileInfo fi = new FileInfo(Path.GetFileName(model.HiteshTaskUserMasterModel.Photo.FileName));
                    if (fi.Extension.ToLower() == ".jpg" || fi.Extension.ToLower() == ".png" || fi.Extension.ToLower() == ".jpeg")
                    {
                        fileName = fileName + "_" + Guid.NewGuid().ToString() + fi.Extension.ToLower();
                        var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        var filePath = Path.Combine(directoryPath, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.HiteshTaskUserMasterModel.Photo.CopyTo(fileStream);
                        }
                    }
                    else
                    {
                        TempData["imageop"] = "Invalid Image File";
                        //returnresult.Result = false;
                        //return Json(returnresult);
                    }
                }
                else
                {
                    TempData["imageop"] = "ImageNotFound";
                    //returnresult.Result = false;
                    //return Json(returnresult);
                }
                model.HiteshTaskUserMasterModel.ImagesUrl = fileName;               

              ResponseModel res = new ResponseModel();
             res = _accountProvider.UpdateProfile(model.HiteshTaskUserMasterModel);
            if (res.IsSuccess)
            {

                res.IsSuccess = true;
                res.Message = "Data Change Successfully";
                res.Rid = _sessionManager.RoleId;
             
            }


            return Json(res);
        }

        [HttpGet]
        public IActionResult ResetPassword(string Id)
        {           
            TaskViewModel taskViewModel = new TaskViewModel();
            if (!string.IsNullOrEmpty(Id))
            {
                string parm = _commonProvider.UnProtectString(Id);
                if (parm == "2")
                    TempData["error"] = "The link was expired, please re-generate new link!";
            }
            return View(taskViewModel);
        }


        [HttpPost]
        public JsonResult Checkemail(TaskViewModel model)
        {
            ResponseModel res = new ResponseModel();
            res = _accountProvider.CheckEmail(model.HiteshTaskUserMasterModel);

            return Json(res);
        }


        [HttpGet]
        public IActionResult Reset(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {

                string parm = "";
                string[] parms;
                try
                {

                    parm = _commonProvider.UnProtectString(id);
                    parms = parm.Split('|');


                    

                    DateTime dt = new DateTime(Convert.ToInt64(parms[1]));
                    dt = dt.AddMinutes(30);
                    if (DateTime.Now > dt)
                        return RedirectToAction("ResetPassword", "Account", new { @id = _dataprotector.Protect("2") });
                }
                catch (Exception)
                {
                    return RedirectToAction("Login", "Account");
                }

                TaskViewModel model = new TaskViewModel();
           
                var user = _userProvider.GetUserById(Convert.ToInt32(parms[0]));
                if (user != null)
                {
                 
                    model.HiteshTaskUserMasterModel.Email = user.Email;
                    model.HiteshTaskUserMasterModel.Id = user.Id;
                }
                else
                    return RedirectToAction("Login", "Account");

                return View(model);
            }
            else
                return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        public IActionResult ResetPassword(TaskViewModel model)
        {

            ResponseModel returnResult = new ResponseModel();

            HiteshTaskUserMasterModel userModel = new HiteshTaskUserMasterModel
            {
                Id = model.HiteshTaskUserMasterModel.Id,
                newpassword =model.HiteshTaskUserMasterModel.newpassword,
                confirmpassword = model.HiteshTaskUserMasterModel.confirmpassword,
               
            };
            returnResult = _accountProvider.ChangeOrResetPassword(userModel);

            if (returnResult.IsSuccess)
            {
                returnResult.Message = "Password Change Successfully !";
              
            }
            else
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "Password Change Successfully !";
            }

            return Json(returnResult);


        }


    }
}
