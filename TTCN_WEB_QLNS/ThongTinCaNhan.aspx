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
</style>
</head>
<body>
    <form id="form1" runat="server">
          <div class="card">
            <h2>Thông tin nhân viên</h2>
            <div class ="welcome">
                <asp:Label ID="lblWelcome" runat="server" CssClass="font-medium text-sm"></asp:Label>
   </div>

            <table class="info">
                <tr>
                    <td class="label">Mã NV:</td>
                    <td><asp:Label ID="lblMaNV" runat="server" Text="-" /></td>
                </tr>
                <tr>
                    <td class="label">Họ và tên:</td>
                    <td><asp:Label ID="lblHoTen" runat="server" Text="-" /></td>
                </tr>
                <tr>
                    <td class="label">Ngày sinh:</td>
                    <td><asp:Label ID="lblNgaySinh" runat="server" Text="-" /></td>
                </tr>
                <tr>
                    <td class="label">Giới tính:</td>
                    <td><asp:Label ID="lblGioiTinh" runat="server" Text="-" /></td>
                </tr>
                <tr>
                    <td class="label">Địa chỉ:</td>
                    <td><asp:Label ID="lblDiaChi" runat="server" Text="-" /></td>
                </tr>
                <tr>
                    <td class="label">Chức vụ:</td>
                    <td><asp:Label ID="lblChucVu" runat="server" Text="-" /></td>
                </tr>
                <tr>
                    <td class="label">Phòng ban:</td>
                    <td><asp:Label ID="lblPhongBan" runat="server" Text="-" /></td>
                </tr>
                <tr>
                    <td class="label">Số bảo hiểm:</td>
                    <td><asp:Label ID="lblSoBH" runat="server" Text="-" /></td>
                </tr>
            </table>
              <asp:Panel ID="pnlEdit" runat="server" Visible="false">
    <hr />
    <h3>Chỉnh sửa thông tin cá nhân</h3>
    <p class="note">Bạn chỉ có thể chỉnh sửa trong thời gian được phép</p>

    <table class="info">
        <tr>
            <td class="label">Họ và tên:</td>
            <td>
                <asp:TextBox ID="txtEditHoTen" runat="server" Width="100%" />
            </td>
        </tr>

        <tr>
            <td class="label">Ngày sinh:</td>
            <td>
                <asp:TextBox ID="txtEditNgaySinh" runat="server" TextMode="Date" />
            </td>
        </tr>

        <tr>
            <td class="label">Giới tính:</td>
            <td>
                <asp:DropDownList ID="ddlEditGioiTinh" runat="server">
                    <asp:ListItem Value="true">Nam</asp:ListItem>
                    <asp:ListItem Value="false">Nữ</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td class="label">Địa chỉ:</td>
            <td>
                <asp:TextBox ID="txtEditDiaChi" runat="server" Width="100%" />
            </td>
        </tr>
    </table>

    <div class="actions">
        <asp:Button ID="btnSaveEdit" runat="server"
            Text="💾 Lưu thay đổi"
            CssClass="btn btn-back"
            OnClick="btnSaveEdit_Click" />
    </div>
</asp:Panel>


            <div class="actions">
               <asp:Button ID="btnEdit" runat="server"
    Text="✏ Chỉnh sửa thông tin"
    CssClass="btn btn-back"
    OnClick="btnEdit_Click"
    Visible="false" />

                <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="UserHome.aspx" CssClass="btn btn-back" Height="22px">← Quay về Trang Chủ</asp:HyperLink>
            </div>
            </div>
    </form>
  </body>
</html>
