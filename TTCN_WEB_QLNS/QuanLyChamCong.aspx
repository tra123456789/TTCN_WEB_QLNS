<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuanLyChamCong.aspx.cs" Inherits="TTCN_WEB_QLNS.QuanLyChamCong" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Khen Thưởng</title>
    <link href ="QLUSER.css" rel ="stylesheet" type="text/css"/>
    </head>
<body>
    <form id="form1" runat="server">
        <div>
             <!-- Sidebar -->
    <div class="sidebar">
        <h2>Trang quản trị</h2>
               <a id="menuTongQuan" runat="server" href="TongQuan.aspx">Tổng quan</a>
<a id="menuNhanVien" runat="server" href="QuanLyUser.aspx">Nhân viên</a>
<a id="menuPhongBan" runat="server" href="PhongBan.aspx">Phòng ban</a>
<a id="menuChamCong" runat="server" href="QuanLyChamCong.aspx">Chấm công</a>
<a id="menuHopDong" runat="server" href="QuanLyHopDong.aspx">Hợp đồng</a>
<a id="menuBaoHiem" runat="server" href="BaoHiem.aspx">Bảo hiểm xã hội</a>
<a id="menuLuong" runat="server" href="QuanLyLuong.aspx">Lương nhân viên</a>
<a id="menuKhenThuong" runat="server" href="KhenThuong.aspx">Khen thưởng</a>


    </div>

    <!-- PAGE CONTENT -->
         <div class="content">
         <div class ="welcome">          
          <asp:Label ID="lblWelcome" runat="server" CssClass="font-medium text-sm"></asp:Label>
          <p class="text-xs opacity-75">Quản trị viên</p>

         </div>
        <h1> Quản lý Chấm Công</h1>
     
       
        <div class="breadcrumb">
            Tổng quan › Chấm công nhân viên
        </div>

        <br /><br />
                <div >
                     <div>
            Tháng:
            <asp:DropDownList ID="ddlThang" runat="server"></asp:DropDownList>

            Năm:
            <asp:DropDownList ID="ddlNam" runat="server"></asp:DropDownList>

            Nhân viên:
            <asp:DropDownList ID="ddlNhanVien" runat="server"></asp:DropDownList>

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
        <asp:GridView ID="gvQuanLyUser" runat="server" AutoGenerateColumns="False"
            CssClass="table" AllowPaging="True"
             DataKeyNames="MANV"
            OnPageIndexChanging="gvQuanLyUser_PageIndexChanging"
            OnSelectedIndexChanged="gvQuanLyUser_SelectedIndexChanged"
            OnRowEditing="gvQuanLyUser_RowEditing"
            OnRowCancelingEdit="gvQuanLyUser_RowCancelingEdit"
            OnRowDeleting="gvQuanLyUser_RowDeleting"
            OnRowUpdating="gvQuanLyUser_RowUpdating">

            <Columns>
                <asp:BoundField DataField="MANV" HeaderText="Mã nhân viên" ReadOnly="True" />
                <asp:BoundField DataField="HoTen" HeaderText="Tên Nhân Viên" />
                <asp:BoundField DataField="NgaySinh" HeaderText="Ngày Sinh" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="SDT" HeaderText="SDT" />
                <asp:BoundField DataField="CCCD" HeaderText="Số Căn Cước"  />
             <asp:BoundField DataField="DiaChi" HeaderText="Địa Chỉ" />
             <asp:TemplateField HeaderText="Hình Ảnh">
                <ItemTemplate>
                    <asp:Image ID="imgAvatar" runat="server" 
                        ImageUrl='<%# Eval("HinhAnh") %>'  CssClass="avatar"/>
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
