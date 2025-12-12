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
        <h1>BẢO HIỂM XÃ HỘI</h1>

        <div class="breadcrumb">
            Tổng quan › Bảo Hiểm Xã Hội
        </div>

        <br /><br />
                <div >
          <asp:Button ID="btnAddHD" runat="server" Text="➕ Thêm Bảo Hiểm " CssClass="btn" OnClick="btnAddBH_Click" />

       <br /><br />
        <div  class ="cssadd">
            <div>
         Mã Nhân Viên :
        <asp:TextBox ID="txtMaNV" runat="server" OnTextChanged="txtMaNV_TextChanged"></asp:TextBox>

        Số Bảo Hiểm :
        <asp:TextBox ID="txtSoBH" runat="server"  OnTextChanged="txtSoBH_TextChanged"></asp:TextBox>

        Từ Tháng:
        <asp:TextBox ID="txtTuThang" runat="server" TextMode="Date"  OnTextChanged="txtTuThang_TextChanged"></asp:TextBox>

        Đến Tháng:
        <asp:TextBox ID="txtDenThang" runat="server" TextMode="Date"  OnTextChanged="txtDenThang_TextChanged"></asp:TextBox>

        Đơn vị :
        <asp:TextBox ID="txtDonVi" runat="server" OnTextChanged="txtNoiDung_TextChanged"></asp:TextBox>
            </div>
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
            DataKeyNames="IDBH"
            CssClass="table" AllowPaging="True"
            OnPageIndexChanging="gvBaoHiem_PageIndexChanging"
            PageSize="10" OnSelectedIndexChanged="gvBaoHiem_SelectedIndexChanged" OnRowCancelingEdit="gvBaoHiem_RowCancelingEdit" OnRowDataBound="gvBaoHiem_RowDataBound" OnRowDeleting="gvBaoHiem_RowDeleting" OnRowEditing="gvBaoHiem_RowEditing" OnRowUpdating="gvBaoHiem_RowUpdating">

            <Columns>
              
                       
  <asp:TemplateField HeaderText="Mã Nhân Viên">
      <ItemTemplate>
          <%# Eval("MaNV") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtMaNV" runat="server" Text='<%# Bind("MaNV") %>' />
      </EditItemTemplate>
  </asp:TemplateField>

  <asp:TemplateField HeaderText="Số Bảo Hiểm">
      <ItemTemplate>
          <%# Eval("SoBH") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtSoBH" runat="server" Text='<%# Bind("SoBH") %>' />
      </EditItemTemplate>
  </asp:TemplateField>

  <asp:TemplateField HeaderText="Từ Tháng">
      <ItemTemplate>
          <%# Eval("TuThang") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtTuThang" runat="server" Text='<%# Bind("TuThang") %>' />
      </EditItemTemplate>
  </asp:TemplateField>
                  
  <asp:TemplateField HeaderText="Đến Tháng">
      <ItemTemplate>
          <%# Eval("DenThang") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtDenThang" runat="server" Text='<%# Bind("DenThang") %>' />
      </EditItemTemplate>
  </asp:TemplateField>

  <asp:TemplateField HeaderText="Đơn Vị">
      <ItemTemplate>
          <%# Eval("DonVi") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:TextBox ID="txtDonVi" runat="server" Text='<%# Bind("DonVi") %>' />
      </EditItemTemplate>
  </asp:TemplateField>

  <asp:TemplateField HeaderText="Chức Vụ">
      <ItemTemplate>
          <%# Eval("Chucvu") %>
      </ItemTemplate>
      <EditItemTemplate>
          <asp:Label ID="lblChucVu" runat="server" Text='<%# Bind("ChucVu") %>' />
      </EditItemTemplate>
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
