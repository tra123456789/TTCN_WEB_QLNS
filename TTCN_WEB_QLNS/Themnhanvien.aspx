<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Themnhanvien.aspx.cs" Inherits="TTCN_WEB_QLNS.Themnhanvien" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <title>Thêm Nhân Viên </title>
    <link href="Themkhenthuong.css" rel="stylesheet" />
</head>
<body>
<form id="form1" runat="server" enctype="multipart/form-data">
    <div class="content">
        
    Họ Tên:
    <asp:TextBox ID="txtHoTen" runat="server" OnTextChanged="txtHoTen_TextChanged" />

    Ngày sinh:
    <asp:TextBox ID="txtNgaySinh" runat="server" TextMode="Date" OnTextChanged="txtNgaySinh_TextChanged" />

    SĐT:
    <asp:TextBox ID="txtSDT" runat="server" OnTextChanged="txtSDT_TextChanged" />

    CCCD:
    <asp:TextBox ID="txtCCCD" runat="server" OnTextChanged="txtCCCD_TextChanged" />

    Địa chỉ:
    <asp:TextBox ID="txtDiaChi" runat="server" OnTextChanged="txtDiaChi_TextChanged" />

    Hình ảnh:
        <br/>
    <asp:FileUpload ID="fileAvatar" runat="server" />
        <br/> 

    Giới tính:
<asp:RadioButtonList ID="rblGioiTinh" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblGioiTinh_SelectedIndexChanged">
    <asp:ListItem Text="Nam" Value="1" Selected="True" />
    <asp:ListItem Text="Nữ" Value="0" />
</asp:RadioButtonList>
        Chức vụ:
        <asp:DropDownList ID="ddlChucVu" runat="server" OnSelectedIndexChanged="ddlChucVu_SelectedIndexChanged"></asp:DropDownList>
        Trình độ:
        <asp:DropDownList ID="ddlTrinhDo" runat="server" OnSelectedIndexChanged="ddlTrinhDo_SelectedIndexChanged"></asp:DropDownList>
        Bộ phận:
        <asp:DropDownList ID="ddlBoPhan" runat="server" OnSelectedIndexChanged="ddlBoPhan_SelectedIndexChanged"></asp:DropDownList>

        <asp:Button ID="btnSave" runat="server"
    Text="💾 Lưu nhân viên"
    CssClass="btn btn-success"
    OnClick="btnSave_Click" />
 

                <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="QuanlyUser.aspx" CssClass="btn btn-back" Height="22px">← Quay về Trang Chủ</asp:HyperLink>

    </div>
</form>
</body>
</html>

