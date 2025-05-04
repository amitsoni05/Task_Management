using TaskManagementSystem.Common.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementSystem.Common.Utility
{
    public class Enumeration
    {
        public enum Role
        {
            Admin = 1, 
            User=2,
            

        }
        public enum Status
        {
            Unavailabel = 1,
            Confirm = 2,         
            Pending = 3,
            InProgresss=4,

        }

        public enum Category
        {
            Music=1,
            Holi_Celebration=2,
            Marriage_Event=3,
            Cricket_Tournament=4,

        }

        public enum Priority 
        { 
            Low=1,
            Medium=2,
            High=3,
                
        }



    }
}
