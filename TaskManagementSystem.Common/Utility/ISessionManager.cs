using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementSystem.Common.Utility
{
    public interface ISessionManager
    {
        int UserId { get; set; }

       
        int OrientationStaffId { get; set; }
        string Username { get; set; }
        string FullName { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        int RoleId { get; set; }
        string RoleName { get; set; }
        string CaptchaCode { get; set; }
        string UserImage { get; set; }
        bool IsNursing { get; set; }
        string CurrentVersion { get; set; }
        string CurrentVersionDate { get; set; }
        int OrganizationId { get; set; }
        string OrganizationName { get; set; }
        int[] UserRoleIds { get; set; }
        string GetSessionId();
        void ClearSession();
        string GetIP();
    }
}