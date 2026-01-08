<%@ Page Title="Home Page" Language="C#" 
    MasterPageFile="~/Public.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="TTCN_WEB_QLNS.Default" %>

<asp:Content ID="c1" ContentPlaceHolderID="TitleContent" runat="server">
    Trang chủ
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- NAV -->

        <nav class="navbar navbar-expand-lg navbar-light bg-white sticky-top shadow-sm">
            <div class="container">
               
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ms-auto align-items-center">
                        <li class="nav-item"><a class="nav-link" href="#">Trang chủ</a></li>
                        <li class="nav-item"><a class="nav-link" href="#features">Tính năng</a></li>
                        <li class="nav-item"><a class="nav-link" href="#pricing">Bảng giá</a></li>
                        <li class="nav-item"><a class="nav-link" href="#contact">Liên hệ</a></li>
                        

                    </ul>
                    <div class="d-flex align-items-center">
    
    <asp:PlaceHolder ID="plhGuest" runat="server">
        <a href="DangNhap.aspx" class="btn btn-outline-primary me-2">Đăng nhập</a>
        <a href="DangKy.aspx" class="btn btn-primary">Dùng thử</a>
    </asp:PlaceHolder>

  <asp:PlaceHolder ID="plhUser" runat="server" Visible="false">
    <div class="user-profile-header d-flex align-items-center">
        <asp:Image ID="imgAvatar" runat="server" 
            ImageUrl='<%# ResolveUrl("~/Images/default-avatar.png") %>' 
            CssClass="avatar-sm me-2" />
        
        <span class="fw-bold text-dark me-3">
            <asp:Literal ID="litFullName" runat="server"></asp:Literal>
        </span>
        
        <a id="lnkAdminDashboard" runat="server" class="btn btn-success btn-sm me-3">
            <i class="fa fa-th-large"></i> Tài khoản
        </a>

        <asp:LinkButton ID="btnLogout" runat="server" 
            CssClass="text-danger text-decoration-none fw-bold" 
            OnClick="btnLogout_Click">Đăng xuất</asp:LinkButton>
    </div>
</asp:PlaceHolder>
</div>
                </div>
            </div>
        </nav>

        <section class="hero-section text-center">
            <div class="container">
                <h1 class="display-4 fw-bold mb-4">Giải pháp quản lý nhân sự toàn diện<br>cho doanh nghiệp hiện đại</h1>
                <p class="lead mb-5">Tự động hóa quy trình nhân sự, tiết kiệm thời gian và chi phí vận hành.</p>
               <div class="d-flex justify-content-center gap-3">
    <asp:LinkButton ID="btnStart" runat="server" CssClass="btn btn-light btn-lg px-5" OnClick="btnStart_Click">
        Trải nghiệm ngay
    </asp:LinkButton>
    
    <a href="#demo" class="btn btn-outline-light btn-lg px-5">Xem Demo</a>
</div>
            </div>
        </section>

        <section id="features" class="py-5 bg-light">
            <div class="container">
                <h2 class="text-center mb-5 fw-bold">Tính năng đột phá</h2>
                <div class="row g-4 text-center">
                    <div class="col-md-3">
                        <a href="QuanlyUser.aspx" class="text-decoration-none text-dark">
                        <div class="card h-100 p-4 feature-box">
                            <i class="fas fa-user-tie fa-3x text-primary mb-3"></i>
                            <h4 >Quản lý hồ sơ</h4>
                            <p class="text-muted">Lưu trữ, tìm kiếm, cập nhật thông tin nhân viên chỉ với vài click.</p>
                        </div>
                        </a>
                    </div>
                    <div class="col-md-3">
                        <a href="QuanLyLuong.aspx" class="text-decoration-none text-dark">
                        <div class="card h-100 p-4 feature-box">
                            <i class="fas fa-clock fa-3x text-primary mb-3"></i>
                            <h4>Chấm công & Lương</h4>
                            <p class="text-muted">Tự động hóa tính lương từ bảng công, giảm sai sót tối đa.</p>
                        </div>
                        </a>
                    </div>
                    <div class="col-md-3">
                        <a href="#" class="text-decoration-none text-dark">
                        <div class="card h-100 p-4 feature-box">
                            <i class="fas fa-chart-line fa-3x text-primary mb-3"></i>
                            <h4>Báo cáo thống kê</h4>
                            <p class="text-muted">Xuất dữ liệu Excel/PDF chuyên nghiệp.</p>
                        </div>
                       </a>
                    </div>
                    <div class="col-md-3">
                        <div class="card h-100 p-4 feature-box">
                            <i class="fas fa-shield-alt fa-3x text-primary mb-3"></i>
                            <h4>Phân quyền</h4>
                            <p class="text-muted">Bảo mật dữ liệu tuyệt đối theo từng cấp bậc quản lý.</p>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section id="pricing" class="py-5">
            <div class="container">
                <h2 class="text-center mb-5 fw-bold">Gói dịch vụ linh hoạt</h2>
                <div class="row g-4 align-items-center">
                    <div class="col-md-4">
                        <div class="card p-4 pricing-card shadow-sm">
                            <h3>Cơ bản</h3>
                            <h2 class="text-primary">0đ<small class="text-muted">/tháng</small></h2>
                            <ul class="list-unstyled my-4">
                                <li><i class="fas fa-check text-success"></i> Tối đa 10 nhân viên</li>
                                <li><i class="fas fa-check text-success"></i> Chấm công cơ bản</li>
                                <li class="text-muted"><i class="fas fa-times"></i> Xuất báo cáo nâng cao</li>
                            </ul>
                            <button class="btn btn-outline-primary w-100">Đăng ký</button>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="card p-4 pricing-card featured shadow">
                            <div class="badge bg-primary mb-2">Phổ biến nhất</div>
                            <h3>Nâng cao</h3>
                            <h2 class="text-primary">499k<small class="text-muted">/tháng</small></h2>
                            <ul class="list-unstyled my-4">
                                <li><i class="fas fa-check text-success"></i> Không giới hạn nhân viên</li>
                                <li><i class="fas fa-check text-success"></i> Tính lương tự động</li>
                                <li><i class="fas fa-check text-success"></i> Xuất Excel/PDF</li>
                            </ul>
                            <button class="btn btn-primary w-100">Đăng ký ngay</button>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="card p-4 pricing-card shadow-sm">
                            <h3>Doanh nghiệp</h3>
                            <h2 class="text-primary">Liên hệ</h2>
                            <ul class="list-unstyled my-4">
                                <li><i class="fas fa-check text-success"></i> Tùy chỉnh tính năng</li>
                                <li><i class="fas fa-check text-success"></i> Hỗ trợ 24/7</li>
                                <li><i class="fas fa-check text-success"></i> Server riêng biệt</li>
                            </ul>
                            <button class="btn btn-outline-primary w-100">Liên hệ chúng tôi</button>
                        </div>
                    </div>
                </div>
            </div>
        </section>
       <section id="contact" class="py-5 bg-light">
       <div class="container">
           <div class="row">
               <div class="col-md-6">
                   <h2 class="fw-bold">Liên hệ tư vấn</h2>
                   <p>Để lại thông tin, chuyên gia của chúng tôi sẽ liên hệ lại ngay.</p>
                   <div class="mb-3">
                       <label class="form-label">Họ tên</label>
                       <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Nhập tên..."></asp:TextBox>
                   </div>
                   <div class="mb-3">
                       <label class="form-label">Email</label>
                       <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="email@example.com"></asp:TextBox>
                   </div>
                   <div class="mb-3">
                       <label class="form-label">Nội dung cần tư vấn</label>
                       <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Bạn cần hỗ trợ gì..."></asp:TextBox>
                   </div>
                   <asp:Button ID="btnSend" runat="server" Text="Gửi yêu cầu" CssClass="btn btn-primary px-5" OnClick="btnSend_Click" />
               </div>

               <div class="col-md-6">
                   <div class="ps-md-5">
                       <h4>Trụ sở chính</h4>
                       <p><i class="fas fa-map-marker-alt text-primary me-2"></i> Trâu Quỳ-Gia Lâm-Hà Nội</p>
                       <p><i class="fas fa-phone text-primary me-2"></i> 0338653836</p>
                       <p><i class="fas fa-envelope text-primary me-2"></i>vhieuk4@gmail.com</p>
                   </div>
               </div>
           </div>
       </div>
   </section>

   <footer class="bg-dark text-white py-4 text-center">
       <div class="container">
           <p class="mb-0">Bản quyền © 2026 HR-PRO SYSTEM. Thiết kế bởi Minus</p>
       </div>
   </footer>

<div id="btn-back-to-top" class="chat-circle" onclick="backToTop()" style="display: none;">
   <i class="fa-solid fa-chevron-up" style="font-size:30px; color:white">^</i>
</div>

</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptsContent" runat="server">
    <script>
        // Lấy thẻ nút
        var mybutton = document.getElementById("btn-back-to-top");

        // Khi người dùng cuộn chuột xuống 20px từ đầu trang, nút sẽ hiện ra
        window.onscroll = function () {
            scrollFunction();
        };

        function scrollFunction() {
            if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
                mybutton.style.display = "flex"; // Hiện nút
            } else {
                mybutton.style.display = "none"; // Ẩn nút
            }
        }

        // Hàm xử lý cuộn lên đầu trang
        function backToTop() {
            window.scrollTo({
                top: 0,
                behavior: 'smooth' // Cuộn mượt mà
            });
        }
     
    </script>
</asp:Content>
