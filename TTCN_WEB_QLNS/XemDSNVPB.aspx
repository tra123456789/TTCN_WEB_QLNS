<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="XemDSNVPB.aspx.cs" Inherits="TTCN_WEB_QLNS.XemDSNVPB" %>


<asp:Content ContentPlaceHolderID="MainContent" runat="server">

     <div class="content">

        <h1>Danh sách nhân viên theo phòng ban</h1>

        <div class="breadcrumb">
            Menu › Phòng ban › Nhân viên
        </div>

        <br />

        <asp:Label ID="lblPhongBan" runat="server"
            Font-Bold="true" Font-Size="Large" />

        <br /><br />

        <asp:GridView ID="gvNhanVien" runat="server"
            AutoGenerateColumns="False"
            CssClass="table">


    <Columns>

        <asp:TemplateField HeaderText="Mã NV">
            <ItemTemplate><%# Eval("MaNV") %></ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Họ tên">
            <ItemTemplate><%# Eval("HoTen") %></ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Bộ phận">
            <ItemTemplate><%# Eval("TenBP") %></ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Phòng ban">
            <ItemTemplate><%# Eval("TenPB") %></ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Lương cơ bản">
            <ItemTemplate>
                <%# Eval("LuongCoBan", "{0:N0}") %>
            </ItemTemplate>
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