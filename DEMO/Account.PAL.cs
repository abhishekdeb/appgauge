using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppGauge.Web;
/// <summary>
/// Presentation Layer for Account [Web App]. Change this According to Application Type.
/// <remarks>This is for Web Application Only. This can not be used for Desktop or Other Applications.</remarks>
/// </summary>
public class pAccount:bAccount
{
    private string _LoginPage;
    private string _LogoutPage;
    private string _DefaultPage;
    private string _LoginKeyString;

    public pAccount()
    {
        _DefaultPage = System.Configuration.ConfigurationManager.AppSettings["HomePage"];
        _LoginPage = System.Configuration.ConfigurationManager.AppSettings["LoginPage"];
        _LogoutPage = System.Configuration.ConfigurationManager.AppSettings["LogoutPage"];
        _LoginKeyString = System.Configuration.ConfigurationManager.AppSettings["LoginKeyString"];

        //Modifying Table Structure according to Application Needs.
        InitializeTableStructure(null, null, new string[] {
                                                           "accType",
                                                           "fatherName",
                                                           "motherName",
                                                           "cityId",
                                                           "pincode",
                                                           "street",
                                                           "currentAddress",
                                                           "emergencyNo",
                                                           "personalNo",
                                                           "specId"
        });
    }

    /// <summary>
    /// Sets the loggedin status.
    /// </summary>
    /// <param name="Status">if set to <c>true</c> [status].</param>
    protected void SetLoggedinStatus(bool Status = true)
    {
        if (Status == true)
        {
            if ((string)HttpContext.Current.Session[_LoginKeyString] == "N")
                HttpContext.Current.Session[_LoginKeyString] = "Y";
        }
        else
        {
            if ((string)HttpContext.Current.Session[_LoginKeyString] == "Y")
                HttpContext.Current.Session[_LoginKeyString] = "N";
        }
    }

    /// <summary>
    /// Sets this page's name.
    /// </summary>
    /// <value>
    /// This page Name.
    /// </value>
    /// <exception cref="System.Exception">This Page Name is Invalid</exception>
    /// <example>
    /// <code>
    /// ThisPage="ShowStat.aspx";
    /// </code>
    /// </example>
    public string ThisPage
    {
        get
        {
            return HttpContext.Current.Session["url"].ToString();
        }
        set
        {
            if (value == null)
                throw new Exception("This Page Name is Invalid");

            HttpContext.Current.Session["url"] = value.Trim();
        }
    }

    /// <summary>
    /// Checks if logged in.
    /// </summary>
    /// <param name="InLoginPage">if set to <c>true</c> [in login].</param>
    public void checkLoggedIn(bool InLoginPage = false)
    {
        if (InLoginPage == true)
        {
            if (HttpContext.Current.Session[_LoginKeyString].Equals("Y"))
                HttpContext.Current.Response.Redirect(_DefaultPage, true);
            return;
        }

        if (HttpContext.Current.Session[_LoginKeyString].Equals("N"))
        {
            HttpContext.Current.Session["url"] = ThisPage;
            HttpContext.Current.Response.Redirect(_LoginPage, true);
        }
    }

    /// <summary>
    /// Runs this code after successful Login.
    /// </summary>
    public override void onLogin(int UsrId)
    {
        SetLoggedinStatus();
        HttpContext.Current.Session["usr"] = UsrId.ToString();
        HttpContext.Current.Response.Redirect(HttpContext.Current.Session["url"].ToString(), true);
    }

    /// <summary>
    /// Runs this code after Logout
    /// </summary>
    public override void onLogout()
    {
        SetLoggedinStatus(false);
        HttpContext.Current.Session["url"] = _DefaultPage;
        HttpContext.Current.Response.Redirect(_LogoutPage, true);
    }

    /// <summary>
    /// Runs this coder Before Register.
    /// </summary>
    public override  void onRegister()
    {

    }

    public void onChangePassword()
    {

    }
}