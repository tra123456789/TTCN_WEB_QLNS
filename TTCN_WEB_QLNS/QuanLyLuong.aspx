<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="QuanLyLuong.aspx.cs" Inherits="TTCN_WEB_QLNS.QuanLyLuong" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Quản Lý Lương</title>
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
     <h1> Quản lý lương</h1>
  
    
     <div class="breadcrumb">
         Menu › Lương
     </div>

     <br /><br />
             <div  >
               <asp:Button ID="btnDS" runat="server" Text="➕ Danh sách nhận lương" CssClass="btn" OnClick="btnDS_Click" />
                  
                 <asp:Button ID="btncaculator" runat="server" Text="➕ Tính Lương" CssClass="btn"  OnClick="btncaculator_Click" />
                
             <br /><br />
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
   <asp:GridView ID="gvLuong" runat="server" AutoGenerateColumns="False"
    CssClass="table" AllowPaging="True"
    OnPageIndexChanging="gvLuong_PageIndexChanging"
    DataKeyNames="ID"
    OnSelectedIndexChanged="gvLuong_SelectedIndexChanged"
    AllowCustomPaging="True" 
    OnRowCancelingEdit="gvLuong_RowCancelingEdit"
    OnRowDeleting="gvLuong_RowDeleting"
    OnRowEditing="gvLuong_RowEditing"
    OnRowUpdating="gvLuong_RowUpdating" OnRowDataBound="gvLuong_RowDataBound">

    <Columns>
        <asp:TemplateField HeaderText="Mã Nhân Viên">
            <ItemTemplate>
                <%# Eval("MaNV") %>
            </ItemTemplate>
               <EditItemTemplate>
       <asp:TextBox ID="txtMaNV" runat="server" Text='<%# Eval("MaNV") %>' />
   </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Họ Tên">
            <ItemTemplate>
                <%# Eval("HoTen") %>
            </ItemTemplate>
               <EditItemTemplate>
       <asp:TextBox ID="txtHoTen" runat="server" Text='<%# Eval("HoTen") %>' />
   </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Ngày Công">
    <ItemTemplate>
        <%# Eval("TongNgayCong") %>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtNgayCong" runat="server" Text='<%# Eval("TongNgayCong") %>' />
    </EditItemTemplate>
</asp:TemplateField>


        <asp:TemplateField HeaderText="Không Phép">
            <ItemTemplate>
                <%# Eval("KhongPhep") %>
            </ItemTemplate>
               <EditItemTemplate>
       <asp:TextBox ID="txtKhongPhep" runat="server" Text='<%# Eval("KhongPhep") %>' />
   </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Ngày Lễ">
            <ItemTemplate>
                <%# Eval("NgayLe") %>
            </ItemTemplate>
               <EditItemTemplate>
       <asp:TextBox ID="txtNgayLe" runat="server" Text='<%# Eval("NgayLe") %>' />
   </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Ngày Chủ Nhật">
            <ItemTemplate>
                <%# Eval("NgayCN") %>

            </ItemTemplate>
               <EditItemTemplate>
       <asp:TextBox ID="txtNgayCN" runat="server" Text='<%# Eval("NgayCN") %>' />
   </EditItemTemplate>
        </asp:TemplateField>

      <asp:TemplateField HeaderText="Thực Lãnh">
    <ItemTemplate>
        <%# Eval("ThucLanh", "{0:N0}") %>
    </ItemTemplate>
</asp:TemplateField>

          <asp:TemplateField HeaderText="Thao tác">
    <ItemTemplate>
        <asp:Button ID="btnExportExcel" runat="server" 
    Text="📥 Xuất danh sách Excel"
    CssClass="btn"
    OnClick="btnExportExcel_Click" />


        <%-- Nút Sửa (Edit) --%>
        <asp:LinkButton ID="BtnEdit" runat="server" CommandName="Edit" Text="Sửa" CssClass="btn btn-sm btn-info" />

        <%-- Nút Xóa (Delete) có thêm OnClientClick xác nhận --%>
        <asp:LinkButton ID="BtnDelete" runat="server" CommandName="Delete" Text="Xóa" CssClass="btn btn-sm btn-danger"
            OnClientClick="return confirm('Bạn chắc chắn muốn xóa nhân viên này không?');" />
    </ItemTemplate>
    <EditItemTemplate>
        <%-- Nút Cập nhật (Update) --%>
        <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Cập nhật" CssClass="btn btn-sm btn-success" />

        <%-- Nút Bỏ qua (Cancel) --%>
        <asp:LinkButton ID="BtnCancel" runat="server" CommandName="Cancel" Text="Bỏ Qua" CssClass="btn btn-sm btn-secondary" />

    </EditItemTemplate>
</asp:TemplateField> 
    </Columns>
</asp:GridView>



        </div>
         </div>         

        </div>
    </form>
</body>
</html>
