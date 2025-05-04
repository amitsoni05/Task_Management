using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManagementSystem.Common.BusinessEntities;

namespace TaskManagementSystem.Provider.IProvider
{
    public interface ICommonProvider
    {
        #region Encrypt Properties

        /// <summary>
        /// For Encrypt Id
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string Protect(int value);

        /// <summary>
        /// Protect Long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string ProtectLong(long value);

        /// <summary>
        /// Protect Short
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string ProtectShort(short value);

        /// <summary>
        /// For Decrypt string Id
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int UnProtect(string value);

        /// <summary>
        ///  For Encrypt Id
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string ProtectString(string value);

        /// <summary>
        /// For Decrypt string Id
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string UnProtectString(string value);

        /// <summary>
        /// UnProtect Long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        long UnProtectLong(string value);

        #endregion
        List<DropDownModel> GetAllProject();

        List<DropDownModel> GetAllUser(int id);

        List<DropDownModel> GetPriority();
        List<DropDownModel> GetStatus();

        List<DropDownModel> GetUser();

        List<DropDownModel> GetSelectedUser(int PId);

        int[] GetStatusData(DateTime? startDate, DateTime? endDate, int? userId);
        int[] GetPriorityData(DateTime? startDate, DateTime? endDate, int? userId);
        //List<DropDownModel> GetAllStates();

        //List<DropDownModel> GetAllCity(int id);

        //ResponseModel ChangeOrResetPassword(HiteshCricketUserMasterModel userData, bool isChangePwd);
        //List<DropDownModel> GetAllSlote(string Date,int id);

        //List<DropDownModel> GetSlotePrice(int SloteID);
    }
}
