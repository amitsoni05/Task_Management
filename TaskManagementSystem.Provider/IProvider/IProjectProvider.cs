using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.CommonEntities;

namespace TaskManagementSystem.Provider.IProvider
{
    public interface IProjectProvider
    {
        DatatablePageResponseModel<HiteshTaskProjectModel> HiteshTaskProjectList(DatatablePageRequestModel datatablePageRequestModel);

        ResponseModel SaveProject(HiteshTaskProjectModel projectdata, SessionProviderModel sessionProviderModel);

        HiteshTaskProjectModel GetProjectById(int id);

        ResponseModel DeleteProject(int id);

        List<HiteshTaskUserMasterModel> GetUserMasterList(int id);

        ResponseModel DocumentSave(HiteshTaskDocumentSaveModel model);

        List<HiteshTaskDocumentSaveModel> GetDocumentList(int id);

        HiteshTaskDocumentSaveModel GetDocId(int id);

        ResponseModel DeleteDocData(int id);

    }
}
