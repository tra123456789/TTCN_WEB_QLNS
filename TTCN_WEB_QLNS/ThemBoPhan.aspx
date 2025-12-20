<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.Master" CodeBehind="ThemBoPhan.aspx.cs" Inherits="TTCN_WEB_QLNS.ThemBoPhan" %>


<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">

        <h1>Quản lý bộ phận</h1>

        <div class="breadcrumb">
            Menu › Phòng ban › Bộ phận
        </div>

        <br />

        <asp:Label ID="lblPhongBan" runat="server"
            Font-Bold="true" Font-Size="Large" />

        <br /><br />

        <!-- THÊM BỘ PHẬN -->
        Tên bộ phận:
        <asp:TextBox ID="txtTenBP" runat="server" />
        <asp:Button ID="btnAddBP" runat="server"
            Text="➕ Thêm bộ phận"
            CssClass="btn"
            OnClick="btnAddBP_Click" />

        <br /><br />

        <!-- GRIDVIEW BỘ PHẬN -->
        <asp:GridView ID="gvBoPhan" runat="server"
            AutoGenerateColumns="False"
            CssClass="table"
            DataKeyNames="IDBP"
            OnRowEditing="gvBoPhan_RowEditing"
            OnRowUpdating="gvBoPhan_RowUpdating"
            OnRowCancelingEdit="gvBoPhan_RowCancelingEdit"
            OnRowDeleting="gvBoPhan_RowDeleting">

            <Columns>

                <asp:TemplateField HeaderText="Tên bộ phận">
                    <ItemTemplate>
                        <%# Eval("TenBP") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTenBP_Edit" runat="server"
                            Text='<%# Bind("TenBP") %>' />
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
                            OnClientClick="return confirm('Xóa bộ phận này?');" />
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:LinkButton runat="server"
                            CommandName="Update"
                            Text="Lưu"
                            CssClass="btn btn-sm btn-success" />

                        <asp:LinkButton runat="server"
                            CommandName="Cancel"
                            Text="Hủy"
                            CssClass="btn btn-sm btn-secondary" />
                    </EditItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

        <br />

        <asp:Button runat="server"
            Text="⬅ Quay lại phòng ban"
            CssClass="btn"
            OnClick="btnBack_Click" />

    </div>

</asp:Content>
