using System;
using AppGauge.Data;

/// <summary>
/// DAL Account, 
/// </summary>
/// 
namespace AppGauge.Web
{
    public class dAccount : BaseRecord
    {
        private string[] cols ={
                           "slid",
                           "usrPass",
                           "emailId",
                           "secQ",
                           "secA",
                           "fullName",
                           "dob",
                           "gender",
                           "doj",
                           "regIP",
                           "accLock"                           
                          };

        /// <summary>
        /// Initializes a new instance of the <see cref="dAccount"/> class.
        /// </summary>
        public dAccount()
            : base(System.Configuration.ConfigurationManager.ConnectionStrings["default"].ToString())
        {
            //Setting up Table
            Table = "tblUsr";
            TKey = cols[0].ToString();
            Columns = cols;
        }

        /*
        public bool CreateAccountTable()
        {

            return true;
        }

        public bool CleanAccountTable()
        {
            return true;
        }

        public bool DeleteAccountTable()
        {
            return true;
        }

        public bool DownloadAccountTable()
        {
            return true;
        }

          */
    }
}