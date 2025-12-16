<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Site.Master"
    CodeBehind="BaoHiem.aspx.cs"
    Inherits="TTCN_WEB_QLNS.Quan_Ly_Nhan_Vien" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">

        <h1>BẢO HIỂM XÃ HỘI</h1>

        <div class="breadcrumb">
            Menu › Bảo hiểm xã hội
        </div>

        <br />

        <!-- FORM ADD -->
        <asp:Panel ID="pnlAddBaoHiem" runat="server">

            <asp:Button ID="btnAddHD" runat="server"
                Text="➕ Thêm Bảo Hiểm"
                CssClass="btn"
                OnClick="btnAddBH_Click" />

            <br /><br />

            <div class="cssadd">
                <div>
                    Mã Nhân Viên:
                    <asp:TextBox ID="txtMaNV" runat="server" />

                    Số Bảo Hiểm:
                    <asp:TextBox ID="txtSoBH" runat="server" />

                    Từ Tháng:
                    <asp:TextBox ID="txtTuThang" runat="server" TextMode="Date" />

                    Đến Tháng:
                    <asp:TextBox ID="txtDenThang" runat="server" TextMode="Date" />

                    Đơn vị:
                    <asp:TextBox ID="txtDonVi" runat="server" />
                </div>
            </div>

        </asp:Panel>

        <br />

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

        <!-- TABLE -->
        <asp:GridView ID="gvBaoHiem" runat="server"
            AutoGenerateColumns="False"
            CssClass="table"
            AllowPaging="True"
            PageSize="10"
            DataKeyNames="IDBH"
            OnPageIndexChanging="gvBaoHiem_PageIndexChanging"
            OnRowEditing="gvBaoHiem_RowEditing"
            OnRowUpdating="gvBaoHiem_RowUpdating"
            OnRowDeleting="gvBaoHiem_RowDeleting"
            OnRowCancelingEdit="gvBaoHiem_RowCancelingEdit"
            OnRowDataBound="gvBaoHiem_RowDataBound">

            <Columns>

                <asp:TemplateField HeaderText="Mã NV">
                    <ItemTemplate><%# Eval("MaNV") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtMaNV" runat="server" Text='<%# Bind("MaNV") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Số BH">
                    <ItemTemplate><%# Eval("SoBH") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSoBH" runat="server" Text='<%# Bind("SoBH") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Từ tháng">
                    <ItemTemplate><%# Eval("TuThang") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTuThang" runat="server" Text='<%# Bind("TuThang") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Đến tháng">
                    <ItemTemplate><%# Eval("DenThang") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDenThang" runat="server" Text='<%# Bind("DenThang") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Đơn vị">
                    <ItemTemplate><%# Eval("DonVi") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDonVi" runat="server" Text='<%# Bind("DonVi") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <asp:LinkButton runat="server"
                            CommandName="Edit"
                            Text="Sửa"
                            CssClass="btn btn-sm btn-info" />

                        <asp:LinkButton runat="server"
                            CommandName="Delete"
                            Text="Xóa"
                            CssClass="btn btn-sm btn-danger"
                            OnClientClick="return confirm('Xóa bảo hiểm này?');" />
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:LinkButton runat="server"
                            CommandName="Update"
                            Text="Cập nhật"
                            CssClass="btn btn-sm btn-success" />

                        <asp:LinkButton runat="server"
                            CommandName="Cancel"
                            Text="Bỏ qua"
                            CssClass="btn btn-sm btn-secondary" />
                    </EditItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>
