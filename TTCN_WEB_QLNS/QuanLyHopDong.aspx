<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuanLyHopDong.aspx.cs" Inherits="TTCN_WEB_QLNS.QuanLyHopDong" %>

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
        <h1> Quản lý Hợp Đồng</h1>
     
       
        <div class="breadcrumb">
            Tổng quan › Hợp Đồng
        </div>

        <br /><br />
                <div >
                  <asp:Button ID="btnAddHD" runat="server" Text="➕ Thêm Hợp Đồng " CssClass="btn" OnClick="btnAddHD_Click" />

               <br /><br />
                <div class ="cssadd">
                Ngày Bắt Đầu:
                <asp:TextBox ID="txtNgayBatDau" runat="server" TextMode="Date" OnTextChanged="txtNgayBatDau_TextChanged"></asp:TextBox>

                Ngày Kết Thúc:
                <asp:TextBox ID="txtNgayKetThuc" runat="server" TextMode="Date" OnTextChanged="txtNgayKetThuc_TextChanged"></asp:TextBox>

                Ngày Ký:
                <asp:TextBox ID="txtNgayKi" runat="server" TextMode="Date" OnTextChanged="txtNgayKi_TextChanged"></asp:TextBox>

                Nội Dung:
                <asp:TextBox ID="txtNoiDung" runat="server" OnTextChanged="txtNoiDung_TextChanged"></asp:TextBox>

                Lần Ký :
                <asp:TextBox ID="txtLanKy" runat="server" OnTextChanged="txtLanKy_TextChanged"></asp:TextBox>

                Thời hạn:
                <asp:TextBox ID="txtThoiHan" runat="server" OnTextChanged="txtThoiHan_TextChanged" ></asp:TextBox>

                 Hệ Số Lương :
                 <asp:TextBox ID="txtHeSoLuong" runat="server" OnTextChanged="txtHeSoLuong_TextChanged"></asp:TextBox>

                 Mã Nhân Viên:
                 <asp:TextBox ID="txtMaNV" runat="server" OnTextChanged="txtMaNV_TextChanged" ></asp:TextBox>

                    </div>
                <br /><br />

   
            
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
        <asp:GridView ID="gvQuanLyHD" runat="server" AutoGenerateColumns="False"
            CssClass="table" AllowPaging="True"
             DataKeyNames="SoHD"
            OnPageIndexChanging="gvQuanLyHD_PageIndexChanging"
            OnSelectedIndexChanged="gvQuanLyHD_SelectedIndexChanged"
            OnRowEditing="gvQuanLyHD_RowEditing"
            OnRowCancelingEdit="gvQuanLyHD_RowCancelingEdit"
            OnRowDeleting="gvQuanLyHD_RowDeleting"
            OnRowUpdating="gvQuanLyHD_RowUpdating" OnRowCommand="gvQuanLyHD_RowCommand">

            <Columns>
                  
  <asp:TemplateField HeaderText="Mã Nhân Viên">
      <ItemTemplate>
          <%# Eval("MaNV") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtGV_MaNV" runat="server" Text='<%# Bind("MaNV") %>' />
      </EditItemTemplate>
  </asp:TemplateField>

<asp:TemplateField HeaderText="Số Hợp Đồng">
    <ItemTemplate>
        <%# Eval("SoHD") %>
    </ItemTemplate>
</asp:TemplateField>

  <asp:TemplateField HeaderText="Ngày Bắt Đầu">
      <ItemTemplate>
          <%# Eval("NgayBatDau") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtGV_NgayBatDau" runat="server" Text='<%# Bind("NgayBatDau") %>' />
      </EditItemTemplate>
  </asp:TemplateField>
                  
  <asp:TemplateField HeaderText="Ngày Kết Thúc">
      <ItemTemplate>
          <%# Eval("NgayKetThuc") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtGV_NgayKetThuc" runat="server" Text='<%# Bind("NgayKetThuc") %>' />
      </EditItemTemplate>
  </asp:TemplateField>

  <asp:TemplateField HeaderText="Ngày ký">
      <ItemTemplate>
          <%# Eval("NgayKi") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtGV_NgayKi" runat="server" Text='<%# Bind("NgayKi") %>' />
      </EditItemTemplate>
  </asp:TemplateField>

  <asp:TemplateField HeaderText="Nội Dung">
      <ItemTemplate>
          <%# Eval("NoiDung") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtGV_NoiDung" runat="server" Text='<%# Bind("NoiDung") %>' />
      </EditItemTemplate>
  </asp:TemplateField>

   <asp:TemplateField HeaderText="Lần Ký ">
     <ItemTemplate>
         <%# Eval("LanKy") %>
     </ItemTemplate>
     <EditItemTemplate>
         <asp:TextBox ID="txtGV_LanKy" runat="server" Text='<%# Bind("LanKy") %>' />
     </EditItemTemplate>
 </asp:TemplateField>
                  <asp:TemplateField HeaderText="Thời Hạn ">
    <ItemTemplate>
        <%# Eval("ThoiHan") %>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtGV_ThoiHan" runat="server" Text='<%# Bind("ThoiHan") %>' />
    </EditItemTemplate>
</asp:TemplateField>

  <asp:TemplateField HeaderText="Hệ Số Lương">
      <ItemTemplate>
          <%# Eval("HeSoLuong") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtGV_HeSoLuong" runat="server" Text='<%# Bind("HeSoLuong") %>' />
      </EditItemTemplate>
  </asp:TemplateField>

      <asp:TemplateField HeaderText="PDF">
    <ItemTemplate>
        <asp:LinkButton 
            ID="btnExportPDF"
            runat="server"
            Text="📄 PDF"
            CommandName="ExportPDF"
            CommandArgument='<%# Eval("SoHD") %>' />
    </ItemTemplate>
</asp:TemplateField>


             
          <asp:TemplateField HeaderText="Thao tác">
            <ItemTemplate>
                <%-- Nút Sửa (Edit) --%>
                <asp:LinkButton ID="LinkButtonEdit" runat="server" CommandName="Edit" Text="Sửa" CssClass="btn btn-sm btn-info" />
        
                <%-- Nút Xóa (Delete) có thêm OnClientClick xác nhận --%>
                <asp:LinkButton ID="LinkButtonDelete" runat="server" CommandName="Delete" Text="Xóa" CssClass="btn btn-sm btn-danger"
                    OnClientClick="return confirm('Bạn chắc chắn muốn xóa nhân viên này không?');" />
            </ItemTemplate>
            <EditItemTemplate>
                <%-- Nút Cập nhật (Update) --%>
                <asp:LinkButton ID="LinkButtonUpdate" runat="server" CommandName="Update" Text="Cập nhật" CssClass="btn btn-sm btn-success" />
        
                <%-- Nút Bỏ qua (Cancel) --%>
                <asp:LinkButton ID="LinkButtonCancel" runat="server" CommandName="Cancel" Text="Bỏ Qua" CssClass="btn btn-sm btn-secondary" />
            </EditItemTemplate>
        </asp:TemplateField>  


    
                 
            </Columns>

        </asp:GridView>

    </div>

        </div>
    </form>
</body>
</html>
