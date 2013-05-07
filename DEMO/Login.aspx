    <%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login System</title>
    <style type="text/css">
        *{padding:0;margin:0}
        body{font-family:Helvetica, Arial, 'DejaVu Sans', 'Liberation Sans', Freesans, sans-serif;
             width:960px;
             margin:0 auto;
             font-size:14px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
            <table style="margin:0 auto;">
                <caption>SECURE LOGIN</caption>
                <tr>
                    <td>User ID</td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Password</td>
                    <td>
                        <asp:TextBox ID="TextBox2" TextMode="Password" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:right;">

                        <asp:Button ID="Button1" runat="server" Text="Login" OnClick="Button1_Click" />
                    </td>
                </tr>
            </table>

    </div>
    </form>
</body>
</html>
