<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserHome.aspx.cs" Inherits="TTCN_WEB_QLNS.UserHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
    <link href ="Quanlynhanvien.css" rel ="stylesheet" />
    <style>
       


    </style>
</head>
<body>
          <form id="form1" runat="server">
        <div>
             <!-- Sidebar -->
    <div class="sidebar">
        <h2>Menu</h2>
            
   <a id="menuThongTinNV" runat="server" href="ThongTinCaNhan.aspx">Thông Tin cá nhân</a>
   <a id="menuChamCong" runat="server" href="QuanLyChamCong.aspx">Chấm công</a>
   <a id="menuBaoHiem" runat="server" href="BaoHiem.aspx">Bảo hiểm xã hội</a>
   <a id="menuLuong" runat="server" href="QuanLyLuong.aspx">Lương nhân viên</a>

         <!-- THÊM Ô ĐĂNG XUẤT -->
        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="logout-link" OnClick="lnkLogout_Click">Đăng xuất</asp:LinkButton>
        

    </div>

    <!-- PAGE CONTENT -->
         <div class="content">
       <div class ="welcome">   
           <p class="text-xs opacity-75">Chào mừng bạn quay trở lại</p>
 <asp:Label ID="lblWelcome" runat="server" CssClass="font-medium text-sm"></asp:Label>
   

</div>
        <h1>Chào mừng đến hệ thống</h1>
     <p> Hãy chọn chức năng từ bên trái menu để bắt đầu</p>

        <br /><br />
                <div  >
               
            </div>
             </div>
         </form>
</body>
</html>
