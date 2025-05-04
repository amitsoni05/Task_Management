
using TaskManagementSystem.Common.BusinessEntities;

namespace TaskManagementSystem.Common.CommonEntities
{
    public class AuthResultModel : ResponseModel
    {
        public int UserId { get; set; }

        
        public string Username { get; set; }
        public int RoleId { get; set; }
        //public bool IsSuccess { get; set; }
        //public string Message { get; set; }
        public string RoleName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string UserImage { get; set; }
        public string BaseURL { get; set; }
        public object Result { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        //public object Result { get; set; }
        public string Ip { get; set; }
        public string Token { get; set; }
        public string PhoneNo { get; set; }
        public string UserPassword { get; set; }
        public int[] UserRoleIds { get; set; }
        public bool IsNursing { get; set; }
    }
}
