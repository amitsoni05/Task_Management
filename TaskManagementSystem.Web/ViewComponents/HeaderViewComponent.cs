using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Provider.IProvider;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.ViewComponents
{
    [ViewComponent(Name ="HeaderView")]
    public class HeaderViewComponent: ViewComponent
    {
        protected ICommonProvider _commonProvider;
        public ISessionManager _sessionManager = null;
        IUserProvider _userProvider;
        public HeaderViewComponent(ISessionManager sessionManager, ICommonProvider commonProvider,IUserProvider userProvider)
        {
            _sessionManager = sessionManager;
            _commonProvider = commonProvider;
            _userProvider = userProvider;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            TaskViewModel viewModel = new TaskViewModel();
            viewModel.HiteshTaskUserMasterModel = _userProvider.GetUserById(_sessionManager.UserId);
            viewModel.hIteshTaskMessageModelsList = _userProvider.GetmsgData(_sessionManager.UserId);
            viewModel.HiteshTaskUserMasterModel.RoleId = _sessionManager.RoleId;

            return View(viewModel);
        }


    }
}
