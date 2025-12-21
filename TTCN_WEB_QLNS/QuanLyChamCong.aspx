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
           <asp:DropDownList ID="ddlThang" runat="server"
    AutoPostBack="true"
    OnSelectedIndexChanged="ddlThang_SelectedIndexChanged" />

            Năm:
            <asp:DropDownList ID="ddlNam" runat="server"   AutoPostBack="true" OnSelectedIndexChanged="ddlNam_SelectedIndexChanged"></asp:DropDownList>

            Nhân viên:
            <asp:DropDownList ID="ddlNhanVien" runat="server"   AutoPostBack="true" OnSelectedIndexChanged="ddlNhanVien_SelectedIndexChanged"></asp:DropDownList>

            <asp:Button ID="btnLoad" runat="server" Text="Tải dữ liệu" OnClick="btnLoad_Click" />
        </div>


   
            
        </div>
     

<br />

        <!-- Options -->
      

        <!-- TABLE -->
       <!-- GridView chấm công -->
             Hiển thị :<br />
  <asp:GridView ID="gvChamCong" runat="server"
    AutoGenerateColumns="False"
      DataKeyNames="Ngay"
    CssClass="tbl" OnSelectedIndexChanged="gvChamCong_SelectedIndexChanged1" Width="603px" OnRowDataBound="gvChamCong_RowDataBound">

    <Columns>
      
        <asp:BoundField DataField="Ngay"
            HeaderText="Ngày"
            DataFormatString="{0:dd/MM/yyyy}"
            ReadOnly="true" />

        <asp:BoundField DataField="Thu"
            HeaderText="Thứ"
            ReadOnly="true" />

  
        <asp:TemplateField HeaderText="Công">
            <ItemTemplate>
                <asp:DropDownList ID="ddlCong" runat="server"
                    SelectedValue='<%# Bind("Cong") %>'>
                    <asp:ListItem Value="0">0</asp:ListItem>
                    <asp:ListItem Value="0.5">0.5</asp:ListItem>
                    <asp:ListItem Value="1">1</asp:ListItem>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>

    
        <asp:TemplateField HeaderText="Ghi chú">
            <ItemTemplate>
                <asp:TextBox ID="txtGhiChu" runat="server"
                    CssClass="note-input"
                    Text='<%# Bind("GhiChu") %>' />
            </ItemTemplate>
        </asp:TemplateField>
     

    </Columns>
</asp:GridView>

             <br/>
            <b>Tổng công tháng:</b>
            <asp:Label ID="lblTongCong" runat="server" />

             <br/>

             <br/>
        <asp:Button ID="btnTinhCong" runat="server" Text="Tính công" OnClick="btnTinhCong_Click" />
        <asp:Button ID="btnSave" runat="server" Text="Lưu dữ liệu" OnClick="btnSave_Click" style="height: 26px" />
          
             <asp:Button ID="btnTongHopCong" runat="server" OnClick="btnTongHopCong_Click" Text="📊 Tổng hợp công tháng" />
          
    </div>

 </asp:Content>