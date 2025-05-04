using TaskManagementSystem.Common.CommonEntities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace TaskManagementSystem.Common.Utility
{
    public class AppCommon
    {

        #region Variables
        public static string ErrorMessage = "Something Went Wrong. Please Contact Administrator!";
        public static string ApplicationLongTitle = "St Anthony's Rehabilitation and Nursing Center LLC";
        public static string ApplicationTitle = "SANRC";
        public static string ForiegnKeyErrorMessage = "Child Record Exists!";
        public static string ConnectionString = "";
        public static string TextLine_Token = "sannz2pdsQN3PyG8byMf";
        public static string TextLine_Api = "https://application.textline.com/api/conversations.json";
        public static string Country_Code = "";
        public static string DateFormat = "MM/dd/yyyy";
        public static string DateTimeFormat = "MM/dd/yyyy hh:mm tt";
        public static string Application_URL = "";
        public static string NumberFormat = "00000";
        public static int MaxShift = 10;
        public static string FileNameSeperator = "__--__";
        public static int MaxShift_In_A_Day = 2;
        public static int Reset_Password_Link_Valid_Time = 30;
        public static int Date_Diffrent = 14;
        public static string SMTP_USERNAME = "";
        public static string SMTP_PASSWORD = "";
        public static string Appname = "StudentManagement";
        public static string AppUrl = "";
        public static string SessionName = "StudentManagement.Session";
        #endregion

        #region TempDataFiltersKeys
        public static string TMP_Searchtext = "TMP_Searchtext";
        public static string TMP_Status = "TMP_Status";
        public static string TMP_UseId = "TMP_UseId";
        #endregion

        public static DateTime CurrentDate
        {
            get
            {
                return DateTime.Now;
            }
        }
        //Logger
        public static void LogException(Exception ex, string source = "")
        {
            try
            {
                var TraceMsg = ex.StackTrace.ToString();
                var ErrorLineNo = TraceMsg.Substring(ex.StackTrace.Length - 7, 7);
                var ErrorMsg = ex.Message.ToString();
                var ErrorMsginDept = ex.InnerException;
                var Errortype = ex.GetType().ToString();
                var ErrorLocation = ex.Message.ToString();
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/LogFiles/");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine("---------------------------------------------------------------------------------------");
                    sw.WriteLine("Log date time     : " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
                    sw.WriteLine("Source			: " + source);
                    sw.WriteLine("Error line number : " + ErrorLineNo);
                    sw.WriteLine("Error message     : " + ErrorMsg + ErrorLocation);
                    sw.WriteLine("Trace message     : " + TraceMsg);
                    sw.WriteLine("Inner Exception   : " + ErrorMsginDept);
                    sw.WriteLine("----------------------------------------------------------------------------------------");
                    sw.WriteLine("\n");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (IOException)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
        public static int ConvertToInt32(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return 0;
                return Convert.ToInt32(value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static string ConvertToUpper(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Trim().ToUpper();
            else
                return value;
        }
        public static string GetFormatedZipCode(string zipCode)
        {
            if (zipCode != null && zipCode != "")
            {
                int len = zipCode.Trim().Length;
                if (zipCode != null && len > 5 && len <= 9)
                {
                    int pos = len - 5;
                    return string.Format("{0}-{1}", zipCode.Substring(0, 5), zipCode.Substring(5, pos));
                }
            }
            return zipCode;
        }
        public static string RemoveFormatedZipCode(string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "");
            return value;
        }
        public static string RemoveUnderScoreZipCode(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {

                value = value.Replace("-", "").Replace(" ", "").Replace("_", "");
                value = GetFormatedZipCode(value);
            }
            return value;
        }
        public static string RemoveExtraFromPhoneNumber(string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "");
            return value;
        }
        public static string RemoveExtraFromSSN(string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "");
            return value;
        }
        public static string GetFormatedSSN(string ssn)
        {

            if (ssn != null && ssn.Trim().Length >= 9)
            {
                return string.Format("{0}-{1}-{2}", ssn.Substring(0, 3), ssn.Substring(3, 2), ssn.Substring(5));
            }
            return ssn;
        }
        public static string RemoveUnderScoreSSN(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "");
                value = GetFormatedSSN(value);
            }
            return value;
        }
        public static string GetFormatedPhoneNumber(string phone)
        {
            phone = RemoveExtraFromPhoneNumber(phone);
            if (!string.IsNullOrEmpty(phone) && phone != "" && phone.Trim().Length == 10)
                return string.Format("({0}) {1}-{2}", phone.Substring(0, 3), phone.Substring(3, 3), phone.Substring(6));
            return phone;
        }
        public static string RemoveUnderScorePhoneNumber(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "");
                value = GetFormatedPhoneNumber(value);
            }
            return value;
        }
        public static string GenerateRandomePassword(int uppercashChars, int lowercaseChars, int numbChars, int speChars)
        {
            string password = "";
            try
            {
                string capitalChars = "ABCDEFGHJKLMNPQRSTWXYZ";
                string lowerChars = "abcdefgijkmnopqrstwxyz";
                string numberChars = "123456789";
                string specialChars = "~!@#$*_";

                string Capital_Char = "";
                string Lower_Char = "";
                string Numeric_Char = "";
                string Special_Char = "";
                for (int i = 0; i < uppercashChars; i++)
                {
                    var random = new Random();
                    Capital_Char += capitalChars.ToCharArray()[random.Next(capitalChars.Length)];
                }

                for (int i = 0; i < lowercaseChars; i++)
                {
                    var random = new Random();
                    Lower_Char += lowerChars.ToCharArray()[random.Next(lowerChars.Length)];
                }

                for (int i = 0; i < numbChars; i++)
                {
                    var random = new Random();
                    Numeric_Char += numberChars.ToCharArray()[random.Next(numberChars.Length)];
                }

                for (int i = 0; i < speChars; i++)
                {
                    var random = new Random();
                    Special_Char += specialChars.ToCharArray()[random.Next(specialChars.Length)];
                }
                password = Capital_Char + Special_Char + Lower_Char + Numeric_Char;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return password;
        }

        public static bool CreateZipFile(string fileName, string filePath, List<ZipDownloadModel> files)
        {
            string zipfilePath = Path.Combine(filePath, fileName);
            if (File.Exists(zipfilePath))
                File.Delete(zipfilePath);

            // Create and open a new ZIP file
            var zip = ZipFile.Open(zipfilePath, ZipArchiveMode.Create);
            try
            {
                foreach (var file in files)
                {
                    // Add the entry for each file
                    if (File.Exists(file.FilePath))
                        zip.CreateEntryFromFile(file.FilePath, file.FileName, System.IO.Compression.CompressionLevel.Optimal);
                }
                // Dispose of the object when we are done
                zip.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                zip.Dispose();
            }
        }
        public static string FormatMoney(decimal money)
        {
            return String.Format("{0:C}", money, CultureInfo.CreateSpecificCulture("en-US"));
        }
        public static string FormatMoney(string moneyString)
        {
            decimal money;
            if (Decimal.TryParse(moneyString, out money))
                return String.Format("{0:C}", money, CultureInfo.CreateSpecificCulture("en-US"));
            else
                return moneyString;
        }
        public static string CleanFileName(string fileName)
        {
            string invalidCharsRemoved = string.Join("", fileName.Split(Path.GetInvalidFileNameChars()));
            invalidCharsRemoved = invalidCharsRemoved.Replace("(", "").Replace(")", "").Replace("'", "").Replace("''", "").Replace("+", "")
                .Replace("~", "").Replace("`", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "");
            return invalidCharsRemoved;
        }
    
    
  
        public static IEnumerable<DateTime> GetDaysBetween(DateTime start, DateTime end)
        {
            for (DateTime i = start; i < end; i = i.AddDays(1))
            {
                yield return i;
            }
        }
   
        public static string ConvertImageToBase64(string folderPath, string name)
        {
            try
            {
                string fullPath = System.IO.Path.Combine(folderPath, name);
                if (System.IO.File.Exists(fullPath))
                    return "data:image/png;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(fullPath));
                else
                    return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }


    }
}
