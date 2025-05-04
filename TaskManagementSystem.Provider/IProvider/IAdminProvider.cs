using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.CommonEntities;

namespace TaskManagementSystem.Provider.IProvider
{
    public interface IAdminProvider
    {
        DatatablePageResponseModel<HiteshTaskUserMasterModel> HiteshTaskUserMasterList(DatatablePageRequestModel datatablePageRequestModel,SessionProviderModel sessionProviderModel);

        ResponseModel SaveEmployee(HiteshTaskUserMasterModel employee,SessionProviderModel sessionProviderModel);

        HiteshTaskUserMasterModel GetEmployeeById(int id);

        HiteshTaskUserMasterModel CheckEmail(string email);

        ResponseModel DeleteEmployee(int id);
    }
}
