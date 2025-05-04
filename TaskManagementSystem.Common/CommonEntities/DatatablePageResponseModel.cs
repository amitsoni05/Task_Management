using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementSystem.Common.CommonEntities
{
    public class DatatablePageResponseModel<T> where T : class
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }
        public object draw { get; set; }
    }
}
