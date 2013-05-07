using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppGauge.Debug;

public partial class Login : System.Web.UI.Page
{
    pAccount ac = new pAccount();

    protected void Page_Load(object sender, EventArgs e)
    {
        ac.checkLoggedIn(true);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Label1.Text = ac.Login(TextBox1.Text.Trim(), TextBox2.Text.Trim()).ToString();
    }
}