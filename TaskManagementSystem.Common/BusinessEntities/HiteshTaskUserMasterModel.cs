using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TaskManagementSystem.Common.BusinessEntities
{
    public class HiteshTaskUserMasterModel:ResponseModel
    {
        public int Id { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string EncUserId { get; set; }
        public string? Password { get; set; }

        public int RoleId { get; set; }

        public bool IsEdit { get; set; }
        public string? Ip { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public IFormFile Photo { get; set; }
        public string ImagesUrl { get; set; }
        public string newpassword { get; set; }

        public string confirmpassword { get; set; }
        public string CreatedName { get; set; }
        public bool? Active { get; set; }

        public string CaptchaCode { get; set; }
        public string CaptchaImage { get; set; }

        public int? UpdateBy { get; set; }

        public int total { get; set; }

        public int success { get; set; }

        public int errorcount { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
