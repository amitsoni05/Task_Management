using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementSystem.Common.CommonEntities
{
    public class SessionProviderModel
    {
        public int UserId { get; set; }
       
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Ip { get; set; }
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public bool IsFromApp { get; set; }
        public bool IsComplaint { get; set; }
        public int OrientationStaffId { get; set; }
        public int[] UserRoleIds { get; set; }
    }
}
