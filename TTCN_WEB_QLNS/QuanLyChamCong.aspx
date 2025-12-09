<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuanLyChamCong.aspx.cs" Inherits="TTCN_WEB_QLNS.QuanLyChamCong" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quản lý chấm công</title>
    <link href ="Quanlynhanvien.css" rel ="stylesheet" type="text/css"/>
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
        <h1> Quản lý Chấm Công</h1>
     
       
        <div class="breadcrumb">
            Tổng quan › Chấm công nhân viên
        </div>

        <br /><br />
                <div >
                     <div>
            Tháng:
            <asp:DropDownList ID="ddlThang" runat="server" OnSelectedIndexChanged="ddlThang_SelectedIndexChanged"></asp:DropDownList>

            Năm:
            <asp:DropDownList ID="ddlNam" runat="server" OnSelectedIndexChanged="ddlNam_SelectedIndexChanged"></asp:DropDownList>

            Nhân viên:
            <asp:DropDownList ID="ddlNhanVien" runat="server" OnSelectedIndexChanged="ddlNhanVien_SelectedIndexChanged"></asp:DropDownList>

            <asp:Button ID="btnLoad" runat="server" Text="Tải dữ liệu" OnClick="btnLoad_Click" />
        </div>


   
            
        </div>
     

<br />

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
       <!-- GridView chấm công -->
        <asp:GridView ID="gvChamCong" runat="server" AutoGenerateColumns="False" CssClass="tbl" OnSelectedIndexChanged="gvChamCong_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="Ngay" HeaderText="Ngày" ReadOnly="true" />
                <asp:BoundField DataField="Thu" HeaderText="Thứ" ReadOnly="true" />

                <asp:TemplateField HeaderText="Giờ vào">
                    <ItemTemplate>
                        <asp:TextBox ID="txtGioVao" runat="server" CssClass="time-input"
                            Text='<%# Bind("GioVao") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Giờ ra">
                    <ItemTemplate>
                        <asp:TextBox ID="txtGioRa" runat="server" CssClass="time-input"
                            Text='<%# Bind("GioRa") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="CongNgay" HeaderText="Công " ReadOnly="true" />

                <asp:TemplateField HeaderText="Ghi chú">
                    <ItemTemplate>
                        <asp:TextBox ID="txtGhiChu" runat="server" CssClass="note-input"
                            Text='<%# Bind("GhiChu") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
             
        <asp:Button ID="btnTinhCong" runat="server" Text="Tính công" OnClick="btnTinhCong_Click" />
        <asp:Button ID="btnSave" runat="server" Text="Lưu dữ liệu" OnClick="btnSave_Click" />

    </div>

        </div>
    </form>
</body>
</html>
