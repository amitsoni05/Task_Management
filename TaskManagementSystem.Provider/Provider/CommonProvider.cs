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

using Microsoft.AspNetCore.DataProtection;
using static System.Net.Mime.MediaTypeNames;

namespace TaskManagementSystem.Provider.Provider
{
    public class CommonProvider:ICommonProvider
    {
        public UnitOfWork unitOfWork = new UnitOfWork();
        private IDataProtector _IDataProtector;

        public CommonProvider(IDataProtectionProvider protectionProvider)
        {
            _IDataProtector = protectionProvider.CreateProtector("EventSolution");
        }

       
        public string Protect(int value)
        {
            return _IDataProtector.Protect(value.ToString());
        }
        public string ProtectLong(long value)
        {
            return _IDataProtector.Protect(value.ToString());
        }
        public string ProtectShort(short value)
        {
            return _IDataProtector.Protect(value.ToString());
        }
        public int UnProtect(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string data = _IDataProtector.Unprotect(value);
                data = data ?? "0";
                return Convert.ToInt32(data);
            }
            else
                return 0;

        }
        public string ProtectString(string value)
        {
            value = value ?? string.Empty;
            return _IDataProtector.Protect(value);
        }
        public string UnProtectString(string value)
        {
            return _IDataProtector.Unprotect(value);
        }
        public long UnProtectLong(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string data = _IDataProtector.Unprotect(value);
                data = data ?? "0";
                return Convert.ToInt64(data);
            }
            else
                return 0;
        }

        public List<DropDownModel> GetAllProject()
        {
            List<DropDownModel> model = new List<DropDownModel>();
            try
            {

                model = (from f in unitOfWork.HiteshTaskProject.GetAll(e=>e.IsActive==true)
                         select new DropDownModel()
                         {
                             Value = f.Id.ToString(),
                             Text = f.Title,
                         }).ToList();


            }
            catch (Exception ex)
            {

                AppCommon.LogException(ex, "Contactprovider=>GetAllLeadId");
            }
            return model;
        }

        public List<DropDownModel> GetAllUser(int id)
        {
            List<DropDownModel> model = new List<DropDownModel>();
            try
            {
              
                    model = (from f in unitOfWork.HiteshTaskProjectAccess.GetAll(e=>e.ProjectId==id)
                             join u in unitOfWork.HiteshTaskUserMaster.GetAll()
                             on f.UserId equals u.Id
                             select new DropDownModel()
                             {
                                 Value = f.Id.ToString(),
                                 Text = u.FullName,
                             }).ToList();
                         
            }
            catch (Exception ex)
            {

                AppCommon.LogException(ex, "Contactprovider=>GetAllLeadId");
            }
            return model;
        }
        public List<DropDownModel> GetUser()
        {
            List<DropDownModel> model = new List<DropDownModel>();
            try
            {

                model = (from f in unitOfWork.HiteshTaskUserMaster.GetAll(e => e.Active == true && e.Role == (int)Enumeration.Role.User)
                         select new DropDownModel()
                         {
                             Value = f.Id.ToString(),
                             Text = f.FullName,
                         }).ToList();



            }
            catch (Exception ex)
            {

                AppCommon.LogException(ex, "Contactprovider=>GetAllLeadId");
            }
            return model;
        }

        public List<DropDownModel> GetSelectedUser(int PId)
        {
            List<DropDownModel> model = new List<DropDownModel>();
            try
            {
                model = (from f in unitOfWork.HiteshTaskProjectAccess.GetAll(e => e.ProjectId == PId)
                         join u in unitOfWork.HiteshTaskUserMaster.GetAll(e=>e.Active==true)
                         on f.UserId equals u.Id
                         select new DropDownModel()
                         {
                             Value = u.Id.ToString(),
                             Text = u.FullName,
                         }).ToList();


            }
            catch (Exception ex)
            {

                AppCommon.LogException(ex, "Contactprovider=>GetAllLeadId");
            }
            return model;
        }

        public int[] GetStatusData(DateTime? startDate, DateTime? endDate, int? userId)
         {
            
            var tasks = unitOfWork.HiteshTaskAssignTask.GetAll();

            if (startDate.HasValue && endDate.HasValue)
            {
                tasks = tasks.Where(x => x.CreatedOn >= startDate && x.CreatedOn <= endDate);
            }

            if (userId.HasValue)
            {
                tasks = tasks.Where(x => ("," + x.AssignTo + ",").Contains("," + userId.Value + ","));
            }

             return new int[]
            {
               tasks.Count(x => x.Status == (byte)Enumeration.Status.Pending),              
                tasks.Count(x => x.Status == (byte)Enumeration.Status.InProgresss),
                tasks.Count(x => x.Status == (byte)Enumeration.Status.Confirm),// Completed
                tasks.Count(x => x.Status == (byte)Enumeration.Status.Unavailabel),
            };
        }

        public int[] GetPriorityData(DateTime? startDate, DateTime? endDate, int? userId)
        {
            var tasks = unitOfWork.HiteshTaskAssignTask.GetAll();

            if (startDate.HasValue && endDate.HasValue)
            {
                tasks = tasks.Where(x => x.CreatedOn >= startDate && x.CreatedOn <= endDate);
            }

            if (userId.HasValue)
            {
                tasks = tasks.Where(x => ("," + x.AssignTo + ",").Contains("," + userId.Value + ","));
            }

            return new int[]
            {
              tasks.Count(x => x.Priority == (byte)Enumeration.Priority.Low),
              tasks.Count(x => x.Priority == (byte)Enumeration.Priority.Medium),
              tasks.Count(x => x.Priority == (byte)Enumeration.Priority.High)
            };
        }

        public List<DropDownModel> GetPriority()
        {
            throw new NotImplementedException();
        }

        public List<DropDownModel> GetStatus()
        {
            throw new NotImplementedException();
        }




        //public List<DropDownModel> GetAllCity(int id)
        //{
        //    List<DropDownModel> model = new List<DropDownModel>();
        //    try
        //    {
        //        model = (from f in unitOfWork.HiteshCity.GetAll(x=>x.SId==id)
        //                 select new DropDownModel()
        //                 {
        //                     Value = f.Id.ToString(),
        //                     Text = f.CityName,
        //                 }).ToList();

        //    }
        //    catch (Exception ex)
        //    {

        //        AppCommon.LogException(ex, "Contactprovider=>GetAllLeadId");
        //    }
        //    return model;
        //}

        //public List<DropDownModel> GetAllSlote(string Date,int id)
        //{
        //    List<DropDownModel> model = new List<DropDownModel>();
        //    try
        //    {
        //        model = (from f in unitOfWork.HiteshCourtSlotDetail.GetAll(x => x.Date.ToString() == Date && x.CourtId==id)
        //                 select new DropDownModel()
        //                 {
        //                     Value = f.Id.ToString(),
        //                     Text = f.StartTime+" "+" - "+" "+f.EndTime,
        //                 }).ToList();

        //    }
        //    catch (Exception ex)
        //    {

        //        AppCommon.LogException(ex, "Contactprovider=>GetAllLeadId");
        //    }
        //    return model;
        //}

        //public List<DropDownModel> GetSlotePrice(int SloteID)
        //{
        //    List<DropDownModel> model = new List<DropDownModel>();
        //    try
        //    {
        //        model = (from f in unitOfWork.HiteshCourtSlotDetail.GetAll(x => x.Id == SloteID)
        //                 select new DropDownModel()
        //                 {
        //                     Value = f.Id.ToString(),
        //                     Text = f.Price.ToString(),
        //                 }).ToList();

        //    }
        //    catch (Exception ex)
        //    {

        //        AppCommon.LogException(ex, "Contactprovider=>GetAllLeadId");
        //    }
        //    return model;
        //}


    }
}
