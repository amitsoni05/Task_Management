
using RestSharp;
using System;
using System.Net;

namespace TaskManagementSystem.Common.Utility
{
    public class SMSSender
	{
		public static bool Send(string phoneNumber, string message)
		{
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

				var client = new RestSharp.RestClient(AppCommon.TextLine_Api);
				var request = new RestSharp.RestRequest(Method.POST);
				request.AddHeader("Accept", "application/json");
				request.AddHeader("X-TGP-ACCESS-TOKEN", AppCommon.TextLine_Token);
				var content = "{  \"phone_number\": \"" + phoneNumber + "\", " +

					" \"comment\": {   " +
					" \"body\": \"" + message + "\"" +
					" }, " +
					"  \"resolve\": \"1\"}";
				request.AddJsonBody(content);
				RestSharp.IRestResponse response = client.Execute(request);
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
					return true;
				else
					return false;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
