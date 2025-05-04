using AutoMapper;
using TaskManagementSystem.Common.BusinessEntities;
using TaskManagementSystem.Repository.Models;
//using TaskManagementSystem.Common.BusinessEntities;



namespace TaskManagementSystem.Providers.Mapping
{
    public class MappingProfile : Profile
    {
     
        public MappingProfile()
        {
            CreateMap<HiteshTaskAssignTaskModel, HiteshTaskAssignTask>();
            CreateMap<HiteshTaskAssignTask, HiteshTaskAssignTaskModel>();

            CreateMap<HiteshTaskProjectModel, HiteshTaskProject>();
            CreateMap<HiteshTaskProject, HiteshTaskProjectModel>();

            CreateMap<HiteshTaskProjectAccessModel, HiteshTaskProjectAccess>();
            CreateMap<HiteshTaskProjectAccess, HiteshTaskProjectAccessModel>();

            CreateMap<HiteshTaskRoleModel, HiteshTaskRole>();           
            CreateMap<HiteshTaskRole, HiteshTaskRoleModel>();

            CreateMap<HiteshTaskUserMasterModel, HiteshTaskUserMaster>();
            CreateMap<HiteshTaskUserMaster, HiteshTaskUserMasterModel>();

          
         
           



        }
    }
}