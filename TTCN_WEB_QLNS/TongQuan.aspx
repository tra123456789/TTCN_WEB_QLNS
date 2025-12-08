<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TongQuan.aspx.cs" Inherits="TTCN_WEB_QLNS.TongQuan" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Tổng quan hệ thống quản lý</title>

    <link href ="TongQuan.css" rel ="stylesheet"  />

    <style>
       
    </style>
</head>
<body>
       <form id="form1" runat="server">
        <div>
             <!-- Sidebar -->
    <div class="sidebar">
        <h2>Trang quản trị</h2>
              <a id="menuTongQuan" runat="server" href="TongQuan.aspx">Tổng quan</a>
<a id="menuNhanVien" runat="server" href="QuanLyUser.aspx">Nhân viên</a>
<a id="menuPhongBan" runat="server" href="PhongBan.aspx">Phòng ban</a>
<a id="menuChamCong" runat="server" href="QuanLyChamCong.aspx">Chấm công</a>
<a id="menuHopDong" runat="server" href="QuanLyHopDong.aspx">Hợp đồng</a>
<a id="menuBaoHiem" runat="server" href="BaoHiem.aspx">Bảo hiểm xã hội</a>
<a id="menuLuong" runat="server" href="QuanLyLuong.aspx">Lương nhân viên</a>
<a id="menuKhenThuong" runat="server" href="KhenThuong.aspx">Khen thưởng</a>


    </div>

    <!-- PAGE CONTENT -->
         <div class="content">
         <div class ="welcome">          
          <asp:Label ID="lblWelcome" runat="server" CssClass="font-medium text-sm"></asp:Label>
          <p class="text-xs opacity-75">Quản trị viên</p>

         </div>
        <h1>Tổng quan hệ thống quản lý</h1>
     

        <br /><br />
                <div  >
                    
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
          

         

        </div>
                     <div>
           
        </div>


   
            
        </div>
     

<br />
            
              </div>
             
            </div>
             </form>
</body>
</html>