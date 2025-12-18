<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuanLyUser.aspx.cs" Inherits="TTCN_WEB_QLNS.WebForm2" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Quản lý nhân viên
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <!-- NỘI DUNG QUẢN LÝ NHÂN VIÊN -->
    <h1>Quản lý nhân viên</h1>


    <br />

    <!-- FORM THÊM NHÂN VIÊN -->
     <a href="Themnhanvien.aspx" class="btn">➕ Thêm Nhân Viên</a>

    <br /><br />

    <!-- OPTIONS -->
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

    <asp:CheckBox ID="chkNhanVienNghi" runat="server"
        Text="Hiển thị nhân viên đã nghỉ"
        AutoPostBack="true"
        OnCheckedChanged="chkNhanVienNghi_CheckedChanged" />

   
    <!-- GRIDVIEW -->
         <br />
 <div class="table-wrapper">
    <asp:GridView ID="gvQuanLyUser" runat="server"
        CssClass="table"
        AutoGenerateColumns="False"
        DataKeyNames="MANV"
        AllowPaging="True"
        OnPageIndexChanging="gvQuanLyUser_PageIndexChanging"
        OnRowEditing="gvQuanLyUser_RowEditing"
        OnRowCancelingEdit="gvQuanLyUser_RowCancelingEdit"
        OnRowUpdating="gvQuanLyUser_RowUpdating"
        OnRowDeleting="gvQuanLyUser_RowDeleting"
        OnRowCommand="gvQuanLyUser_RowCommand"
        OnRowDataBound="gvQuanLyUser_RowDataBound">


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
            ImageUrl='<%# Eval("HinhAnh") %>'
            CssClass="avatar" />
    </ItemTemplate>

    <EditItemTemplate>
        <asp:Image ID="imgOld" runat="server"
            ImageUrl='<%# Eval("HinhAnh") %>'
            CssClass="avatar" /><br />

        <asp:FileUpload ID="fuEditAvatar" runat="server" />
    </EditItemTemplate>
</asp:TemplateField>

   <asp:BoundField DataField="GioiTinhText" HeaderText="Giới tính" />


<asp:TemplateField HeaderText="Bộ phận">
    <ItemTemplate>
        <%# Eval("TenBP") %>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:DropDownList ID="ddlBP_Grid" runat="server"></asp:DropDownList>
    </EditItemTemplate>
</asp:TemplateField>

  <asp:TemplateField HeaderText="Thao tác">
    <ItemTemplate>


        <asp:LinkButton ID="LinkButtonDetail"
    runat="server"
    Text="Chi tiết"
    CssClass="btn btn-sm btn-primary"
    CommandName="Detail"
    CommandArgument='<%# Eval("MaNV") %>' />

    
        <asp:LinkButton ID="LinkButtonDelete"
            runat="server"
            CommandName="Delete"
            Text="Xóa"
            CssClass="btn btn-sm btn-danger"
            OnClientClick="return confirm('Cho nhân viên này nghỉ việc?');" />

        <asp:LinkButton ID="LinkButtonRestore"
            runat="server"
            Text="Khôi phục"
            CommandName="Restore"
            CommandArgument='<%# Eval("MaNV") %>'
            CssClass="btn btn-sm btn-warning"
            Visible="false"
            OnClientClick="return confirm('Khôi phục nhân viên này?');" />

    </ItemTemplate>

    
      </asp:TemplateField>
    
                 
            </Columns>

        </asp:GridView>
   </div>
  
    </asp:Content>