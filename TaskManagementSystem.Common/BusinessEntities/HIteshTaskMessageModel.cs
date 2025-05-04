using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Common.BusinessEntities
{
    public class HIteshTaskMessageModel:ResponseModel
    {
        public int Id { get; set; }

        public string? Message { get; set; }

        public int? TaskId { get; set; }

        public TimeOnly Time { get; set; }

        public DateOnly Date { get; set; }
        public List<HIteshTaskMessageModel> AllMessage { get; set; }
        public string? FullName { get; set; }
        public string ImagesUrl { get; set; }

        public int SendId { get; set; }

        public int ReceiveId { get; set; }

        public int count { get; set; }
      
    }
}
