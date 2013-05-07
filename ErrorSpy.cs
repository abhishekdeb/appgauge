using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace AppGauge.Debug
{
    /// <summary>
    /// This class hold all error codes.
    /// 0 - Success
    /// 1 - Account
    /// 2 - DataBase
    /// 3 - Session/Cookie
    /// 4 - Security
    /// </summary>
    class ErrorSpy
    {
       private static IDictionary<int, string> e = new Dictionary<int, string>();
       
 
       public ErrorSpy()
       {
           // 1 - Account
           e.Add(1001, "Incorrect Password");
           e.Add(1002, "User Doesn't Exist");
           e.Add(1003, "User Already Exists");
           e.Add(1004, "User Suspended");
           e.Add(1005, "User still Pending");
           e.Add(1006, "Invalid User Role");
           e.Add(1007, "User Not Logged In");
           e.Add(1008, "User Already Logged In");


           // 2 - Database
           e.Add(2001, "DB Connection Failed");
           e.Add(2002, "Table doesn't Exist");
           e.Add(2003, "Column Doesn't Exist");
           e.Add(2004, "Key Doesn't Exist");
           e.Add(2005, "Invalid/Null Value Arrangement");
           e.Add(2006, "Invalid/Null Connection String");
           e.Add(2007, "Invalid/Null SQL Query");
       }


        public static string Error(int ErrorNumber)
        {
            if (e.ContainsKey(ErrorNumber))
                return e[ErrorNumber].ToString();
            else
                return "N/A";
        }
    }
}
