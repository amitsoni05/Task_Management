using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Provider.IProvider;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.ViewComponents
{
    [ViewComponent(Name ="MenuView")]
    public class MenuViewComponent: ViewComponent
    {
        protected ICommonProvider _commonProvider;
        public ISessionManager _sessionManager = null;

        public MenuViewComponent(ISessionManager sessionManager, ICommonProvider commonProvider)
        {
            _sessionManager = sessionManager;
            _commonProvider = commonProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            TaskViewModel viewModel = new TaskViewModel();       
           
             viewModel.RoleId=_sessionManager.RoleId;

            return View(viewModel);
        }
    }
}
