<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Site.Master"
    EnableEventValidation="false"
    CodeBehind="QuanLyLuong.aspx.cs"
    Inherits="TTCN_WEB_QLNS.QuanLyLuong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">

        <h1>Quản lý lương</h1>

        <div class="breadcrumb">
            Menu › Lương
        </div>
        
  <br /><br />
        <!-- Nhân viên:
<asp:DropDownList ID="ddlNhanVien" runat="server" CssClass="input"></asp:DropDownList>
-->
Mã nhân viên:
<asp:TextBox ID="txtMaNV" runat="server" />
Ngày công:
<asp:TextBox ID="txtNgayCong" runat="server" />

Không phép:
<asp:TextBox ID="txtKhongPhep" runat="server" />

Ngày lễ:
<asp:TextBox ID="txtNgayLe" runat="server" />

Ngày CN:
<asp:TextBox ID="txtNgayCN" runat="server" />

<br /><br />

<asp:Button ID="Button1" runat="server"
    Text="➕ Thêm lương nhân viên"
    CssClass="btn"
    OnClick="btnAddLuong_Click" />

  <br />

        <br />

        <asp:Button ID="btnDS" runat="server"
            Text="➕ Danh sách nhận lương"
            CssClass="btn"
            OnClick="btnDS_Click" />

        <asp:Button ID="btncaculator" runat="server"
            Text="➕ Tính Lương"
            CssClass="btn"
            OnClick="btncaculator_Click" />

        <br /><br />

        <!-- top options -->
        <div class="top-options">
            Hiển thị
            <asp:DropDownList ID="ddlPageSize" runat="server"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem Selected="True">10</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
            </asp:DropDownList>

            <div class="search-box">
                Tìm kiếm:
                <asp:TextBox ID="txtSearch" runat="server"
                    AutoPostBack="true"
                    OnTextChanged="txtSearch_TextChanged" />
            </div>
        </div>

        <!-- GRID -->
        <asp:GridView ID="gvLuong" runat="server"
            AutoGenerateColumns="False"
            CssClass="table"
            AllowPaging="True"
            DataKeyNames="ID"
            OnPageIndexChanging="gvLuong_PageIndexChanging"
            OnRowEditing="gvLuong_RowEditing"
            OnRowUpdating="gvLuong_RowUpdating"
            OnRowDeleting="gvLuong_RowDeleting"
            OnRowCancelingEdit="gvLuong_RowCancelingEdit"
            OnRowDataBound="gvLuong_RowDataBound">

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

</asp:Content>
