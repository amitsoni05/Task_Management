using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.CommonEntities;
using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Provider.IProvider;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Repository.Repository;
namespace TaskManagementSystem.Provider.Provider
{
    public class ProjectProvider:IProjectProvider
    {
        private readonly IDataProtector _IDataProtector;
        private readonly IMapper _mapper;
        private UnitOfWork unitofwork = new UnitOfWork();
        private ICommonProvider _commonProvider;

        public ProjectProvider(IDataProtectionProvider dataProtection, IMapper mapper, ICommonProvider commonProvider)
        {
            _IDataProtector = dataProtection.CreateProtector("TaskManagementSystem");
            _mapper = mapper;
            _commonProvider = commonProvider;
        }

        public DatatablePageResponseModel<HiteshTaskProjectModel> HiteshTaskProjectList(DatatablePageRequestModel datatablePageRequestModel)
        {
            DatatablePageResponseModel<HiteshTaskProjectModel> model = new DatatablePageResponseModel<HiteshTaskProjectModel>
            {
                draw = datatablePageRequestModel.Draw,
                data = new List<HiteshTaskProjectModel>()
            };

            try
            {

                var userlist = (from u in unitofwork.HiteshTaskProject.GetAll(e=>e.IsActive==true)
                                join n in unitofwork.HiteshTaskUserMaster.GetAll()
                                on u.CreatedBy equals n.Id
                                select new HiteshTaskProjectModel()
                                {
                                    Id = u.Id,
                                    Title=u.Title,
                                    Description=u.Description,
                                    CreatedName=n.FullName,
                                    CreatedDate = u.CreatedOn.ToString("dd MMM yyyy, hh:mm tt"),

                                }
                                );
                model.recordsTotal = userlist.Count();


                if (!string.IsNullOrEmpty(datatablePageRequestModel.SearchText))
                {
                    userlist = userlist.Where(x =>
                    x.Title.ToLower().Contains(datatablePageRequestModel.SearchText.ToLower())
                    || x.Description.ToLower().Contains(datatablePageRequestModel.SearchText.ToLower())

                    );
                }


                model.recordsFiltered = userlist.Count();
                model.data = userlist.Skip(datatablePageRequestModel.StartIndex).Take(datatablePageRequestModel.PageSize).ToList().Select(x =>
                {
                    x.EncProjectId = _commonProvider.Protect(x.Id);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {

            }
            return model;
        }

        public ResponseModel SaveProject(HiteshTaskProjectModel projectdata, SessionProviderModel sessionProviderModel)
        {
            ResponseModel model = new ResponseModel();
            HiteshTaskProject umodel = new HiteshTaskProject();
            try
            {
                if (projectdata.Id > 0)
                {
                    if (projectdata.UserIds.Count > 0)
                    {
                        var data = unitofwork.HiteshTaskProjectAccess.GetAll(e => e.ProjectId == projectdata.Id);
                        foreach (var item in data) { 
                          unitofwork.HiteshTaskProjectAccess.Delete(item);
                        }
                        HiteshTaskProjectAccess accessmodel = new HiteshTaskProjectAccess();

                        foreach (var userId in projectdata.UserIds)
                        {
                            var newAccess = new HiteshTaskProjectAccess
                            {
                                ProjectId = projectdata.Id,
                                UserId = userId,
                                
                            };

                            unitofwork.HiteshTaskProjectAccess.Insert(newAccess); 
                        }
                        
                        unitofwork.Save();
                    }
                    umodel = unitofwork.HiteshTaskProject.GetById(projectdata.Id);
                    umodel.Title = projectdata.Title;
                    umodel.Description = projectdata.Description;
                   
                    unitofwork.HiteshTaskProject.Update(umodel, sessionProviderModel.UserId, sessionProviderModel.Ip);
                    unitofwork.Save();
                    model.IsSuccess = true;
                    model.Message = "Project Update Successfully";
                }
                else
                {

                    umodel = new HiteshTaskProject()
                    {
                        Title = projectdata.Title,
                        Description = projectdata.Description,                      
                        CreatedBy = sessionProviderModel.UserId,
                        IsActive=true,
                    };
                    unitofwork.HiteshTaskProject.Insert(umodel, sessionProviderModel.UserId, sessionProviderModel.Ip);
                    unitofwork.Save();

                    HiteshTaskProjectAccess accessmodel = new HiteshTaskProjectAccess();
                    foreach(var item in projectdata.UserIds)
                    {
                        accessmodel = new HiteshTaskProjectAccess
                        {
                            ProjectId=umodel.Id,
                            UserId=item,
                        };
                        unitofwork.HiteshTaskProjectAccess.Insert(accessmodel);
                        unitofwork.Save();
                    }
                    model.IsSuccess = true;
                    model.Message = "Project Insert Successfully";
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


        public HiteshTaskProjectModel GetProjectById(int id)
        {
            HiteshTaskProjectModel model = new HiteshTaskProjectModel();
            try
            {
                if (id > 0)
                {
                    var data = unitofwork.HiteshTaskProject.GetById(id);                                    
                    model.UserIds=unitofwork.HiteshTaskProjectAccess.GetAll(e => e.ProjectId == id).Select(x=>x.UserId).ToList();
                    var user = unitofwork.HiteshTaskUserMaster.GetAll();
                    model.UserMasterList = user.Select(u => new SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = u.FullName,
                        Selected = model.UserIds.Contains(u.Id)
                    }).ToList();
                    model.Title = data.Title;
                    model.Description = data.Description;
                    model.IsEdit = true;
                    model.Id = id;
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

        public ResponseModel DeleteProject(int id)
        {
            ResponseModel model = new ResponseModel();

            try
            {
                if (id > 0)
                {
                    var data = unitofwork.HiteshTaskProject.GetById(id);
                    data.IsActive = false;
                    unitofwork.HiteshTaskProject.Update(data);
                    unitofwork.Save();
                    model.IsSuccess = true;
                    model.Message = "Project Delete Successfully";
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

        public List<HiteshTaskUserMasterModel> GetUserMasterList(int id)
        {
            List<HiteshTaskUserMasterModel> model=new List<HiteshTaskUserMasterModel> ();
            var Data = unitofwork.HiteshTaskProjectAccess.GetAll(e=>e.ProjectId==id).ToList();
            foreach(var data in Data)
            {
                var user = unitofwork.HiteshTaskUserMaster.GetAll(e => e.Id == data.UserId).FirstOrDefault();
               
                    HiteshTaskUserMasterModel udata = new HiteshTaskUserMasterModel()
                    {
                        FullName = user.FullName,
                        Email = user.Email,

                    };
                    model.Add(udata);
                
               
            }
            return model;
        }


        public ResponseModel DocumentSave(HiteshTaskDocumentSaveModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                foreach (var imgUrl in model.DocName)
                {
                    HiteshTaskDocumentSave hmodel = new HiteshTaskDocumentSave();
                    hmodel.ProjectId = model.ProjectId;
                    hmodel.DocumentName = imgUrl;
                    unitofwork.HiteshTaskDocumentSave.Insert(hmodel);
                }
                unitofwork.Save();
                response.IsSuccess = true;



            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "Authentication");
            }
            return response;
        }

        public List<HiteshTaskDocumentSaveModel> GetDocumentList(int id)
        {
           List<HiteshTaskDocumentSaveModel> model=new List<HiteshTaskDocumentSaveModel> ();

            var data=unitofwork.HiteshTaskDocumentSave.GetAll(e=>e.ProjectId==id);

            foreach (var item in data)
            {
                HiteshTaskDocumentSaveModel mdata= new HiteshTaskDocumentSaveModel()              
                {
                   Id = item.Id,
                   ProjectId = item.ProjectId,
                   DocumentName = item.DocumentName,
                   TaskId=item.TaskId,
                };
                model.Add(mdata);
            }
            return model;
        }

        public HiteshTaskDocumentSaveModel GetDocId(int id)
        {
            HiteshTaskDocumentSaveModel model = new HiteshTaskDocumentSaveModel();
            try
            {
                var data=unitofwork.HiteshTaskDocumentSave.GetById(id);
                model.Id = data.Id;
                model.DocumentName = data.DocumentName;
                model.ProjectId = data.ProjectId;
                model.TaskId = data.TaskId;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "Authentication");
            }
            return model;
        }

        public ResponseModel DeleteDocData(int id)
        {
            ResponseModel model = new ResponseModel();

            try
            {
                if (id > 0)
                {
                    var data = unitofwork.HiteshTaskDocumentSave.GetById(id);
                  
                    unitofwork.HiteshTaskDocumentSave.Delete(data);
                    unitofwork.Save();
                    model.IsSuccess = true;
                    model.Message = "Document Delete Successfully";
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
    }
}
