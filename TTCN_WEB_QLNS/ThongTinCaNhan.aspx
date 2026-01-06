<%@ Page Language="C#" AutoEventWireup="true"   MasterPageFile="~/Site.Master" CodeBehind="ThongTinCaNhan.aspx.cs" Inherits="TTCN_WEB_QLNS.ThongTinCaNhan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">

   
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
    .info-container {
    display: flex;
    gap: 30px; 
    align-items: flex-start;
}
.column-left {
    flex: 1;
}
.column-right {
    flex: 1;
    background: #fdfdfd;
    padding: 15px;
    border-radius: 8px;
    border: 1px dashed #ddd;
}
    .auto-style1 {
        width: 107px;
    }
</style>


   <div class="card">
    <h3>Thông tin nhân viên</h3>
    <div class="welcome">
        <asp:Label ID="lblWelcome" runat="server" />
    </div>

    <div class="info-container">
        
        <div class="column-left">
            <table class="info">
                <tr>
                    <td class="label">Họ tên:</td>
                    <td><asp:TextBox ID="txtHoTen" runat="server" Width="100%" /></td>
                </tr>
                <tr>
                    <td class="label">Ngày sinh:</td>
                    <td><asp:TextBox ID="txtNgaySinh" runat="server" TextMode="Date" Width="100%" /></td>
                </tr>
                <tr>
                    <td class="label">Giới tính:</td>
                    <td>
                        <asp:DropDownList ID="ddlGioiTinh" runat="server" Width="100%">
                            <asp:ListItem Value="true">Nam</asp:ListItem>
                            <asp:ListItem Value="false">Nữ</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="label">Địa chỉ:</td>
                    <td><asp:TextBox ID="txtDiaChi" runat="server" Width="100%" /></td>
                </tr>
                <tr>
                    <td class="label">Chức vụ:</td>
                    <td><asp:DropDownList ID="ddlChucVu" runat="server" Width="100%" /></td>
                </tr>
                <tr>
                <td class="label">Trình độ:</td>
                <td>
                    <asp:DropDownList ID="ddlTrinhDo" runat="server" Width="100%" CssClass="form-control">
                    </asp:DropDownList>
                </td>
            </tr>
                <tr>
                    <td class="label">Bộ phận:</td>
                    <td><asp:DropDownList ID="ddlBoPhan" runat="server" Width="100%" /></td>
                </tr>
                <tr>
                    <td class="label">Trạng thái:</td>
                    <td>
                        <asp:DropDownList ID="ddlTrangThai" runat="server" Width="100%">
                            <asp:ListItem Value="1">Đang làm</asp:ListItem>
                            <asp:ListItem Value="0">Đã nghỉ</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
       
<div id="divPhuCap" runat="server" class="column-right">
        <h4>Chế độ Phụ cấp</h4>
        <table class="info">
            <tr><td class="label">Trách nhiệm:</td><td><asp:TextBox ID="txtPCTrachNhiem" runat="server" Text="0" Width="100%" /></td></tr>
            <tr><td class="label">Độc hại:</td><td><asp:TextBox ID="txtPCDocHai" runat="server" Text="0" Width="100%" /></td></tr>
            <tr><td class="label">Khác:</td><td><asp:TextBox ID="txtPCKhac" runat="server" Text="0" Width="100%" /></td></tr>
        </table>
        </div> </div>
                <h4>Hình ảnh đại diện</h4>
        <div id="avatarBox" runat="server"
             style="width:120px;height:120px; background-size:cover; background-position:center; border:1px solid #ccc;">
        </div>
        <asp:FileUpload ID="fuAvatar" runat="server" Width="24%" />
        <div style=" margin-top:5px;" class="auto-style1">
            <asp:Button ID="btnPreview" runat="server" Text="Xem ảnh" OnClick="btnPreview_Click" CssClass="btn" style="font-size:12px;" />
        </div>
    
    <div class="actions">
        <asp:Button ID="btnSave" runat="server" Text="💾 Lưu thay đổi" CssClass="btn btn-back" OnClick="btnSave_Click" Visible="false"/>
        <asp:Button ID="btnEditUser" runat="server" Text="✏ Chỉnh sửa" CssClass="btn btn-back" OnClick="btnEditUser_Click" Visible="false" />
        <asp:Button ID="btnBack" runat="server" Text="← Quay về" CssClass="btn btn-back" OnClick="btnBack_Click" />
    </div>
</div>
   
      </div>
</asp:Content>
