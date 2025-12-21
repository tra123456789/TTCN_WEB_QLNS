<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Site.Master"
    CodeBehind="KhenThuong.aspx.cs"
    Inherits="TTCN_WEB_QLNS.KhenThuong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">

        <div class="welcome">
            <p class="text-xs opacity-75">Chào mừng bạn quay trở lại</p>
            <asp:Label ID="lblWelcome" runat="server" CssClass="font-medium text-sm"></asp:Label>
        </div>

        <h1>Khen thưởng nhân viên</h1>

        <div class="breadcrumb">
            Menu › Khen thưởng
        </div>

        <br />

        <a href="ThemKhenThuong.aspx" class="btn">➕ Khen thưởng</a>

        <br /><br />

        <!-- Options -->
        <div class="top-options">
            <div class="top-options">

    Tháng:
    <asp:DropDownList ID="ddlThang" runat="server"
        AutoPostBack="true"
        OnSelectedIndexChanged="FilterChanged" />

    Năm:
    <asp:DropDownList ID="ddlNam" runat="server"
        AutoPostBack="true"
        OnSelectedIndexChanged="FilterChanged" />
                <asp:Button ID="btnExportExcel" runat="server"
    Text="📥 Xuất Excel"
    CssClass="btn"
    OnClick="btnExportExcel_Click" />

</div>

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
        <asp:GridView ID="gvKhenThuong" runat="server"
            AutoGenerateColumns="False"
            CssClass="table"
            AllowPaging="True"
            PageSize="10"
            OnPageIndexChanging="gvKhenThuong_PageIndexChanging">

            <Columns>

                <asp:BoundField DataField="MaNV" HeaderText="Mã NV" />
                <asp:BoundField DataField="HoTen" HeaderText="Họ và tên" />
                <asp:BoundField DataField="Ngay"
                    HeaderText="Ngày"
                    DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="NoiDung" HeaderText="Nội dung" />
                <asp:BoundField DataField="SoKTKL"
                    HeaderText="Số tiền"
                    DataFormatString="{0:N0}" />

                <asp:TemplateField HeaderText="Loại">
                    <ItemTemplate>
                        <%# Convert.ToInt32(Eval("Loai")) == 1 ? "Khen thưởng" : "Kỷ luật" %>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

    </div>

</asp:Content>
