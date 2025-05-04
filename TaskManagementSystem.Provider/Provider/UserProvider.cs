using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.DataProtection;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.CommonEntities;
using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Provider.IProvider;
using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Repository.Repository;

namespace TaskManagementSystem.Provider.Provider
{
    public class UserProvider : IUserProvider
    {
        private readonly IDataProtector _IDataProtector;
        private readonly IMapper _mapper;
        private UnitOfWork unitofwork = new UnitOfWork();
        private ICommonProvider _commonProvider;

        public UserProvider(IDataProtectionProvider dataProtection, IMapper mapper, ICommonProvider commonProvider)
        {
            _IDataProtector = dataProtection.CreateProtector("TaskManagementSystem");
            _mapper = mapper;
            _commonProvider = commonProvider;
        }
        public DatatablePageResponseModel<HiteshTaskProjectModel> HiteshTaskProjectList(DatatablePageRequestModel datatablePageRequestModel, SessionProviderModel sessionProviderModel)
        {
            DatatablePageResponseModel<HiteshTaskProjectModel> model = new DatatablePageResponseModel<HiteshTaskProjectModel>
            {
                draw = datatablePageRequestModel.Draw,
                data = new List<HiteshTaskProjectModel>()
            };

            try
            {

                var userlist = (from u in unitofwork.HiteshTaskProjectAccess.GetAll(e => e.UserId == sessionProviderModel.UserId).ToList()
                                join a in unitofwork.HiteshTaskProject.GetAll(e => e.IsActive == true)
                                on u.ProjectId equals a.Id
                                join n in unitofwork.HiteshTaskUserMaster.GetAll()
                                on a.CreatedBy equals n.Id
                              
                                select new HiteshTaskProjectModel()
                                {
                                    Id = u.Id,
                                    Title = a.Title,
                                    Description = a.Description,
                                    CreatedName = n.FullName,
                                    CreatedDate = a.CreatedOn.ToString("dd MMM yyyy, hh:mm tt"),

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

        public DatatablePageResponseModel<HiteshTaskAssignTaskModel> HiteshTaskList(DatatablePageRequestModel datatablePageRequestModel, SessionProviderModel sessionProviderModel)
        {
            DatatablePageResponseModel<HiteshTaskAssignTaskModel> model = new DatatablePageResponseModel<HiteshTaskAssignTaskModel>
            {
                draw = datatablePageRequestModel.Draw,
                data = new List<HiteshTaskAssignTaskModel>()
            };

            try
            {

                //var tasks = unitofwork.HiteshTaskAssignTask.GetAll().ToList();
                //var users = unitofwork.HiteshTaskUserMaster.GetAll().ToList();
                //var projects = unitofwork.HiteshTaskProject.GetAll().ToList();
                var data = unitofwork.HiteshTaskAssignTask.GetAll().Where(x => ("," + x.AssignTo + ",").Contains("," + sessionProviderModel.UserId + ",")).ToList();

                var userlist = (from u in data                               
                                join p in unitofwork.HiteshTaskProject.GetAll()
                                on u.ProjectId equals p.Id
                                select new HiteshTaskAssignTaskModel()
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
                                    AssignTo = u.AssignTo,
                                    ProjectId = u.ProjectId,                                    
                                    ProjectName = p.Title,
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

        public HiteshTaskAssignTaskModel StatusChange(int UserId, int id, SessionProviderModel sessionProviderModel)
        {
            ResponseModel result = new ResponseModel();
            HiteshTaskAssignTaskModel model = new HiteshTaskAssignTaskModel();
            try
            {

                var RegDetails = unitofwork.HiteshTaskAssignTask.GetById(UserId);
                RegDetails.Status = (byte)id;
                RegDetails.CompleteBy = sessionProviderModel.UserId;
                unitofwork.HiteshTaskAssignTask.Update(RegDetails);

                unitofwork.Save();


            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "Authentication");
            }
            return model;
        }

        public HiteshTaskUserMasterModel GetUserById(int UserId)
        {
            HiteshTaskUserMasterModel model = new HiteshTaskUserMasterModel();
            try
            {
               
                var data=unitofwork.HiteshTaskUserMaster.GetById(UserId);
                model = new HiteshTaskUserMasterModel
                {
                    Id = data.Id,
                    FullName = data.FullName,
                    Email= data.Email,
                    ImagesUrl=data.Image,
                    
                };
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "Authentication");
            }
            return model;


        }


        public HiteshTaskUserMasterModel GetTaskData(int TaskId)
        {
            HiteshTaskUserMasterModel model = new HiteshTaskUserMasterModel();
            try
            {
                var id=unitofwork.HiteshTaskAssignTask.GetById(TaskId);
                var data = unitofwork.HiteshTaskUserMaster.GetById(Convert.ToInt32(id.AssignTo));
                model = new HiteshTaskUserMasterModel
                {
                    Id = data.Id,
                    FullName = data.FullName,
                    Email = data.Email,
                    ImagesUrl = data.Image,

                };
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "Authentication");
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
        public int count = 1;
        public HIteshTaskMessageModel SaveMessage(HIteshTaskMessageModel message)
        {
            HIteshTaskMessageModel model = new HIteshTaskMessageModel();
            HiteshTaskMessage data= new HiteshTaskMessage();
         try
           {
            data.Message=message.Message;
            data.SendId = message.SendId;
            data.ReceiveId = message.ReceiveId;
            data.TaskId = message.TaskId;
            data.Date = DateOnly.FromDateTime(DateTime.Now);  
            data.Time = TimeOnly.FromDateTime(DateTime.Now);
            model.IsSuccess = true;
                model.SendId = message.SendId;
               
                model.ReceiveId = message.ReceiveId;
            unitofwork.HiteshTaskMessage.Insert(data);
            unitofwork.Save();
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "Authentication");
            }
            return model;
        }

   
       

        public List<HIteshTaskMessageModel> GetMessage(int uid, int rid)
        {
            var data=unitofwork.HiteshTaskMessage.GetAll(m =>m.SendId == uid && m.ReceiveId == rid || m.SendId == rid && m.ReceiveId == uid) .ToList();
            List<HIteshTaskMessageModel> model=new List<HIteshTaskMessageModel>();
            foreach (var item in data)
            {
                HIteshTaskMessageModel dataitem = new HIteshTaskMessageModel()
                {
                    Message = item.Message,
                    Date = item.Date,
                    Time = item.Time,
                    Id = item.Id,
                    SendId = item.SendId,
                    ReceiveId = item.ReceiveId,
                };
                model.Add(dataitem);
            }
            return model;
            
        }

        public List<HIteshTaskMessageModel> GetmsgData(int uid)
        {
            var data = unitofwork.HiteshTaskMessage.GetAll(m =>m.ReceiveId==uid).ToList();

           
            List<HIteshTaskMessageModel> model = new List<HIteshTaskMessageModel>();
            foreach (var item in data)
            {
                var udata = unitofwork.HiteshTaskUserMaster.GetAll(m =>m.Id==item.SendId).FirstOrDefault();
                HIteshTaskMessageModel dataitem = new HIteshTaskMessageModel()
                {
                    ImagesUrl = udata.Image,
                    FullName = udata.FullName,
                    Message = item.Message,
                    Date = item.Date,
                    Time = item.Time,
                    Id = item.Id,
                    SendId = item.ReceiveId,
                    ReceiveId = item.SendId,
                    count = data.Count(),
                };
                model.Add(dataitem);
            }
            return model;

        }
        public List<HiteshTaskAssignTaskModel> GetToDoCount(DateTime startDate, DateTime endDate ,SessionProviderModel model)
        {

            var tasks = unitofwork.HiteshTaskAssignTask.GetAll()
          .Where(x => x.CreatedOn >= startDate && x.CreatedOn <= endDate);

            if (model.RoleId == (int)Enumeration.Role.User)
            {
                tasks = tasks.Where(x => ("," + x.AssignTo + ",").Contains("," + model.UserId + ","));
            }
            var data = tasks
           .GroupBy(x => x.CreatedOn.Date) // Group by date only
          .Select(g => new HiteshTaskAssignTaskModel
          {
              Title = "", // You can also say $"Tasks: {g.Count()}"
              Start = g.Key.ToString("yyyy-MM-dd"),
              End = g.Key.ToString("yyyy-MM-dd"),
              AllDay = true,
              TotalCount = g.Count(),

              SlotDate = g.Key.ToString("yyyy-MM-dd"),
              complete = g.Count(x => x.Status == (byte)Enumeration.Status.Confirm),
              pending = g.Count(x => x.Status == (byte)Enumeration.Status.Pending),

            })
         .ToList();

                return data;
                   
          
        }

        public List<HiteshTaskAssignTaskModel> GetDateData(string date , SessionProviderModel sessionProviderModel)
        {
            DateTime taskDate = DateTime.Parse(date);

            var task = unitofwork.HiteshTaskAssignTask.GetAll(e => e.CreatedOn.Date == taskDate.Date);
            var user = unitofwork.HiteshTaskUserMaster.GetAll().ToList();
          

            if (sessionProviderModel.RoleId == (int)Enumeration.Role.User)
            {
                task = task.Where(x => ("," + x.AssignTo + ",").Contains("," + sessionProviderModel.UserId + ","));
            }

            var data = task.ToList();
            List<HiteshTaskAssignTaskModel> List = new List<HiteshTaskAssignTaskModel>();

            foreach (var item in data)
            {
                                var userIds = item.AssignTo?.Split(',') ?? new string[0];
                                var assignedUsers = user.Where(x => userIds.Contains(x.Id.ToString())).ToList();
                                var completeByNames = string.Join(", ", assignedUsers.Select(x => x.FullName));
                                var project = unitofwork.HiteshTaskProject.GetById(item.ProjectId);
                var assigndata = new HiteshTaskAssignTaskModel
                {
                    ProjectName=project?.Title ?? "No project",
                    Title= item.Title,
                   AssignTo= completeByNames,
                   roleid=sessionProviderModel.RoleId,
                    Description= item.Description,
                    CreatedOn = item.CreatedOn,
                    PriorityString= ((Enumeration.Priority)item.Priority).ToString(),
                    StatusString=((Enumeration.Status)item.Status).ToString(),
                    DeadLine=item.DeadLine,
                };
                List.Add(assigndata);
            }

           
            
            return List;
        }

    }
}
