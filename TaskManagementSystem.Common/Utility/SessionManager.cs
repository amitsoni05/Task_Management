using Microsoft.AspNetCore.Http;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace TaskManagementSystem.Common.Utility
{
    public class SessionManager : ISessionManager
    {
        private string sessionKey = "SANRCSeesionKEY";
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private SessionModel SessionData { get; set; }

        public int UserId
        {
            get
            {
                return GetSession().UserId;
            }
            set
            {
                SessionData.UserId = value;
                SetSession();
            }
        }

       
        public string Username
        {
            get
            {
                return GetSession().Username;
            }
            set
            {
                SessionData.Username = value;
                SetSession();
            }

        }

        public string FullName
        {
            get
            {
                return GetSession().FullName;
            }
            set
            {
                SessionData.FullName = value;
                SetSession();
            }

        }
        public string FirstName
        {
            get
            {
                return GetSession().FirstName;
            }
            set
            {
                SessionData.FirstName = value;
                SetSession();
            }

        }
        public string LastName
        {
            get
            {
                return GetSession().LastName;
            }
            set
            {
                SessionData.LastName = value;
                SetSession();
            }

        }
       
    
        public int RoleId
        {
            get
            {
                return GetSession().RoleId;
            }
            set
            {
                SessionData.RoleId = value;
                SetSession();
            }
        }
        public string RoleName
        {
            get
            {
                return GetSession().RoleName;
            }
            set
            {
                SessionData.RoleName = value;
                SetSession();
            }
        }
       

        public string CaptchaCode
        {
            get
            {
                return GetSession().CaptchaCode;
            }
            set
            {
                SessionData.CaptchaCode = value;
                SetSession();
            }
        }

        public string UserImage
        {
            get
            {
                return GetSession().UserImage;
            }
            set
            {
                SessionData.UserImage = value;
                SetSession();
            }
        }
        public bool IsNursing
        {
            get
            {
                return GetSession().IsComplaint;
            }
            set
            {
                SessionData.IsComplaint = value;
                SetSession();
            }
        }
        public string CurrentVersion
        {
            get
            {
                return GetSession().CurrentVersion;
            }
            set
            {
                SessionData.CurrentVersion = value;
                SetSession();
            }
        }
        public string CurrentVersionDate
        {
            get
            {
                return GetSession().CurrentVersionDate;
            }
            set
            {
                SessionData.CurrentVersionDate = value;
                SetSession();
            }
        }
        public string OrganizationName
        {
            get
            {
                return GetSession().OrganizationName;
            }
            set
            {
                SessionData.OrganizationName = value;
                SetSession();
            }

        }
        public int OrganizationId
        {
            get
            {
                return GetSession().OrganizationId;
            }
            set
            {
                SessionData.OrganizationId = value;
                SetSession();
            }

        }
        public int[] UserRoleIds
        {
            get
            {
                return GetSession().UserRoleIds;
            }
            set
            {
                SessionData.UserRoleIds = value;
                SetSession();
            }

        }
        public int OrientationStaffId
        {
            get
            {
                return GetSession().OrientationStaffId;
            }
            set
            {
                SessionData.OrientationStaffId = value;
                SetSession();
            }
        }
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            SessionData = GetSession();
            if (SessionData == null)
            {
                SessionData = new SessionModel();
            }
        }


        public SessionModel GetSession()
        {
            var session = _httpContextAccessor.HttpContext.Session.Get(sessionKey);
            return (SessionModel)(session != null ? FromByteArray<SessionModel>(session) : new SessionModel());
        }
        public void SetSession()
        {
            _httpContextAccessor.HttpContext.Session.Set(sessionKey, ObjectToByteArray(SessionData));
        }

        public string GetSessionId()
        {
            return _httpContextAccessor.HttpContext.Session.Id.ToString();
        }

        public void ClearSession()
        {
            SessionData = new SessionModel();
            _httpContextAccessor.HttpContext.Session.Remove(sessionKey);
            _httpContextAccessor.HttpContext.Session.Clear();
        }
        public string GetIP()
        {
            return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        private static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            // Serialize the object to a JSON string and then convert to byte array
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }
        // Method to deserialize a byte array to an object using JSON
        private static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            // Deserialize the byte array (which contains the JSON) back to the object
            return JsonSerializer.Deserialize<T>(data);
        }
    }
}
