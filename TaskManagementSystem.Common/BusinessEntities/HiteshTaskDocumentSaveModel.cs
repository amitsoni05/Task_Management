using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TaskManagementSystem.Common.BusinessEntities
{
    public class HiteshTaskDocumentSaveModel:ResponseModel
    {
        public int Id { get; set; }

        public string? DocumentName { get; set; }

        public List<IFormFile> Files { get; set; }

        public List<string> DocName { get; set; }

        public int? ProjectId { get; set; }
        public string DocumentType { get; set; }
        public int? TaskId { get; set; }
    }
}
