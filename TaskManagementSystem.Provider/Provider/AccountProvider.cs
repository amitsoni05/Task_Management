using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Provider.IProvider;
using TaskManagementSystem.Repository.Repository;

namespace TaskManagementSystem.Provider.Provider
{
    public class AccountProvider : IAccountProvider
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private IMapper _mapper;

        public AccountProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            this._mapper = mapper;
            this._commonProvider = commonProvider;
        }
        public HiteshTaskUserMasterModel CheckLogin(HiteshTaskUserMasterModel master)
        {
            HiteshTaskUserMasterModel model = new HiteshTaskUserMasterModel();
            try
            {
                var data = unitOfWork.HiteshTaskUserMaster.GetAll(e => e.Email == master.Email);
                foreach(var item in data)
                {
                  
                        if (PasswordHash.ValidatePassword(AES.DecryptAES(master.Password), item.Password))
                        {
                            model.FullName = item.FullName;
                            model.Id = item.Id;
                            model.RoleId = item.Role;
                            model.IsSuccess = true;
                        break;
                        }
                        else
                        {
                            model.IsSuccess = false;
                            model.Message = "Inavlide Data";
                        }

                   
                }
                
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
               model.Message = AppCommon.ErrorMessage;
               AppCommon.LogException(ex, "Authentication");
            }

            return model;

        }


        public HiteshTaskUserMasterModel CheckPassword(HiteshTaskUserMasterModel master)
        {
            HiteshTaskUserMasterModel model=new HiteshTaskUserMasterModel();
            try
            {
                var data= unitOfWork.HiteshTaskUserMaster.GetById(master.Id);

                if (PasswordHash.ValidatePassword(AES.DecryptAES(master.Password), data.Password))
                {
                    if (master.newpassword == master.confirmpassword)
                    {
                        data.Password = PasswordHash.CreateHash(master.confirmpassword);
                        unitOfWork.HiteshTaskUserMaster.Update(data);
                        unitOfWork.Save();
                        model.IsSuccess = true;
                        model.Message = "Password Update Successfull !!";

                    }
                    else
                    {
                        model.IsSuccess = false;
                        model.Message = "New password And Confirm Password Are Not Match  !!";
                    }
                }
                else
                {
                    model.IsSuccess = false;
                    model.Message = "Please Enter Valid Original Password";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "Authentication");
            }
            return model;
        }

        public ResponseModel UpdateProfile(HiteshTaskUserMasterModel master)
        {
            ResponseModel model = new ResponseModel();
        
            try
            {
                var data = unitOfWork.HiteshTaskUserMaster.GetById(master.Id);
                data.Image=master.ImagesUrl;
                unitOfWork.Save();
                model.IsSuccess = true;
                model.Message = "Image Update SuccessFully";
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "Authentication");
            }

            return model;
        }



        public ResponseModel CheckEmail(HiteshTaskUserMasterModel master)
        {
            ResponseModel model = new ResponseModel();

            try
            {
                var data = unitOfWork.HiteshTaskUserMaster.GetAll(e=>e.Email==master.Email).FirstOrDefault();
                 if(data!= null)
                {

                    string resetLink = AppCommon.Application_URL + "Account/Reset/" + _commonProvider.ProtectString(data.Id.ToString()+"|"+DateTime.Now.Ticks.ToString());

                    string mailBody = $@"Hello {data.FullName},<br><br>
                                    
                                        Click below link to reset your password. <br><br> 
                                        <a href='{resetLink}'>{resetLink}</a><br>   <br>                                   
                                        <b>Note:</b> Above link will expire with in 30 min.<br> 
                                        If link is not clickable then please copy and past in your browser.<br><br>

                                        <b>Thanks & Regard</b><br>
                                        {AppCommon.ApplicationLongTitle}
                                    ";

                    EmailSender.SendEmail(data.Email, "", AppCommon.ApplicationTitle, "Reset Password", mailBody);
                    model.IsSuccess = true;
                    model.Message = "Reset password link sent to your email.";
                    

                }
                else
                {
                    model.IsSuccess = false;
                    model.Message = "Please Enter A Valid Email !!";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "Authentication");
            }

            return model;
        }

        public ResponseModel ChangeOrResetPassword(HiteshTaskUserMasterModel master)
        {
            ResponseModel model = new ResponseModel();
            if (master.newpassword == master.confirmpassword)
            {
                var data=unitOfWork.HiteshTaskUserMaster.GetById(master.Id);
                data.Password=PasswordHash.CreateHash(master.confirmpassword);
                unitOfWork.HiteshTaskUserMaster.Update(data);
                unitOfWork.Save();
                model.IsSuccess = true;
                
            }

            return model;
        }
    }
}
