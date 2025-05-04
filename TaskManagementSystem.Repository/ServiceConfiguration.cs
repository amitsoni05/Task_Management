using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Common.Utility;

//using CricketCourtBookingSystem.Repository.Models;



namespace TaskManagementSystem.Repository
{
    public static class ServicesConfiguration
    {
        public static void AddRepositoryService(this IServiceCollection services, IConfiguration configuration)
        {
            AppCommon.ConnectionString = configuration.GetConnectionString("DefaultConnection");
            AppCommon.Application_URL = configuration.GetSection("AppCommonSettings:Application_URL").Value;
            //AppCommon.MaxShift = configuration.GetValue<int>("AppCommonSettings:MaxShift");
            //AppCommon.MaxShift_In_A_Day = configuration.GetValue<int>("AppCommonSettings:MaxShift_In_A_Day");
            //AppCommon.Reset_Password_Link_Valid_Time = configuration.GetValue<int>("AppCommonSettings:Reset_Password_Link_Valid_Time");
            //AppCommon.Date_Diffrent = configuration.GetValue<int>("AppCommonSettings:Date_Diffrent");
            //AppCommon.Org_Name = configuration.GetValue<string>("AppCommonSettings:Org_Name");
            //AppCommon.Org_Address = configuration.GetValue<string>("AppCommonSettings:Org_Address");
            //AppCommon.Org_City = configuration.GetValue<string>("AppCommonSettings:Org_City");
            //AppCommon.Org_State = configuration.GetValue<string>("AppCommonSettings:Org_State");
            //AppCommon.Org_Zipcode = configuration.GetValue<string>("AppCommonSettings:Org_Zipcode");
            //AppCommon.Org_Email = configuration.GetValue<string>("AppCommonSettings:Org_Email");
            //AppCommon.Org_Phone = configuration.GetValue<string>("AppCommonSettings:Org_Phone");
            //AppCommon.Org_Title_Of_Representative = configuration.GetValue<string>("AppCommonSettings:Org_Title_Of_Representative");
            //AppCommon.Document_Completion_To_Email_Address = configuration.GetValue<string>("AppCommonSettings:Document_Completion_To_Email_Address");
            //AppCommon.Document_Completion_CC_Email_Address = configuration.GetValue<string>("AppCommonSettings:Document_Completion_CC_Email_Address");
            //AppCommon.BG_Check_To_Email = configuration.GetValue<string>("AppCommonSettings:BG_Check_To_Email");
            //AppCommon.BG_Check_CC_Email = configuration.GetValue<string>("AppCommonSettings:BG_Check_CC_Email");
            AppCommon.SMTP_USERNAME = configuration.GetSection("AppCommonSettings:SMTP_USERNAME").Value;
            AppCommon.SMTP_PASSWORD = configuration.GetSection("AppCommonSettings:SMTP_PASSWORD").Value;
            //AppCommon.SMTP_USERNAME_HR = configuration.GetValue<string>("AppCommonSettings:SMTP_USERNAME_HR");
            //AppCommon.SMTP_PASSWORD_HR = configuration.GetValue<string>("AppCommonSettings:SMTP_PASSWORD_HR");
            //AppCommon.Candidate_CC_Email = configuration.GetValue<string>("AppCommonSettings:Candidate_CC_Email");
            //AppCommon.Complain_CC_Email = configuration.GetValue<string>("AppCommonSettings:Complain_CC_Email");
            //AppCommon.Complain_To_Email = configuration.GetValue<string>("AppCommonSettings:Complain_To_Email");

            services.AddDbContext<TestDbContext>(options => options.UseSqlServer(AppCommon.ConnectionString).UseLazyLoadingProxies());
        }
    }
}
