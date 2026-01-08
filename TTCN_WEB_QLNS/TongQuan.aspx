<%@ Page Language="C#"
    AutoEventWireup="true"
    MasterPageFile="~/Site.Master"
    CodeBehind="TongQuan.aspx.cs"
    Inherits="TTCN_WEB_QLNS.TongQuan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="content">

    <div class="welcome">
        <p class="text-xs opacity-75">Chào mừng bạn quay trở lại</p>
        <asp:Label ID="lblWelcome" runat="server"></asp:Label>
    </div>

    <h1>Tổng quan hệ thống quản lý</h1>

   
  <h3 class="mb-4">Tổng quan</h3>

  <div class="row g-4">
          <!-- Nhân viên -->
    <div class="col-md-3">
        <div class="card-dashboard bg-orange">
            <p>Tổng số nhân viên</p>
            <p class="value"><asp:Label ID="lblEmp" runat="server" Text="0"></asp:Label></p>
           <asp:LinkButton ID="btntsnv" runat="server" CssClass="btn-detail" OnClick="btntsnv_Click">Xem chi tiết</asp:LinkButton>

        </div>
    </div> 

    <!-- Phòng ban -->
    <div class="col-md-3">
        <div class="card-dashboard bg-green">
            <p>Số phòng ban</p>
            <p class="value"><asp:Label ID="lblDept" runat="server" Text="0"></asp:Label></p>
          <asp:LinkButton ID="btnpban" runat="server" CssClass="btn-detail" OnClick="btnpban_Click">Xem chi tiết</asp:LinkButton>
     </div>
    </div>

    <!-- Khen thưởng -->
 
                <!-- Hợp Đồng -->
    <div class="col-md-3">
        <div class="card-dashboard bg-blue">
            <p>Tổng số hợp đồng</p>
            <p class="value"><asp:Label ID="lblhd" runat="server" Text="0"></asp:Label></p>
           <asp:LinkButton ID="btnhd" runat="server" CssClass="btn-detail" OnClick="btnhd_Click">Xem chi tiết</asp:LinkButton>

        </div>
    </div> 

   

  
    <br />

    <!-- Cấu hình sửa thông tin -->
    <div class="card-config">
        <h4>Cho phép nhân viên chỉnh sửa thông tin</h4>

        <p>Từ ngày:</p>
        <asp:TextBox ID="txtMoTu" runat="server" TextMode="DateTimeLocal" />

        <p>Đến ngày:</p>
        <asp:TextBox ID="txtDongDen" runat="server" TextMode="DateTimeLocal" />

        <br /><br />

        <asp:Button ID="btnLuuCauHinh"
            runat="server"
            Text="Lưu cấu hình"
            CssClass="btn"
            OnClick="btnLuuCauHinh_Click" />
    </div>
      <div class="container mt-4">
    <h2 class="fw-bold mb-4">Danh sách yêu cầu tư vấn</h2>
    <asp:GridView ID="gvLienHe" runat="server" AutoGenerateColumns="False" 
        CssClass="table table-hover table-bordered shadow-sm" DataKeyNames="ID">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" />
            <asp:BoundField DataField="HoTen" HeaderText="Họ Tên" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="NgayGui" HeaderText="Ngày gửi" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:TemplateField HeaderText="Trạng thái">
                <ItemTemplate>
                    <span class='badge <%# Eval("TrangThai").ToString() == "Chưa xử lý" ? "bg-warning" : "bg-success" %>'>
                        <%# Eval("TrangThai") %>
                    </span>
                </ItemTemplate>
            </asp:TemplateField>
           <asp:TemplateField HeaderText="Thao tác">
    <ItemTemplate>
        <a href='mailto:<%# Eval("Email") %>?subject=Phản hồi hỗ trợ từ HR-PRO' class="btn btn-primary btn-sm me-2">
            <i class="fas fa-reply"></i> Trả lời
        </a>

        <asp:LinkButton ID="btnConfirm" runat="server" 
            CommandArgument='<%# Eval("ID") %>' 
            OnCommand="btnConfirm_Click"
            CssClass="btn btn-success btn-sm"
            Visible='<%# Eval("TrangThai").ToString() == "Chưa xử lý" || Eval("TrangThai") == DBNull.Value %>'>
            <i class="fas fa-check"></i> Xong
        </asp:LinkButton>
    </ItemTemplate>
</asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
</div>
    </div>
</asp:Content>
