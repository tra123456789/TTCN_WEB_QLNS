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

      <!-- Bảng lương -->
      <div class="col-md-3">
          <div class="card-dashboard bg-blue">
              <p>Hệ số bảng lương</p>
              <p class="value"><asp:Label ID="lblSalary" runat="server" Text="0"></asp:Label></p>
       <asp:LinkButton ID="btnbluong" runat="server" CssClass="btn-detail" OnClick="btnbluong_Click">Xem chi tiết</asp:LinkButton>
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
      <div class="col-md-3">
          <div class="card-dashboard bg-red">
              <p>Khen thưởng</p>
              <p class="value"><asp:Label ID="lblReward" runat="server" Text="0"></asp:Label></p>
            <asp:LinkButton ID="btnkthuong" runat="server" CssClass="btn-detail" OnClick="btnkthuong_Click">Xem chi tiết</asp:LinkButton>
   </div>
      </div>
                  <!-- Hợp Đồng -->
      <div class="col-md-3">
          <div class="card-dashboard bg-orange">
              <p>Tổng số hợp đồng</p>
              <p class="value"><asp:Label ID="lblhd" runat="server" Text="0"></asp:Label></p>
             <asp:LinkButton ID="btnhd" runat="server" CssClass="btn-detail" OnClick="btnhd_Click">Xem chi tiết</asp:LinkButton>

          </div>
      </div> 

      <!-- Bảo Hiểm -->
      <div class="col-md-3">
          <div class="card-dashboard bg-blue">
              <p>Số bảo hiểm</p>
              <p class="value"><asp:Label ID="lblbh" runat="server" Text="0"></asp:Label></p>
       <asp:LinkButton ID="btnbh" runat="server" CssClass="btn-detail" OnClick="btnbh_Click">Xem chi tiết</asp:LinkButton>
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

</div>
    </div>
</asp:Content>
