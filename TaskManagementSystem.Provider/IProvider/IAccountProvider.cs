using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.BusinessEntities;

namespace TaskManagementSystem.Provider.IProvider
{
    public interface IAccountProvider
    {
        HiteshTaskUserMasterModel CheckLogin(HiteshTaskUserMasterModel master);

        HiteshTaskUserMasterModel CheckPassword(HiteshTaskUserMasterModel master);

        ResponseModel UpdateProfile(HiteshTaskUserMasterModel master);

         ResponseModel CheckEmail(HiteshTaskUserMasterModel master);

        ResponseModel ChangeOrResetPassword(HiteshTaskUserMasterModel master);
    }
}
