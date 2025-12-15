<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThemKhenThuong.aspx.cs" Inherits="TTCN_WEB_QLNS.ThemKhenThuong" %>
   

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Thêm Khen thưởng / Kỷ luật</title>
    <link href="Themkhenthuong.css" rel="stylesheet" />
</head>
<body>
<form id="form1" runat="server">
    <div class="content">
        <h2>Thêm khen thưởng / kỷ luật</h2>

        <label>Nhân viên</label>
        <asp:DropDownList ID="ddlNhanVien" runat="server" CssClass="input"></asp:DropDownList>

        <label>Loại</label>
        <asp:DropDownList ID="ddlLoai" runat="server" CssClass="input">
            <asp:ListItem Value="1">Khen thưởng</asp:ListItem>
            <asp:ListItem Value="0">Kỷ luật</asp:ListItem>
        </asp:DropDownList>

        <label>Số tiền</label>
        <asp:TextBox ID="txtSoTien" runat="server" CssClass="input"></asp:TextBox>

        <label>Lý do</label>
        <asp:TextBox ID="txtLyDo" runat="server" CssClass="input"></asp:TextBox>

        <label>Ngày áp dụng</label>
        <asp:TextBox ID="txtNgay" runat="server" TextMode="Date" CssClass="input"></asp:TextBox>

        <br /><br />
        <asp:Button ID="btnLuu" runat="server" Text="Lưu"
            CssClass="btn" OnClick="btnLuu_Click" />

        <a href="KhenThuong.aspx" class="btn">Quay lại</a>
    </div>
</form>
</body>
</html>
