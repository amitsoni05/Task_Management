using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.CommonEntities;
using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Provider.IProvider;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Repository.Repository;

namespace TaskManagementSystem.Provider.Provider
{
    public class AdminProvider : IAdminProvider
    {
        private IDataProtector _IDataProtector;
        private readonly IMapper _mapper;
        private UnitOfWork unitofwork = new UnitOfWork();
        private ICommonProvider _commonProvider;

        public AdminProvider(IDataProtectionProvider dataProtection, IMapper mapper, ICommonProvider commonProvider)
        {
            _IDataProtector = dataProtection.CreateProtector("TaskManagementSystem");
            _mapper = mapper;
            _commonProvider = commonProvider;
        }
        public DatatablePageResponseModel<HiteshTaskUserMasterModel> HiteshTaskUserMasterList(DatatablePageRequestModel datatablePageRequestModel,SessionProviderModel sessionProviderModel)
        {
            DatatablePageResponseModel<HiteshTaskUserMasterModel> model = new DatatablePageResponseModel<HiteshTaskUserMasterModel>
            {
                draw = datatablePageRequestModel.Draw,
                data = new List<HiteshTaskUserMasterModel>()
            };

            try
            {

                var userlist = (from u in unitofwork.HiteshTaskUserMaster.GetAll(e=>e.Role==(int)Enumeration.Role.User) 
                               
                               select new HiteshTaskUserMasterModel()
                                {
                                    Id = u.Id,
                                   FullName=u.FullName,
                                   Email=u.Email,
                                   RoleId=u.Role,
                                   CreatedName=sessionProviderModel.Username,
                                   CreatedDate = u.CreatedOn.ToString("dd MMM yyyy, hh:mm tt"),
                                   Active =u.Active,
                                }
                                );
                model.recordsTotal = userlist.Count();


                if (!string.IsNullOrEmpty(datatablePageRequestModel.SearchText))
                {
                    userlist = userlist.Where(x =>
                    x.FullName.ToLower().Contains(datatablePageRequestModel.SearchText.ToLower())
                    || x.Email.ToLower().Contains(datatablePageRequestModel.SearchText.ToLower())
                   
                    );
                }


                model.recordsFiltered = userlist.Count();
                model.data = userlist.Skip(datatablePageRequestModel.StartIndex).Take(datatablePageRequestModel.PageSize).ToList().Select(x =>
                {
                    x.EncUserId = _commonProvider.Protect(x.Id);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {

            }
            return model;
        }

        public ResponseModel SaveEmployee(HiteshTaskUserMasterModel employee, SessionProviderModel sessionProviderModel)
        {
            ResponseModel model = new ResponseModel();
            HiteshTaskUserMaster umodel = new HiteshTaskUserMaster();
            try
            {
                if (employee.Id > 0)
                {
                     umodel = unitofwork.HiteshTaskUserMaster.GetById(employee.Id);
                    umodel.FullName = employee.FullName;
                    umodel.Email = employee.Email;
                    unitofwork.HiteshTaskUserMaster.Update(umodel,sessionProviderModel.UserId,sessionProviderModel.Ip);
                    unitofwork.Save();
                    model.IsSuccess = true;                  
                    model.Message = "Data Update Successfully";
                }
                else
                {
                    umodel = new HiteshTaskUserMaster()
                    {
                        FullName = employee.FullName,
                        Email = employee.Email,
                        Password = PasswordHash.CreateHash(employee.Password),
                        Role = (int)Enumeration.Role.User,
                        CreatedBy = sessionProviderModel.UserId,
                        Active = true,
                    };
                    unitofwork.HiteshTaskUserMaster.Insert(umodel, sessionProviderModel.UserId, sessionProviderModel.Ip);
                    unitofwork.Save();

                    string Subject = "";
                    string Body = "";
                    string email = employee.Email;
                    if (employee.Id==0)
                    {
                        Subject = "Login Successfully -Project Management System";
                        Body = $@"
                            Dear {employee.FullName},<br><br>                          
                            <strong>Details:</strong><br>
                             Email: {employee.Email} <br>
                             Password: {employee.Password} <br>                           

                          Thank you for Join with us!<br><br>
                          Best Regards,<br>
                         Project Management System
                        ";
                    }
                    
                    // Send Email
                    EmailSender.SendEmail(email, "", "Project Task Management System", Subject, Body);                    
              
                    model.IsSuccess = true;
                    model.Message = "Data Insert Successfully";
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

        public HiteshTaskUserMasterModel GetEmployeeById(int id)
        {
            HiteshTaskUserMasterModel model = new HiteshTaskUserMasterModel();
            try
            {
                if (id > 0)
                {
                    var data = unitofwork.HiteshTaskUserMaster.GetById(id);
                    model.FullName = data.FullName;
                    model.Email = data.Email;
                    model.Id = id;   
                    model.IsSuccess= true;
                    model.IsEdit = true;
                 }
                else
                {
                    model.IsSuccess = false;
                }
            } catch (Exception ex) {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "Authentication");
            }
            return model;
        }

        public ResponseModel DeleteEmployee(int id)
        {
            ResponseModel model = new ResponseModel();
            
            try
            {
                if (id > 0)
                {
                    var data= unitofwork.HiteshTaskUserMaster.GetById(id);
                    data.Active = false;
                    unitofwork.HiteshTaskUserMaster.Update(data);
                    unitofwork.Save();
                    model.IsSuccess = true;
                }
                else
                {
                    model.IsSuccess = false;
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

        public HiteshTaskUserMasterModel CheckEmail(string email)
        {
           HiteshTaskUserMasterModel model=new HiteshTaskUserMasterModel();
            var data=unitofwork.HiteshTaskUserMaster.GetAll(e=>e.Email == email).FirstOrDefault();
            if(data != null)
            {
                model.IsSuccess = true;
                model.Message = AppCommon.ErrorMessage;
                model.Email = email;
            }
            return model;
        }
    }
}
