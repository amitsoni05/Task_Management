using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.CommonEntities;

namespace TaskManagementSystem.Provider.IProvider
{
    public interface IUserProvider
    {
        DatatablePageResponseModel<HiteshTaskProjectModel> HiteshTaskProjectList(DatatablePageRequestModel datatablePageRequestModel,SessionProviderModel sessionProviderModel);

        DatatablePageResponseModel<HiteshTaskAssignTaskModel> HiteshTaskList(DatatablePageRequestModel datatablePageRequestModel,SessionProviderModel sessionProviderModel);

        HiteshTaskAssignTaskModel StatusChange(int UserId, int id,SessionProviderModel sessionProviderModel);

        HiteshTaskUserMasterModel GetUserById (int UserId);

        HiteshTaskUserMasterModel GetTaskData(int TaskId);

        HIteshTaskMessageModel SaveMessage(HIteshTaskMessageModel message);

        List<HIteshTaskMessageModel> GetMessage(int uid,int rid);

        List<HIteshTaskMessageModel> GetmsgData(int uid);

        List<HiteshTaskAssignTaskModel> GetDateData(string date , SessionProviderModel sessionProviderModel);

        List<HiteshTaskAssignTaskModel> GetToDoCount(DateTime startDate, DateTime endDate,SessionProviderModel model);
    }
}
