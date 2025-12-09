<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaoHiem.aspx.cs" Inherits="TTCN_WEB_QLNS.Quan_Ly_Nhan_Vien" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bảo Hiểm</title>
    <link href ="BaoHiem.css" rel ="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <!-- Sidebar -->
    <div class="sidebar">
        <h2>Menu</h2>
               <a id="menuTongQuan" runat="server" href="TongQuan.aspx">Tổng quan</a>
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
        <h1>BẢO HIỂM XÃ HỘI</h1>

        <div class="breadcrumb">
            Tổng quan › Bảo Hiểm Xã Hội
        </div>

        <br /><br />
                <div >
          <asp:Button ID="btnAddHD" runat="server" Text="➕ Thêm Bảo Hiểm " CssClass="btn" OnClick="btnAddBH_Click" />

       <br /><br />
        <div class ="cssadd">
         Mã Nhân Viên :
        <asp:TextBox ID="txtMaNV" runat="server" OnTextChanged="txtMaNV_TextChanged"></asp:TextBox>

        Số Bảo Hiểm :
        <asp:TextBox ID="txtSoBaoHiem" runat="server"  OnTextChanged="txtSoBaoHiem_TextChanged"></asp:TextBox>

        Từ Tháng:
        <asp:TextBox ID="txtTuThang" runat="server" TextMode="Date"  OnTextChanged="txtTuThang_TextChanged"></asp:TextBox>

        Đến Tháng:
        <asp:TextBox ID="txtDenThang" runat="server" TextMode="Date"  OnTextChanged="txtDenThang_TextChanged"></asp:TextBox>

        Đơn vị :
        <asp:TextBox ID="txtDonVi" runat="server" OnTextChanged="txtNoiDung_TextChanged"></asp:TextBox>

            </div>
        <br /><br />

   
    
</div>

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
        <asp:GridView ID="gvBaoHiem" runat="server" AutoGenerateColumns="False"
            DataKeyNames="MaNV"
            CssClass="table" AllowPaging="True"
            OnPageIndexChanging="gvBaoHiem_PageIndexChanging"
            PageSize="10" OnSelectedIndexChanged="gvBaoHiem_SelectedIndexChanged">

            <Columns>
                <asp:BoundField DataField="MaNV" HeaderText="Mã nhân viên" />
                <asp:BoundField DataField="SoBH" HeaderText="Số Bảo Hiểm"  />
                <asp:BoundField DataField="TuThang" HeaderText="Từ Tháng" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="DenThang" HeaderText="Đến Tháng" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="DonVi" HeaderText="Đơn Vị" />
                <asp:BoundField DataField="Chucvu" HeaderText="Chức Vụ" />
       

            </Columns>

       
        </asp:GridView>

    </div>

        </div>
    </form>
</body>
</html>
