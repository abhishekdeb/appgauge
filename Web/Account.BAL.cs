using System;
using System.Web;
using System.Data;
using System.Security.Cryptography;

namespace AppGauge.Web
{
    /// <summary>
    /// Authentication Class is used for Web Page Authentication.
    /// </summary>
    public class bAccount
    {

        private int _UsrID; // int
        private string _UsrPass; // Byte
        private string _EmailId;
  

        private dAccount ac;


        public int UsrID
        {
            get
            {
                return this._UsrID;

            }
            set
            {
                if (value != 0)
                    this._UsrID = value;
            }
        }

        public string EmailId
        {
            get
            {
                return this._EmailId;
            }
            set
            {
                if (value != null && value.Length > 5 && value.Length < 30)
                    this._EmailId = value;
            }
        }

        public string UsrPass
        {
            get
            {
                return this._UsrPass;
            }
            set
            {
                if (value != null && value.Length > 5 && value.Length < 40)
                    this._UsrPass = value;
            }
        }


        public bAccount()
        {
               
            ac = new dAccount();
        }

        //Following 3 methods must be overrided in child class
        /// <summary>
        /// Executes after Login is Successfull.
        /// </summary>
        /// <remarks>Override this in pAccount (Account.PAL.cs) i.e. the Presentation Layer of Account</remarks>
        /// <param name="Uid">The uid.</param>
        public virtual void onLogin(int Uid){}
        /// <summary>
        /// Executes after Logout.
        /// </summary>
        /// <remarks>Override this in pAccount (Account.PAL.cs) i.e. the Presentation Layer of Account</remarks>
        public virtual void onLogout() { }
        /// <summary>
        /// Executes before Registration.
        /// </summary>
        /// <remarks>Override this in pAccount (Account.PAL.cs) i.e. the Presentation Layer of Account</remarks>
        public virtual void onRegister() { }


        /// <summary>
        /// Logins the specified user name.
        /// </summary>
        /// <param name="UserName">Name of the user.</param>
        /// <param name="Password">The password.</param>
        /// <returns>Executes OnLogin Method from pAccount Class on successfull Login. Else, Returns Error Code.</returns>
        public int Login(string UserName, string Password)
        {
            UsrID = 0;
            EmailId = UserName;
            UsrID = GetUsrIdFromEmail();
            if (UsrID < 1) return -1002;
            if (SubLogin(Password) > 0)
                onLogin(UsrID);
            return 1;
        }

        /// <summary>
        /// Sub Login Helps you where you need extra protection, such as, requiring password while editing securty settings.
        /// </summary>
        /// <param name="Password">The password.</param>
        /// <remarks>It is also used by the main Login Method.</remarks>
        /// <returns>1 if OK else, Negative No.</returns>
        public int SubLogin(string Password)
        {
            string pass1, pass2;
            pass1 = GetStoredPassword();
            pass2 = byte2string(GetHash(EmailId, Password));
            if (pass1 == pass2)
            {
                return 1;
            }
            else
            {
                return -1001;
            }
        }

        /// <summary>
        /// Logouts this instance. Runs the OnLogout Method from class pAccount.
        /// </summary>
        public void Logout()
        {
            onLogout();
        }

        /// <summary>
        /// Registers a new account
        /// </summary>
        /// <param name="Values">Array of Columns Values In Order</param>
        /// <returns>Generated UserId.</returns>
        /// <remarks>This UserId is not to be used as Login Id.</remarks>
        /// <value>Specified Values must be given in the order of the columns in the table without the Primary Key</value>
        /// <example>Register(new string[]{"pass123","asd@asd.sd","..."});</example>
        public int Register(string[] Values)
        {
            EmailId = Values[1];
            if (checkUser())
            {
                onRegister();
                return ac.Insert(Values);
            }
            else
                return -1;
        }

        /// <summary>
        /// Matches the passwords. Get the HASH values of the passwords first by GetHash Method
        /// </summary>
        /// <param name="p1">First Password in Byte[]</param>
        /// <param name="p2">Second Password in Byte[]</param>
        /// <returns>Returns True if Passwords Match,else, False</returns>
        /// 
        private static bool MatchPass(byte[] p1, byte[] p2)
        {
            bool result = false;
            if (p1 != null && p2 != null)
            {
                if (p1.Length == p2.Length)
                {
                    result = true;
                    for (int i = 0; i < p1.Length; i++)
                    {
                        if (p1[i] != p2[i])
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the hash value from UserID and Password.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <param name="Password">The password.</param>
        /// <returns>Byte[] Hash Value</returns>
        /// <remarks>Use VARBINARY (20) Column in SQL SERVER Database to stroe this value</remarks>
        public byte[] GetHash(string UserName, string Password)
        {
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(System.Text.Encoding.ASCII.GetBytes(UserName + Password));
        }

        /// <summary>
        /// Checks if user is in the database or not.
        /// </summary>
        /// <param name="CheckId">if set to <c>true</c> [check id], It will search for UserID, else, UserName.</param>
        /// <returns>
        /// True of User is in the DB, else, fasle
        /// </returns>
        public bool checkUser()
        {
            DataTable dt = new DataTable();
            dt = ac.Search(UsrID.ToString(), new string[] { ac.TKey });
            if (dt.Rows.Count < 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Gets the usr id from email.
        /// </summary>
        /// <returns></returns>
        public int GetUsrIdFromEmail()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt = ac.Search(EmailId, new string[] { ac.Columns[2] });
            return int.Parse(dt.Rows[0][0].ToString());
        }

        /// <summary>
        /// Gets the stored password of the current user.
        /// </summary>
        /// <returns>Password as String</returns>
        public string GetStoredPassword()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt = (DataTable)ac.CustomView(UsrID.ToString(), new string[] { ac.Columns[1] });
            return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// Gets all the user details.
        /// </summary>
        /// <returns>DataTable Contaning all UserDetails</returns>
        public DataTable GetUserDetails()
        {
            if (UsrID > 0)
                return (DataTable)ac.View(UsrID.ToString());
            else
                return null;
        }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="NewPassword">The new password.</param>
        /// <returns>True if OK</returns>
        public bool SetPassword(string NewPassword)
        {
            if (NewPassword != null && NewPassword.Length < 40)
            {
                DataTable dt = new DataTable();
                dt = GetUserDetails();
                string newPass = byte2string(GetHash(dt.Rows[0][1].ToString(), NewPassword)); // Creating New Hash Password
                return ac.CustomUpdate(UsrID.ToString(), new string[] { ac.Columns[1] }, new string[] { newPass });
            }
            else
                return false;
        }

        /// <summary>
        /// Converts Byte Array to String.
        /// </summary>
        /// <param name="b">The byte Array</param>
        /// <returns>String format of Byte Array</returns>
        public string byte2string(byte[] b)
        {
            string s = System.String.Empty;
            for (int i = 0; i < b.Length; i++)
                s += b[i].ToString();

            return s;
        }

        /// <summary>
        /// Modifies the table structure further.
        /// </summary>
        /// <param name="TableName">Name of the table. Default = tblUsr</param>
        /// <param name="TKey">The Table Primary key. Default = slid</param>
        /// <param name="MoreColumns">More columns that will be appended to existing Column. </param>
        public void InitializeTableStructure(string TableName = null, string TKey = null, string[] MoreColumns = null)
        {
            if (TableName != null)
                ac.Table = TableName;

            if (TKey != null)
                ac.TKey = TKey; ;

            if (MoreColumns != null)
                ac.AppendColumns(MoreColumns);
        }
    }
}
