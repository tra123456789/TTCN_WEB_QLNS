<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DangKy.aspx.cs" Inherits="TTCN_WEB_QLNS.DangKy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Đăng Ký</title>
    <link href="Login.css" rel="stylesheet" type="text/css"/>
    <style type="text/css">
     .auto-style1 {
         width: 350px;
         background: white;
         padding: 30px;
         border-radius: 12px;
         box-shadow: 0px 10px 25px rgba(0,0,0,0.15);
         height: 377px;
     }
        .auto-style2 {
            width: 421px;
            border-radius: 12px;
            box-shadow: 0px 10px 25px rgba(0, 0, 0, 0.15);
            position: relative;
            height: 440px;
            left: -3px;
            top: 1px;
            padding: 30px;
            background: white;
        }
 </style>
</head>
<body class ="page-register">
    <form id="form1" runat="server">
      <div class="auto-style2">
     
            <div class="form-register">
            <h2>Đăng ký nhân viên</h2>

            <label for="txtUsername">Tên đăng nhập:</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="textbox" OnTextChanged="txtUsername_TextChanged"></asp:TextBox>

            <label for="txtPassword">Mật khẩu:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="textbox" OnTextChanged="txtPassword_TextChanged"></asp:TextBox>

            <label for="txtPassword2">Nhập lại mật khẩu:</label>
            <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" CssClass="textbox" OnTextChanged="txtPassword2_TextChanged"></asp:TextBox>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="messageDK"></asp:Label>
                <div class ="btn-box">
        <asp:Button ID="btnLogin" runat="server" CssClass="cssbtnDN" Text="Đăng nhập" OnClick="btnLogin_Click" Width="151px" />
        <asp:Button ID="btnDangKy" runat="server" CssClass="cssbtnDK" Text="Đăng Ký" OnClick="btnDangKy_Click" Width="151px" />
              </div>
       </div>
        </div>
    </form>
</body>
</html>
