using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.CommonEntities;

namespace TaskManagementSystem.Provider.IProvider
{
    public interface ITaskProvider
    {
        DatatablePageResponseModel<HiteshTaskAssignTaskModel> HiteshTaskList(DatatablePageRequestModel datatablePageRequestModel);

        ResponseModel SaveTask(HiteshTaskAssignTaskModel data, SessionProviderModel sessionProviderModel);

        HiteshTaskAssignTaskModel GetTaskById(int id);

        List<HiteshTaskUserMasterModel> GetUserMasterList(int id);

        ResponseModel DeleteTask(int id);
    }
}
