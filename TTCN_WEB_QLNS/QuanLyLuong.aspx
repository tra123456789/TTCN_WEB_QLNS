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
        


        <br />

       <asp:Button ID="btnTinhLuong" runat="server"
    Text="🧮 Tính lương tháng"
    CssClass="btn btn-success"
    OnClick="btnTinhLuong_Click" />

<asp:Button ID="btnExportExcel" runat="server"
    Text="📥 Xuất Excel bảng lương"
    CssClass="btn"
    OnClick="btnExportExcel_Click" />

        <asp:Button ID="btnChotLuong" runat="server"
    Text="🔒 Chốt lương tháng"
    CssClass="btn btn-danger"
    OnClick="btnChotLuong_Click"
    OnClientClick="return confirm('Chốt lương sẽ KHÔNG thể sửa lại. Bạn chắc chắn?');" />

        <br /><br />

        <!-- top options -->
        <div class="top-options">
            Hiển thị:
            <asp:DropDownList ID="ddlPageSize" runat="server"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem Selected="True">10</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
            </asp:DropDownList>

            <br />
            <br />
            Tháng:
            <asp:DropDownList ID="ddlThang" runat="server" 
            AutoPostBack="true"
    OnSelectedIndexChanged="ddlThang_SelectedIndexChanged" />
            Năm:
            <asp:DropDownList ID="ddlNam" runat="server" 
            AutoPostBack="true"
    OnSelectedIndexChanged="ddlThang_SelectedIndexChanged" />
            <br />

            <br />

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
    PageSize="10" OnSelectedIndexChanged="gvLuong_SelectedIndexChanged">

    <Columns>

      
        <asp:BoundField DataField="MaNV" HeaderText="Mã NV" />

        
        <asp:BoundField DataField="HoTen" HeaderText="Họ tên" />

       
        <asp:BoundField DataField="TenPB" HeaderText="Phòng ban" />

        
        <asp:BoundField DataField="LuongCoBan"
            HeaderText="Lương cơ bản"
            DataFormatString="{0:N0}" />

      
        <asp:BoundField DataField="TongNgayCong"
            HeaderText="Ngày công" />

       
        <asp:BoundField DataField="TongThuong"
            HeaderText="Thưởng"
            DataFormatString="{0:N0}" />

       
        <asp:BoundField DataField="TongPhat"
            HeaderText="Phạt"
            DataFormatString="{0:N0}" />

        
        <asp:BoundField DataField="BHXH"
            HeaderText="BHXH (8%)"
            DataFormatString="{0:N0}" />

    
        <asp:BoundField DataField="BHYT"
            HeaderText="BHYT (1.5%)"
            DataFormatString="{0:N0}" />

       
        <asp:BoundField DataField="BHTN"
            HeaderText="BHTN (1%)"
            DataFormatString="{0:N0}" />

      
        <asp:BoundField DataField="ThucLanh"
            HeaderText="Thực lãnh"
            DataFormatString="{0:N0}" />

    </Columns>
</asp:GridView>


    </div>

</asp:Content>
