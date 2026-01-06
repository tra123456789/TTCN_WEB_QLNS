<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuenMatKhau.aspx.cs" Inherits="TTCN_WEB_QLNS.QuenMatKhau" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Khôi phục mật khẩu</title>
    <style> 
        body { font-family: Arial, sans-serif; background: #f0f2f5; display: flex; justify-content: center; align-items: center; height: 100vh; margin: 0; }
        .container { width: 380px; background: white; padding: 30px; border-radius: 12px; box-shadow: 0 4px 15px rgba(0,0,0,0.1); text-align: center; }
        h2 { margin-bottom: 5px; color: #333; }
        p { font-size: 13px; color: #666; margin-bottom: 20px; }
        .input-group { text-align: left; margin-bottom: 15px; }
        .input-group label { font-size: 12px; font-weight: bold; color: #555; display: block; margin-bottom: 5px; }
        .input-group input { width: 100%; padding: 12px; border: 1px solid #ccc; border-radius: 8px; outline: none; font-size: 14px; box-sizing: border-box; }
        .btn { width: 100%; padding: 12px; background: #007bff; color: white; border: none; border-radius: 8px; cursor: pointer; font-size: 15px; font-weight: bold; transition: 0.3s; }
        .btn:hover { background: #0056d6; }
        #lblMessage { display: block; margin-top: 15px; font-size: 14px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Khôi phục mật khẩu</h2>
            <p>Xác minh danh tính để đặt mật khẩu mới</p>

            <div class="input-group">
                <label>Số điện thoại (Tài khoản)</label>
                <asp:TextBox ID="txtUsername" runat="server" Placeholder="Nhập SĐT đăng nhập"></asp:TextBox>
            </div>

            <div class="input-group">
                <label>Số CCCD</label>
                <asp:TextBox ID="txtCCCD" runat="server" Placeholder="Nhập số CCCD trong hồ sơ"></asp:TextBox>
            </div>

            <hr style="border: 0; border-top: 1px solid #eee; margin: 20px 0;" />

            <div class="input-group">
                <label>Mật khẩu mới</label>
                <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password" Placeholder="Nhập mật khẩu mới"></asp:TextBox>
            </div>

            <div class="input-group">
                <label>Xác nhận mật khẩu mới</label>
                <asp:TextBox ID="txtConfirmPass" runat="server" TextMode="Password" Placeholder="Nhập lại mật khẩu mới"></asp:TextBox>
            </div>

            <asp:Button ID="btnSubmit" runat="server" Text="Đổi mật khẩu" CssClass="btn" OnClick="btnSubmit_Click" />
            
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            
            <div style="margin-top: 20px;">
                <a href="DangNhap.aspx" style="text-decoration: none; color: #007bff; font-size: 13px;"> Quay lại Đăng nhập</a>
            </div>
        </div>
    </form>
</body>
</html>