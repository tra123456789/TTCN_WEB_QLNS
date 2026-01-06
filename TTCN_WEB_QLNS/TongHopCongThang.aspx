<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.Master" CodeBehind="TongHopCongThang.aspx.cs" Inherits="TTCN_WEB_QLNS.TongHopCongThang" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">

    <style>
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid #ccc; padding: 8px; text-align: center; }
        th { background-color: #f2f2f2; }
        .filter { margin-bottom: 15px; }
    </style>

        <h2>TỔNG HỢP CÔNG THÁNG</h2>
            <div class="breadcrumb">
                Menu › Phòng ban › Nhân viên
        </div>
&nbsp;<div class="filter">

            Tháng:
            <asp:DropDownList ID="ddlThang" runat="server" />
            Năm:
            <asp:DropDownList ID="ddlNam" runat="server" />

            <asp:Button ID="btnXem" runat="server"
                Text="Xem tổng hợp"
                OnClick="btnXem_Click" />
        </div>

        <asp:GridView ID="gvTongHopCong" runat="server"
            AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="MaNV" HeaderText="Mã NV" />
                <asp:BoundField DataField="HoTen" HeaderText="Họ tên" />
                <asp:BoundField DataField="Thang" HeaderText="Tháng" />
                <asp:BoundField DataField="Nam" HeaderText="Năm" />
                <asp:BoundField DataField="TongCong" HeaderText="Tổng công" />
            </Columns>
        </asp:GridView>

        <br />
        <asp:Label ID="lblThongBao" runat="server" ForeColor="Red" />
           </div>

</asp:Content>
