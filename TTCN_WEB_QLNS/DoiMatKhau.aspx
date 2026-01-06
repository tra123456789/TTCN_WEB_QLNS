<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoiMatKhau.aspx.cs" Inherits="TTCN_WEB_QLNS.DoiMatKhau" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Đổi Mật Khẩu</title>
    <style>
        .form-control {
            width: 100%;
            padding: 8px;
            margin-top: 5px;
            display: inline-block;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }
        .btn-primary {
            width: 100%;
            background-color: #007bff;
            color: white;
            padding: 10px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            margin-top: 10px;
        }
        .btn-primary:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
      
    <div style="width: 400px; margin: 50px auto; padding: 20px; border: 1px solid #ccc; border-radius: 10px; background-color: #f9f9f9;">
        <h2 style="text-align: center;">Đổi Mật Khẩu</h2>
        
        <div style="margin-bottom: 15px;">
            <label>Mật khẩu cũ:</label>
            <asp:TextBox ID="txtOldPass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>

        <div style="margin-bottom: 15px;">
            <label>Mật khẩu mới:</label>
            <asp:TextBox ID="txtNewPass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>

        <div style="margin-bottom: 15px;">
            <label>Xác nhận mật khẩu mới:</label>
            <asp:TextBox ID="txtConfirmPass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>

        <div style="text-align: center;">
            <asp:Button ID="btnChangePass" runat="server" Text="Cập nhật" OnClick="btnChangePass_Click" CssClass="btn btn-primary" />
            <br />
            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
        </div>
    </div>

    </form>
</body>
</html>
