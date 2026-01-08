<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DangNhap.aspx.cs" Inherits="TTCN_WEB_QLNS.DangNhap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Đăng Nhập</title>
    <link href ="Login.css" rel ="stylesheet" type ="text/css"/>
    <style type="text/css">
     .auto-style1 {
         width: 350px;
         background: white;
         padding: 30px;
         border-radius: 12px;
         box-shadow: 0px 10px 25px rgba(0,0,0,0.15);
         height: 377px;
     }
 </style>
</head>
<body class ="page-login">
    <form id="form1" runat="server">
        <div class="auto-style1" style="position: relative;">
           
    <a href="Default.aspx" style="position: absolute; top: 10px; right: 15px; text-decoration: none; color: #999; font-size: 20px;">&times;</a>

        <h2>Đăng nhập nhân viên</h2>

        <label>Tên đăng nhập:</label>
        <asp:TextBox ID="txtUsername" runat="server" OnTextChanged="txtUsername_TextChanged"></asp:TextBox>

        <label>Mật khẩu:</label>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" OnTextChanged="txtPassword_TextChanged"></asp:TextBox>

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            <div class ="btn-box">
        <asp:Button ID="btnLogin" runat="server" CssClass="cssbtnDN" Text="Đăng nhập" OnClick="btnLogin_Click" Width="151px" />
        <asp:Button ID="btnDangKy" runat="server" CssClass="cssbtnDK" Text="Đăng Ký" OnClick="btnDangKy_Click" Width="151px" />
              
       </div>
            <asp:HyperLink ID="HLQMK" runat="server" NavigateUrl="~/QuenMatKhau.aspx">Quên mật khẩu</asp:HyperLink>

            </div>
      
    </form>
</body>
</html>
