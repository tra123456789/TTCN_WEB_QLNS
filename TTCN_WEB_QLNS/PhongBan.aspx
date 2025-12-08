<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhongBan.aspx.cs" Inherits="TTCN_WEB_QLNS.PhanQuyen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Phòng Ban</title>
    <link href ="Quanlynhanvien.css" rel ="stylesheet" type="text/css"/>
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
        <h1>Phòng ban nhân viên</h1>

        <div class="breadcrumb">
            Tổng quan › Phòng Ban
        </div>

        <br /><br />

               <asp:Button ID="btnAddUser" runat="server" Text="➕ Thêm Phòng Ban"
            CssClass="btn" OnClick="btnAdd_Click" />
                  <div class ="cssaddPB">
     
         <br /><br />

         Tên Phòng Ban:
         <asp:TextBox ID="txtTenPB" runat="server" OnTextChanged="txtTenPB_TextChanged"></asp:TextBox>

         SĐT:
         <asp:TextBox ID="txtSDT" runat="server" OnTextChanged="txtSDT_TextChanged"></asp:TextBox>

         Địa chỉ:
         <asp:TextBox ID="txtDiaChi" runat="server" OnTextChanged="txtDiaChi_TextChanged"></asp:TextBox>

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
        <asp:GridView ID="gvPhongBan" runat="server" AutoGenerateColumns="False"
            CssClass="table" AllowPaging="True"
            OnPageIndexChanging="gvPhongBan_PageIndexChanging"
            DataKeyNames="IDPB"
            PageSize="10"
            OnSelectedIndexChanged="gvPhongBan_SelectedIndexChanged"
            AllowCustomPaging="True" 
            OnRowCancelingEdit="gvPhongBan_RowCancelingEdit"
            OnRowDeleting="gvPhongBan_RowDeleting"
            OnRowEditing="gvPhongBan_RowEditing"
            OnRowUpdating="gvPhongBan_RowUpdating">

            <Columns>
            <asp:TemplateField HeaderText="Tên Phòng Ban">
                <ItemTemplate>
                    <%# Eval("TenPB") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtTenPB" runat="server" Text='<%# Bind("TenPB") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Số Điện Thoại">
                <ItemTemplate>
                    <%# Eval("SDT") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtSDT" runat="server" Text='<%# Bind("SDT") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Địa Chỉ">
                <ItemTemplate>
                    <%# Eval("DiaChi") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtDiaChi" runat="server" Text='<%# Bind("DiaChi") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

                          <asp:TemplateField HeaderText="Thao tác">
            <ItemTemplate>
                <%-- Nút Sửa (Edit) --%>
                <asp:LinkButton ID="LinkButtonEdit" runat="server" CommandName="Edit" Text="Sửa" CssClass="btn btn-sm btn-info" />

                <%-- Nút Xóa (Delete) có thêm OnClientClick xác nhận --%>
                <asp:LinkButton ID="LinkButtonDelete" runat="server" CommandName="Delete" Text="Xóa" CssClass="btn btn-sm btn-danger"
                    OnClientClick="return confirm('Bạn chắc chắn muốn xóa phòng ban này không?');" />
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
