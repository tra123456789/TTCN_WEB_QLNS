<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="QuanLyChamCong.aspx.cs" Inherits="TTCN_WEB_QLNS.QuanLyChamCong" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

         <div class="content">
        <h1> Quản lý Chấm Công</h1>
     
       
        <div class="breadcrumb">
            Menu › Chấm công nhân viên
        </div>

        <br /><br />
                <div >
                     <div>
            Tháng:
            <asp:DropDownList ID="ddlThang" runat="server" OnSelectedIndexChanged="ddlThang_SelectedIndexChanged"></asp:DropDownList>

            Năm:
            <asp:DropDownList ID="ddlNam" runat="server" OnSelectedIndexChanged="ddlNam_SelectedIndexChanged"></asp:DropDownList>

            Nhân viên:
            <asp:DropDownList ID="ddlNhanVien" runat="server" OnSelectedIndexChanged="ddlNhanVien_SelectedIndexChanged"></asp:DropDownList>

            <asp:Button ID="btnLoad" runat="server" Text="Tải dữ liệu" OnClick="btnLoad_Click" />
        </div>


   
            
        </div>
     

<br />

        <!-- Options -->
        <div class="top-options">
            Hiển thị
            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem Selected="True">10</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
            </asp:DropDownList>

            <div class="search-box">
                Tìm kiếm:
                <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="true"
                    OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
            </div>
        </div>

        <!-- TABLE -->
       <!-- GridView chấm công -->
        <asp:GridView ID="gvChamCong" runat="server" AutoGenerateColumns="False" CssClass="tbl" OnSelectedIndexChanged="gvChamCong_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="Ngay" HeaderText="Ngày" ReadOnly="true" />
                <asp:BoundField DataField="Thu" HeaderText="Thứ" ReadOnly="true" />

                <asp:TemplateField HeaderText="Giờ vào">
                    <ItemTemplate>
                        <asp:TextBox ID="txtGioVao" runat="server" CssClass="time-input"
                            Text='<%# Bind("GioVao") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Giờ ra">
                    <ItemTemplate>
                        <asp:TextBox ID="txtGioRa" runat="server" CssClass="time-input"
                            Text='<%# Bind("GioRa") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="CongNgay" HeaderText="Công " ReadOnly="true" />

                <asp:TemplateField HeaderText="Ghi chú">
                    <ItemTemplate>
                        <asp:TextBox ID="txtGhiChu" runat="server" CssClass="note-input"
                            Text='<%# Bind("GhiChu") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
             
        <asp:Button ID="btnTinhCong" runat="server" Text="Tính công" OnClick="btnTinhCong_Click" />
        <asp:Button ID="btnSave" runat="server" Text="Lưu dữ liệu" OnClick="btnSave_Click" />

    </div>

 </asp:Content>