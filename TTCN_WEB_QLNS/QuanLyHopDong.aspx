<%@ Page Language="C#"
    AutoEventWireup="true"
    MasterPageFile="~/Site.Master"
    CodeBehind="QuanLyHopDong.aspx.cs"
    Inherits="TTCN_WEB_QLNS.QuanLyHopDong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">

        <!-- Welcome -->

        <h1>Quản lý Hợp Đồng</h1>

        <div class="breadcrumb">
            Menu › Hợp Đồng
        </div>

        <br />

        <!-- ADD CONTRACT -->
        <asp:Button ID="btnAddHD"
            runat="server"
            Text="➕ Thêm Hợp Đồng"
            CssClass="btn"
            OnClick="btnAddHD_Click" />

        <br />
        <div class="top-options">
            Hiển thị
            <asp:DropDownList ID="ddlPageSize"
                runat="server"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem Selected="True">10</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
            </asp:DropDownList>

            <div class="search-box">
                Tìm kiếm:
                <asp:TextBox ID="txtSearch"
                    runat="server"
                    AutoPostBack="true"
                    OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
            </div>
        </div>

        <!-- GRID -->
        <asp:GridView ID="gvQuanLyHD"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table"
            AllowPaging="True"
            DataKeyNames="SoHD"
            OnPageIndexChanging="gvQuanLyHD_PageIndexChanging"
            OnRowEditing="gvQuanLyHD_RowEditing"
            OnRowCancelingEdit="gvQuanLyHD_RowCancelingEdit"
            OnRowUpdating="gvQuanLyHD_RowUpdating"
            OnRowDeleting="gvQuanLyHD_RowDeleting"
            OnRowCommand="gvQuanLyHD_RowCommand">

            <Columns>

                <asp:BoundField DataField="SoHD" HeaderText="Số Hợp Đồng" ReadOnly="True" />

                <asp:TemplateField HeaderText="Mã Nhân Viên">
                    <ItemTemplate><%# Eval("MaNV") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGV_MaNV" runat="server" Text='<%# Bind("MaNV") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                           <asp:TemplateField HeaderText="Ngày Bắt Đầu">
                <ItemTemplate><%# Eval("NgayBatDau", "{0:dd/MM/yyyy}") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtGV_NgayBatDau" runat="server" 
                        Text='<%# Bind("NgayBatDau", "{0:yyyy-MM-dd}") %>' 
                        TextMode="Date" CssClass="form-control" />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Ngày Kết Thúc">
                <ItemTemplate><%# Eval("NgayKetThuc", "{0:dd/MM/yyyy}") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtGV_NgayKetThuc" runat="server" 
                        Text='<%# Bind("NgayKetThuc", "{0:yyyy-MM-dd}") %>' 
                        TextMode="Date" CssClass="form-control" />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Ngày Ký">
                <ItemTemplate><%# Eval("NgayKi", "{0:dd/MM/yyyy}") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtGV_NgayKi" runat="server" 
                        Text='<%# Bind("NgayKi", "{0:yyyy-MM-dd}") %>' 
                        TextMode="Date" CssClass="form-control" />
                </EditItemTemplate>
            </asp:TemplateField>

                <asp:TemplateField HeaderText="Nội Dung">
                    <ItemTemplate><%# Eval("NoiDung") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGV_NoiDung" runat="server" Text='<%# Bind("NoiDung") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Lần Ký">
                    <ItemTemplate><%# Eval("LanKy") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGV_LanKy" runat="server" Text='<%# Bind("LanKy") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Thời Hạn">
                    <ItemTemplate><%# Eval("ThoiHan") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGV_ThoiHan" runat="server" Text='<%# Bind("ThoiHan") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

              <asp:TemplateField HeaderText="Lương cơ bản">
    <ItemTemplate>
        <%# Eval("LuongCoBan", "{0:N0}") %>
    </ItemTemplate>

    <EditItemTemplate>
        <asp:TextBox ID="txtGV_LuongCoBan"
                     runat="server"
                     Text='<%# Bind("LuongCoBan") %>'
                     CssClass="form-control" />
    </EditItemTemplate>
</asp:TemplateField>


                <asp:TemplateField HeaderText="PDF">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnExportPDF"
                            runat="server"
                            Text="📄 PDF"
                            CommandName="ExportPDF"
                            CommandArgument='<%# Eval("SoHD") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CommandName="Edit" Text="Sửa" CssClass="btn btn-sm btn-info" />
                        <asp:LinkButton runat="server" CommandName="Delete" Text="Xóa"
                            CssClass="btn btn-sm btn-danger"
                            OnClientClick="return confirm('Bạn chắc chắn muốn xóa hợp đồng này?');" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton runat="server" CommandName="Update" Text="Cập nhật" CssClass="btn btn-sm btn-success" />
                        <asp:LinkButton runat="server" CommandName="Cancel" Text="Bỏ qua" CssClass="btn btn-sm btn-secondary" />
                    </EditItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

    </div>

</asp:Content>
