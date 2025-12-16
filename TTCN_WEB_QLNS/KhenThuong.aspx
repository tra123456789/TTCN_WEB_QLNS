<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhenThuong.aspx.cs" Inherits="TTCN_WEB_QLNS.KhenThuong" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Khen Thưởng</title>
    <link href ="Quanlynhanvien.css" rel ="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <!-- Sidebar -->
    <div class="sidebar">
        <h2>Menu</h2>
                <a id="menuTongQuan" runat="server" href="TongQuan.aspx">Tổng quan</a>
         <a id="menuThongTinNV" runat="server" href="ThongTinCaNhan.aspx">Thông tin cá nhân</a>
<a id="menuNhanVien" runat="server" href="QuanLyUser.aspx">Nhân viên</a>
<a id="menuPhongBan" runat="server" href="PhongBan.aspx">Phòng ban</a>
<a id="menuChamCong" runat="server" href="QuanLyChamCong.aspx">Chấm công</a>
<a id="menuHopDong" runat="server" href="QuanLyHopDong.aspx">Hợp đồng</a>
<a id="menuBaoHiem" runat="server" href="BaoHiem.aspx">Bảo hiểm xã hội</a>
<a id="menuLuong" runat="server" href="QuanLyLuong.aspx">Lương nhân viên</a>
<a id="menuKhenThuong" runat="server" href="KhenThuong.aspx">Khen thưởng</a>
         <!-- THÊM Ô ĐĂNG XUẤT VÀO ĐÂY -->
        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="logout-link" OnClick="lnkLogout_Click">Đăng xuất</asp:LinkButton>
        
    </div>

    <!-- PAGE CONTENT -->
    <div class="content">
                       <div class ="welcome">   
           <p class="text-xs opacity-75">Chào mừng bạn quay trở lại</p>
 <asp:Label ID="lblWelcome" runat="server" CssClass="font-medium text-sm"></asp:Label>
   

</div>
        <h1>Khen thưởng nhân viên</h1>

        <div class="breadcrumb">
            Menu › Khen thưởng
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
                <asp:BoundField DataField ="HoTen" HeaderText ="Họ và tên "/>
                <asp:BoundField DataField="Ngay" HeaderText="Tháng thưởng" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="NoiDung" HeaderText="Nội Dung" />
                <asp:BoundField DataField="SoKTKL" HeaderText="Tiền Thưởng" DataFormatString="{0:N0}" />
               <asp:TemplateField HeaderText="Loại">
    <ItemTemplate>
        <%# Convert.ToInt32(Eval("Loai")) == 1 ? "Khen thưởng" : "Kỷ luật" %>
    </ItemTemplate>
</asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

        </div>
    </form>
</body>
</html>
