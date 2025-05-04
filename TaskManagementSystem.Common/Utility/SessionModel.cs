using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementSystem.Common.Utility
{
    [Serializable]
    public class SessionModel
    {
        public int UserId { get; set; }

        
        public string Username { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public string CaptchaCode { get; set; }
        public string UserImage { get; set; }
        public bool IsComplaint { get; set; }
        public string CurrentVersion { get; set; }
        public string CurrentVersionDate { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int OrientationStaffId { get; internal set; }
        public int[] UserRoleIds { get; set; }
    }
}
