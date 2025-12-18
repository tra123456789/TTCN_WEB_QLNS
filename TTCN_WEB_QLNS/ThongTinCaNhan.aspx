<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThongTinCaNhan.aspx.cs" Inherits="TTCN_WEB_QLNS.ThongTinCaNhan" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Chi tiết nhân viên</title>
<style>
  body {
            font-family: Arial, Helvetica, sans-serif;
            background: #f7f8fb;
            margin: 20px;
        }
        .card {
            max-width: 800px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            padding: 20px;
        }
        .card h2 {
            margin-top: 0;
            margin-bottom: 12px;
            font-size: 22px;
            color: #333;
        }
        table.info {
            width: 100%;
            border-collapse: collapse;
        }
        table.info td {
            padding: 8px 6px;
            vertical-align: top;
        }
        table.info td.label {
            width: 180px;
            font-weight: bold;
            color: #555;
        }
        .actions {
            margin-top: 16px;
            text-align: right;
        }
        .btn {
            display: inline-block;
            padding: 8px 14px;
            border-radius: 6px;
            text-decoration: none;
            border: none;
            cursor: pointer;
            font-size: 14px;
        }
        .btn-back {
            background: #e9eef8;
            color: #2a4a9a;
        }
        .note {
            color: #888;
            font-size: 13px;
            margin-bottom: 10px;
        }
        .welcome{
            float: right;
        }
    .auto-style1 {
        height: 38px;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
          <div class="card">
     <h3>Thông tin nhân viên</h3>
               <div class="welcome">
     <asp:Label ID="lblWelcome" runat="server" />
 </div>
<table class="info">

<tr>
    <td class="label">Họ tên:</td>
    <td><asp:TextBox ID="txtHoTen" runat="server" /></td>
</tr>

<tr>
    <td class="label">Ngày sinh:</td>
    <td><asp:TextBox ID="txtNgaySinh" runat="server" TextMode="Date" />
  

      </td>
</tr>

<tr>
    <td class="label" style="height: 38px">Giới tính:</td>
    <td class="auto-style1">
        <asp:DropDownList ID="ddlGioiTinh" runat="server">
            <asp:ListItem Value="true">Nam</asp:ListItem>
            <asp:ListItem Value="false">Nữ</asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>

<tr>
    <td class="label">Địa chỉ:</td>
    <td><asp:TextBox ID="txtDiaChi" runat="server" /></td>
</tr>

<tr>
    <td class="label">Chức vụ:</td>
    <td><asp:DropDownList ID="ddlChucVu" runat="server" /></td>
</tr>

<tr>
    <td class="label" style="height: 38px">Bộ phận:</td>
    <td class="auto-style1"><asp:DropDownList ID="ddlBoPhan" runat="server" /></td>
</tr>
 <tr> 
        <td class="label">Hình ảnh:</td>
        <td>
            <div id="avatarBox" runat="server"
                 style="width:150px;height:150px;
                 border-radius:50%;
                 background-size:cover;
                 background-position:center;
                 border:1px solid #ccc">
            </div>

            <asp:FileUpload ID="fuAvatar" runat="server" />
            <asp:Button ID="btnPreview" runat="server"
                Text="Xem ảnh"
                OnClick="btnPreview_Click" />
        </td>
    </tr> 
<tr>
    <td class="label">Trạng thái:</td>
    <td>
        <asp:DropDownList ID="ddlTrangThai" runat="server">
            <asp:ListItem Value="1">Đang làm</asp:ListItem>
            <asp:ListItem Value="0">Đã nghỉ</asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>
   
</table>
               
                <div style="width:180px; text-align:center">
      </div>
<asp:Button ID="btnSave" runat="server"
    Text="💾 Lưu thay đổi"
    CssClass="btn btn-back"
    OnClick="btnSave_Click" 
    Visible="false"/>


             
              <asp:Button ID="btnEditUser" runat="server"
    Text="✏ Chỉnh sửa thông tin"
    CssClass="btn btn-back"
    OnClick="btnEditUser_Click"
    Visible="false" />

              <asp:Button ID="btnBack" runat="server"
    Text="← Quay về Trang Chủ"
    CssClass="btn btn-back"
    OnClick="btnBack_Click" />
       </div>
            
    </form>
  </body>
</html>
