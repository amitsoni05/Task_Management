using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.CommonEntities;
using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Provider.IProvider;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Repository.Repository;

namespace TaskManagementSystem.Provider.Provider
{
    public class TaskProvider:ITaskProvider
    {
        private readonly IDataProtector _IDataProtector;
        private readonly IMapper _mapper;
        private UnitOfWork unitofwork = new UnitOfWork();
        private ICommonProvider _commonProvider;

        public TaskProvider(IDataProtectionProvider dataProtection, IMapper mapper, ICommonProvider commonProvider)
        {
            _IDataProtector = dataProtection.CreateProtector("TaskManagementSystem");
            _mapper = mapper;
            _commonProvider = commonProvider;
        }

        public ResponseModel DeleteTask(int id)
        {
            ResponseModel model= new ResponseModel();
            if (id > 0)
            {
                unitofwork.HiteshTaskAssignTask.Delete(id);
                unitofwork.Save();
                model.IsSuccess=true;
                model.Message = "Task Deleted !!";
            }
            return model;
        }

        public HiteshTaskAssignTaskModel GetTaskById(int id)
        {
            HiteshTaskAssignTaskModel model = new HiteshTaskAssignTaskModel();
            if (id > 0)
            {
                var data = unitofwork.HiteshTaskAssignTask.GetById(id);

                model.AllDocumentsList = unitofwork.HiteshTaskDocumentSave.GetAll().Where(d => d.TaskId == data.Id)
                    .Select(d => new HiteshTaskDocumentSaveModel
                    {
                       DocumentName = d.DocumentName,
                      DocumentType = d.DocumentType,
                   }).ToList();
                // Step 1: Split comma-separated string into int list
                var assignedUserIds = data.AssignTo?
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList() ?? new List<int>();

                // Step 2: Get all users
                var users = unitofwork.HiteshTaskUserMaster.GetAll();

                // Step 3: Populate UserIds and UserMasterList
                model.UserIds = assignedUserIds;

                model.UserMasterList = users.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.FullName,
                    Selected = assignedUserIds.Contains(u.Id)
                }).ToList();

                // Step 4: Map the rest of the task data
                model.Id = data.Id;
                model.Title = data.Title;
                model.Description = data.Description;
                model.DeadLine = data.DeadLine;
                model.ProjectId = data.ProjectId;
                model.Priority = data.Priority;
               // model.AssignTo = data.AssignTo;
                model.IsEdit = true;
                model.IsSuccess= true;
            }
            return model;
        }

        public DatatablePageResponseModel<HiteshTaskAssignTaskModel> HiteshTaskList(DatatablePageRequestModel datatablePageRequestModel)
        {
            DatatablePageResponseModel<HiteshTaskAssignTaskModel> model = new DatatablePageResponseModel<HiteshTaskAssignTaskModel>
            {
                draw = datatablePageRequestModel.Draw,
                data = new List<HiteshTaskAssignTaskModel>()
            };

            try
            {

                

                // Get all data from DB first
                var tasks = unitofwork.HiteshTaskAssignTask.GetAll().ToList();
                var users = unitofwork.HiteshTaskUserMaster.GetAll().ToList();
                var projects = unitofwork.HiteshTaskProject.GetAll().ToList();

                // Build final list
                var userlist = (from u in tasks
                                let userIds = u.AssignTo?.Split(',') ?? new string[0]
                                let assignedUsers = users.Where(x => userIds.Contains(x.Id.ToString())).ToList()
                                let completeByNames = string.Join(", ", assignedUsers.Select(x => x.FullName))
                                let project = projects.FirstOrDefault(p => p.Id == u.ProjectId)
                                let completeby=users.FirstOrDefault(e=>e.Id==u.CompleteBy)
                                select new HiteshTaskAssignTaskModel
                                {
                                    Id = u.Id,
                                    Title = u.Title,
                                    Description = u.Description,
                                    Ip = u.Ip,
                                    CreatedDate = u.CreatedOn.ToString("dd MMM yyyy, hh:mm tt"),
                                    Priority = u.Priority,
                                    Status = u.Status,
                                    PriorityString = ((Enumeration.Priority)u.Priority).ToString(),
                                    StatusString = ((Enumeration.Status)u.Status).ToString(),
                                    DeadLine = u.DeadLine,
                                    Active = u.Active,
                                    AssignTo = completeByNames,
                                    ProjectId = u.ProjectId,
                                    CompleteBy = completeby?.FullName ?? "",
                                    ProjectName = project?.Title ?? ""
                                }).ToList();

                model.recordsTotal = userlist.Count();


                if (!string.IsNullOrEmpty(datatablePageRequestModel.SearchText))
                {
                    userlist = userlist.Where(x =>
                    x.Title.ToLower().Contains(datatablePageRequestModel.SearchText.ToLower())
                    || x.Description.ToLower().Contains(datatablePageRequestModel.SearchText.ToLower())

                    ).ToList();
                }


                model.recordsFiltered = userlist.Count();
                model.data = userlist.Skip(datatablePageRequestModel.StartIndex).Take(datatablePageRequestModel.PageSize).ToList().Select(x =>
                {
                    x.EncTaskId = _commonProvider.Protect(x.Id);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {

            }
            return model;
        }



        public ResponseModel SaveTask(HiteshTaskAssignTaskModel data, SessionProviderModel sessionProviderModel)
        {
            ResponseModel model = new ResponseModel();
            HiteshTaskAssignTask umodel = new HiteshTaskAssignTask();
            List<string> Data = new List<string>();
            
            try
            {
              
                if (data.Id > 0)
                {
                    var taskdata = unitofwork.HiteshTaskAssignTask.GetById(data.Id);
                    if (data.ExistingFiles !=null)
                    {
                        foreach (var item in data.ExistingFiles)
                        {
                            Data.Add(item);
                        }
                    }
                    if (data.DocName != null) {
                        foreach (var item in data.DocName)
                        {
                            Data.Add(item);
                        }
                    }
                    if(data.DocName == null)
                    {
                        var taskid = unitofwork.HiteshTaskDocumentSave.GetAll(x => x.TaskId == taskdata.Id);
                        foreach (var item in taskid)
                        {
                            unitofwork.HiteshTaskDocumentSave.Delete(item);
                        }
                    }
                    
                    taskdata.Title = data.Title;
                    taskdata.Description = data.Description;
                    taskdata.DeadLine = data.DeadLine;
                    taskdata.ProjectId = data.ProjectId;
                    taskdata.Priority = data.Priority;
                    taskdata.AssignTo = string.Join(",", data.UserIds);

                    unitofwork.HiteshTaskAssignTask.Update(taskdata, sessionProviderModel.UserId, sessionProviderModel.Ip);

                    if (data.DocName != null && data.DocumentTypeList != null && data.DocumentTypeList.Count==Data.Count)
                    {
                        var taskid = unitofwork.HiteshTaskDocumentSave.GetAll(x => x.TaskId == taskdata.Id);
                        foreach(var item in taskid)
                        {
                            unitofwork.HiteshTaskDocumentSave.Delete(item);
                        }

                        for (int i = 0; i < data.DocumentTypeList.Count; i++)
                        {
                            
                            HiteshTaskDocumentSave hmodel = new HiteshTaskDocumentSave
                            {
                                TaskId = taskdata.Id,
                                DocumentName = Data[i],
                                DocumentType = data.DocumentTypeList[i]
                            };
                            unitofwork.HiteshTaskDocumentSave.Insert(hmodel);
                        }
                    }
                    else if(data.DocumentTypeList.Count==data.Files.Count)
                    {
                        var taskid = unitofwork.HiteshTaskDocumentSave.GetAll(x => x.TaskId == taskdata.Id);
                        foreach (var item in taskid)
                        {
                            unitofwork.HiteshTaskDocumentSave.Delete(item);
                        }

                        for (int i = 0; i < data.DocumentTypeList.Count; i++)
                        {

                            HiteshTaskDocumentSave hmodel = new HiteshTaskDocumentSave
                            {
                                TaskId = taskdata.Id,
                                DocumentName = data.DocName[i],
                                DocumentType = data.DocumentTypeList[i]
                            };
                            unitofwork.HiteshTaskDocumentSave.Insert(hmodel);
                        }
                    }

                    unitofwork.Save();

                    model.IsSuccess = true;
                    model.Message = "Task Update Successfully";
                }
                else
                {

                    umodel = new HiteshTaskAssignTask()
                    {
                        Title = data.Title,
                        Description = data.Description,
                        CreatedBy = sessionProviderModel.UserId,
                        Priority = data.Priority,
                        Status = (byte)Enumeration.Status.Pending,

                        DeadLine = data.DeadLine,
                        Active = true,
                        AssignTo = string.Join(",", data.UserIds),
                        ProjectId = data.ProjectId,

                    };
                    unitofwork.HiteshTaskAssignTask.Insert(umodel, sessionProviderModel.UserId, sessionProviderModel.Ip);
                    unitofwork.Save();
                    if (data.DocName != null && data.DocumentTypeList != null && data.DocName.Count == data.DocumentTypeList.Count)
                    {
                        for (int i = 0; i < data.DocName.Count; i++)
                        {
                            HiteshTaskDocumentSave hmodel = new HiteshTaskDocumentSave
                            {
                                TaskId = umodel.Id,
                                DocumentName = data.DocName[i],
                                DocumentType = data.DocumentTypeList[i]
                            };
                            unitofwork.HiteshTaskDocumentSave.Insert(hmodel);
                        }

                        unitofwork.Save();
                    }


                    model.IsSuccess = true;
                    model.Message = "Task Insert Successfully";
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

        //public List<HiteshTaskUserMasterModel> GetUserMasterList(int id)
        //{
        //    List<HiteshTaskUserMasterModel> model = new List<HiteshTaskUserMasterModel>();

        //    var Data = unitofwork.HiteshTaskAssignTask.GetAll(e => e.Id == id).FirstOrDefault();

        //        var user = unitofwork.HiteshTaskUserMaster.GetById(Data.AssignTo);

        //        HiteshTaskUserMasterModel udata = new HiteshTaskUserMasterModel()
        //        {
        //            FullName = user.FullName,
        //            Email = user.Email,

        //        };
        //        model.Add(udata);

        //    return model;
        //}

        public List<HiteshTaskUserMasterModel> GetUserMasterList(int id)
        {
            var model = new List<HiteshTaskUserMasterModel>();

            var task = unitofwork.HiteshTaskAssignTask.GetAll(e => e.Id == id).FirstOrDefault();
            if (task == null || string.IsNullOrEmpty(task.AssignTo))
                return model;

            // If AssignTo = "2,5,8"
            // Convert comma-separated string to list of int
            List<int> userIds = task.AssignTo.Split(',').Select(int.Parse).ToList();

            // Get all users where ID is in the list
            var users = unitofwork.HiteshTaskUserMaster.GetAll(u => userIds.Contains(u.Id));

            foreach (var user in users)
            {
                var udata = new HiteshTaskUserMasterModel
                {
                    Id=user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                };
                model.Add(udata);
            }

            return model;
        }


    }
}
