<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Site.Master"
    CodeBehind="PhongBan.aspx.cs"
    Inherits="TTCN_WEB_QLNS.PhanQuyen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">

        <h1>Phòng ban nhân viên</h1>

        <div class="breadcrumb">
            Menu › Phòng ban
        </div>

        <br />

        <asp:Button ID="btnAddUser" runat="server"
            Text="➕ Thêm phòng ban"
            CssClass="btn"
            OnClick="btnAdd_Click" />

        <div class="cssaddPB">
            <br />

            Tên phòng ban:
            <asp:TextBox ID="txtTenPB" runat="server" />

            SĐT:
            <asp:TextBox ID="txtSDT" runat="server" />

            Địa chỉ:
            <asp:TextBox ID="txtDiaChi" runat="server" />
            Lương cơ bản:
<asp:TextBox ID="txtLuongCoBan" runat="server" />

        </div>

        <br />

        <!-- Options -->
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

        <!-- TABLE -->
        <asp:GridView ID="gvPhongBan" runat="server"
            AutoGenerateColumns="False"
            CssClass="table"
            AllowPaging="True"
            DataKeyNames="IDPB"
            PageSize="10"
            OnPageIndexChanging="gvPhongBan_PageIndexChanging"
            OnRowEditing="gvPhongBan_RowEditing"
            OnRowUpdating="gvPhongBan_RowUpdating"
            OnRowDeleting="gvPhongBan_RowDeleting"
            OnRowCancelingEdit="gvPhongBan_RowCancelingEdit" OnRowCommand="gvPhongBan_RowCommand">

            <Columns>

                <asp:TemplateField HeaderText="Tên phòng ban">
                    <ItemTemplate><%# Eval("TenPB") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTenPB" runat="server"
                            Text='<%# Bind("TenPB") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="SĐT">
                    <ItemTemplate><%# Eval("SDT") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSDT" runat="server"
                            Text='<%# Bind("SDT") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Địa chỉ">
                    <ItemTemplate><%# Eval("DiaChi") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDiaChi" runat="server"
                            Text='<%# Bind("DiaChi") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lương cơ bản">
    <ItemTemplate>
        <%# Eval("LuongCoBan", "{0:N0}") %>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtLuongCoBan" runat="server"
            Text='<%# Bind("LuongCoBan") %>' />
    </EditItemTemplate>
</asp:TemplateField>

                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                      

                        <asp:LinkButton runat="server"
                            CommandName="Edit"
                            Text="Sửa"
                            CssClass="btn btn-sm btn-warning" />

                        <asp:LinkButton runat="server"
                            CommandName="Delete"
                            Text="Xóa"
                            CssClass="btn btn-sm btn-danger"
                            OnClientClick="return confirm('Xóa phòng ban này?');" />

                                <asp:LinkButton runat="server"
                                Text="Xem nhân viên"
                                CssClass="btn btn-sm btn-info"
                                CommandArgument='<%# Eval("IDPB") %>'
                                OnClick="btnXemNhanVien_Click" />
                        <asp:LinkButton runat="server"
                            Text="➕ Thêm bộ phận"
                            CssClass="btn btn-sm btn-success"
                            CommandName="AddBoPhan"
                            CommandArgument='<%# Eval("IDPB") %>' />

                    </ItemTemplate>
                     <EditItemTemplate>



        <!-- LƯU -->
        <asp:LinkButton runat="server"
            CommandName="Update"
            Text="Lưu"
            CssClass="btn btn-sm btn-success" />

        <!-- HỦY -->
        <asp:LinkButton runat="server"
            CommandName="Cancel"
            Text="Hủy"
            CssClass="btn btn-sm btn-secondary" />

    </EditItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

    </div>

</asp:Content>
