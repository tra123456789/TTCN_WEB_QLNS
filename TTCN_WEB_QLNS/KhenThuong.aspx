<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhenThuong.aspx.cs" Inherits="TTCN_WEB_QLNS.KhenThuong" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Khen Thưởng</title>
    <link href ="KhenThuong.css" rel ="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <!-- Sidebar -->
    <div class="sidebar">
        <h2>Trang quản trị</h2>
        <a >Tổng quan</a>
        <a href ="QuanLyUser.aspx">Nhân viên</a>
        <a>Phòng ban</a>
        <a>Lương nhân viên</a>
        <a class="active" href ="KhenThuong.aspx">Khen thưởng</a>
    </div>

    <!-- PAGE CONTENT -->
    <div class="content">
        <h1>Khen thưởng nhân viên</h1>

        <div class="breadcrumb">
            Tổng quan › Khen thưởng
        </div>

        <br /><br />

        <a href="ThemKhenThuong.aspx" class="btn">Khen Thưởng</a>

        <!-- Options -->
        <div class="top-options">
            Hiển thị
            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem Selected="True">10</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
            </asp:DropDownList>

            <div class="search-box">
                Tìm kiếm:
                <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="true"
                    OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
            </div>
        </div>

        <!-- TABLE -->
        <asp:GridView ID="gvKhenThuong" runat="server" AutoGenerateColumns="False"
            CssClass="table" AllowPaging="True"
            OnPageIndexChanging="gvKhenThuong_PageIndexChanging"
            PageSize="10" OnSelectedIndexChanged="gvKhenThuong_SelectedIndexChanged">

            <Columns>
                <asp:BoundField DataField="MANV" HeaderText="Mã nhân viên" />
                <asp:BoundField DataField="Ngay" HeaderText="Tháng thưởng" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="NoiDung" HeaderText="Nội Dung" />
                <asp:BoundField DataField="SoKTKL" HeaderText="Tiền Thưởng" DataFormatString="{0:N0}" />
            </Columns>

        </asp:GridView>

    </div>

        </div>
    </form>
</body>
</html>
