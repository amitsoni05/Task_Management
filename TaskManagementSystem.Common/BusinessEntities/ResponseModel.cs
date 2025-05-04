using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementSystem.Common.BusinessEntities
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string HeaderMessage { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public int Rid { get; set; }

    }
}
