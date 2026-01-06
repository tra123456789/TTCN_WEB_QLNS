<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="QuanLyChamCong.aspx.cs" Inherits="TTCN_WEB_QLNS.QuanLyChamCong" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

         <div class="content">
        <h1> Quản lý Chấm Công</h1>
     
       
        <div class="breadcrumb">
            Menu › Chấm công nhân viên
        </div>

        <br />
                <div >
                     <div>
                         Ngày:
                         <asp:DropDownList ID="ddlNgay" runat="server" OnSelectedIndexChanged="ddlNgay_SelectedIndexChanged" />
            Tháng:
           <asp:DropDownList ID="ddlThang" runat="server"
    AutoPostBack="true"
    OnSelectedIndexChanged="ddlThang_SelectedIndexChanged" />

            Năm:
            <asp:DropDownList ID="ddlNam" runat="server"   AutoPostBack="true" OnSelectedIndexChanged="ddlNam_SelectedIndexChanged"></asp:DropDownList>
                         <br />
<asp:Panel ID="pnlCong" runat="server">
    Công: 
    <asp:DropDownList ID="ddlCongAll" runat="server">
        <asp:ListItem Value="1">1 công</asp:ListItem>
        <asp:ListItem Value="0.5">0.5 công</asp:ListItem>
        <asp:ListItem Value="0">Nghỉ</asp:ListItem>
    </asp:DropDownList>
        <asp:Button ID="btnChamCongAll" runat="server" Text="🧾 Chấm cho tất cả nhân viên" CssClass="btn btn-primary" OnClick="btnChamCongAll_Click" OnClientClick="return confirm('Áp công cho TẤT CẢ nhân viên trong ngày này?');" />

</asp:Panel>

<br />
                         <br />

       Bộ phận:
<asp:DropDownList ID="ddlPhongBan" runat="server" 
    AutoPostBack="true" 
    OnSelectedIndexChanged="ddlPhongBan_SelectedIndexChanged" 
    AppendDataBoundItems="true">
    <asp:ListItem Value="0">-- Chọn Bộ Phận --</asp:ListItem>
</asp:DropDownList>

Nhân viên:
<asp:DropDownList ID="ddlNhanVien" runat="server" 
    AutoPostBack="true" 
    OnSelectedIndexChanged="ddlNhanVien_SelectedIndexChanged" 
    AppendDataBoundItems="true">
    <asp:ListItem Value="0">-- Chọn Nhân Viên --</asp:ListItem>
</asp:DropDownList>
                         <asp:Button ID="btnLoad" runat="server" CssClass="btn" ForeColor="White" OnClick="btnLoad_Click" Text="Tải dữ liệu" />
                         <asp:RadioButtonList ID="rblViewMode" runat="server" RepeatDirection="Horizontal" 
    AutoPostBack="true" OnSelectedIndexChanged="rblViewMode_SelectedIndexChanged">
    <asp:ListItem Value="Day" Selected="True">Xem theo ngày</asp:ListItem>
    <asp:ListItem Value="Month">Xem cả tháng</asp:ListItem>
</asp:RadioButtonList>

        </div>


   
            
        </div>
     

<br />

        <!-- Options -->
      

        <!-- TABLE -->
       <!-- GridView chấm công -->
             Hiển thị :<br />
              <div class="search-box">
     Tìm kiếm:
     <asp:TextBox ID="txtSearch"
         runat="server"
         AutoPostBack="true"
         OnTextChanged="txtSearch_TextChanged"
         ></asp:TextBox>
 </div>
  <asp:GridView ID="gvChamCong" runat="server"
    AutoGenerateColumns="False"
      DataKeyNames="Ngay"
    CssClass="tbl" OnSelectedIndexChanged="gvChamCong_SelectedIndexChanged1" Width="603px" OnRowDataBound="gvChamCong_RowDataBound">

  <Columns>
        <asp:BoundField DataField="Thu" HeaderText="Thứ" ReadOnly="true" />
        <asp:BoundField DataField="Ngay" HeaderText="Ngày" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
      

        <asp:TemplateField HeaderText="Công">
            <ItemTemplate>
                <asp:DropDownList ID="ddlCong" runat="server" SelectedValue='<%# Bind("Cong") %>'>
                    <asp:ListItem Value="0">0</asp:ListItem>
                    <asp:ListItem Value="0.5">0.5</asp:ListItem>
                    <asp:ListItem Value="1">1</asp:ListItem>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>

     
        <asp:TemplateField HeaderText="Giờ TC">
            <ItemTemplate>
                <asp:TextBox ID="txtSoGioTC" runat="server" Width="50px" 
                    Text='<%# Bind("SoGio") %>' CssClass="note-input" />
            </ItemTemplate>
        </asp:TemplateField>

     <asp:TemplateField HeaderText="Loại ca">
    <ItemTemplate>
        <asp:Label ID="lblLoaiCa" runat="server"></asp:Label>
        <asp:HiddenField ID="hfIDLoaiCa" runat="server" />
    </ItemTemplate>
</asp:TemplateField>

        <asp:TemplateField HeaderText="Ghi chú">
            <ItemTemplate>
                <asp:TextBox ID="txtGhiChu" runat="server" CssClass="note-input" Text='<%# Bind("GhiChu") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

             <br/>
<b>Tổng công tháng:</b>
<asp:Label ID="lblTongCong"
    runat="server"
    Text="0"
    Font-Bold="true"
    ForeColor="Blue" />


             <br/>

             <br/>
        <asp:Button ID="btnSave" runat="server" Text="Lưu dữ liệu" Visible="false" OnClick="btnSave_Click" CssClass="btn" ForeColor="White" Height="42px" />
          
             <asp:Button ID="btnTongHopCong" runat="server" OnClick="btnTongHopCong_Click" Text="📊 Tổng hợp công tháng" CssClass="btn" ForeColor="White" />
          
             <asp:Button ID="btnResetCongThang" runat="server" CssClass="btn btn-warning" OnClick="btnResetCongThang_Click" OnClientClick="return confirm('Reset sẽ mở khóa công tháng. Bạn chắc chắn?');" Text="♻️ Reset công tháng đã tổng hợp"  />
          
    </div>

 </asp:Content>