<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuenMatKhau.aspx.cs" Inherits="TTCN_WEB_QLNS.QuenMatKhau" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quên mật khẩu</title>
    <style> 
         body {
            font-family: Arial, sans-serif;
            background: #f0f2f5;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
        }

        .container {
            width: 350px;
            background: white;
            padding: 25px;
            border-radius: 12px;
            box-shadow: 0 0 12px rgba(0,0,0,0.1);
            text-align: center;
        }

        h2 {
            margin-bottom: 10px;
            color: #333;
        }

        p {
            font-size: 14px;
            color: #555;
        }

        .input-box {
            width: 100%;
            margin-top: 15px;
        }

        .input-box input {
            width: 100%;
            padding: 12px;
            border: 1px solid #ccc;
            border-radius: 8px;
            outline: none;
            font-size: 14px;
        }

        .btn {
            width: 100%;
            padding: 12px;
            margin-top: 20px;
            background: #007bff;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-size: 15px;
            transition: 0.3s;
        }

        .btn:hover {
            background: #0056d6;
        }

        #lblMessage {
            display: block;
            margin-top: 15px;
            font-size: 14px;
        }
    </style>
</head>
<body>
   <form id="form1" runat="server">
        <div>
            <h2>Đặt lại mật khẩu</h2>
            <p>Vui lòng nhập email của bạn để nhận hướng dẫn đặt lại mật khẩu:</p>
            <asp:TextBox ID="txtEmail" runat="server" Placeholder="Nhập Email" OnTextChanged="txtEmail_TextChanged"></asp:TextBox>
            <asp:Button ID="btnSubmit" runat="server" Text="Gửi yêu cầu" OnClick="btnSubmit_Click" />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
        </div>
    </form>
       
</body>
</html>
