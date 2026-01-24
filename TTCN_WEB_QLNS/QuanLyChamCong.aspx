<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="QuanLyChamCong.aspx.cs" Inherits="TTCN_WEB_QLNS.QuanLyChamCong" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <meta http-equiv="refresh" content="300">

         <div class="content">
        <h1> Quản lý Chấm Công</h1>
     
       
        <div class="breadcrumb">
            Menu › Chấm công nhân viên
        </div>
          <div class="d-flex align-items-center justify-content-between">
   <div ID="notify" runat ="server" class="notify-box" title="Yêu cầu chờ duyệt" onclick="openThongBao() ">
    <i class="fas fa-bell"></i>
    <span class="badge" style="width: 18px">
        <asp:Literal ID="ltrCount" runat="server">0</asp:Literal>
    </span>
</div>

</div>


        <br />
<div class="card shadow-sm border-0">
    <div class="card-header bg-primary text-white fw-bold py-3">
        <i class="fas fa-file-invoice me-2"></i> 🧾 Chấm Công Nhân Viên
    </div>
    <div class="card-body p-4">
        <div class="row g-2 mb-4 align-items-end">
            <div class="col-md-1">
                <label class="form-label small fw-bold">Ngày:</label>
                <asp:DropDownList ID="ddlNgay" runat="server" CssClass="form-select form-select-sm" 
                    AutoPostBack="true" OnSelectedIndexChanged="ddlNgay_SelectedIndexChanged" />
            </div>
            <div class="col-md-1">
                <label class="form-label small fw-bold">Tháng:</label>
                <asp:DropDownList ID="ddlThang" runat="server" CssClass="form-select form-select-sm"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlThang_SelectedIndexChanged" />
            </div>
            <div class="col-md-2">
                <label class="form-label small fw-bold">Năm:</label>
                <asp:DropDownList ID="ddlNam" runat="server" CssClass="form-select form-select-sm"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlNam_SelectedIndexChanged" />
            </div>

           <div class="col-md-3" id="pnlPhongBan" runat="server">
    <label class="form-label small fw-bold">Bộ phận:</label>
    <asp:DropDownList ID="ddlPhongBan" runat="server" 
        CssClass="form-select form-select-sm select-search"
        AutoPostBack="true" 
        OnSelectedIndexChanged="ddlPhongBan_SelectedIndexChanged"
        AppendDataBoundItems="true">
        <asp:ListItem Value="0">-- Chọn bộ phận --</asp:ListItem>
    </asp:DropDownList>
</div>

<div class="col-md-3" id="pnlNhanVien" runat="server">
    <label class="form-label small fw-bold">Nhân viên:</label>
    <asp:DropDownList ID="ddlNhanVien" runat="server" 
        CssClass="form-select form-select-sm select-search"
        AutoPostBack="true" 
        OnSelectedIndexChanged="ddlNhanVien_SelectedIndexChanged"
        AppendDataBoundItems="true">
        <asp:ListItem Value="0">-- Chọn nhân viên --</asp:ListItem>
    </asp:DropDownList>
</div>

           <div class="row mb-3">
    <div class="col-12">
        <asp:Button ID="btnUserChamCong"
            runat="server"
            CssClass="btn btn-success w-100"
            Text="Chấm công hôm nay" />
    </div>
</div>
        </div>

        <hr class="text-secondary opacity-25" />

        <asp:Panel ID="pnlCong" runat="server" CssClass="bg-light p-3 rounded border">
            <div class="row g-3 align-items-center">
                <div class="col-auto">
                    <label class="form-label mb-0 fw-bold text-success">Chấm nhanh cho ngày đã chọn:</label>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlCongAll" runat="server" CssClass="form-select form-select-sm">
                        <asp:ListItem Value="1">✅ 1 công</asp:ListItem>
                        <asp:ListItem Value="0.5">🌗 0.5 công</asp:ListItem>
                        <asp:ListItem Value="0">❌ Nghỉ</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-auto">
                    <asp:Button ID="btnChamCongAll" runat="server" Text="🧾 Áp dụng cho tất cả"
                        CssClass="btn btn-success btn-sm"
                        OnClick="btnChamCongAll_Click"
                        OnClientClick="return confirm('Áp công cho TẤT CẢ nhân viên trong ngày này?');" />
                </div>
            </div>
        </asp:Panel>
    </div>
</div>     

<br />

        <!-- Options -->
      

        <!-- TABLE -->
       <!-- GridView chấm công -->
             Hiển thị :<br />

             
 <asp:UpdatePanel ID="upThongBao" runat="server">
    <ContentTemplate>
  <div class="table-responsive">
    <table class="table-chamcong" border="1" style="border-collapse: collapse; text-align: center; width: 100%;">
        <thead>
            <tr class="header-blue">
                <th rowspan="2">STT</th>
                <th rowspan="2">Họ và tên</th>
                <th rowspan="2">Chức vụ</th>
                <th colspan='<%= SoNgayTrongThang %>'>Ngày trong tháng</th>

                <th rowspan="2">Tổng công</th>
                <th rowspan="2">Ghi chú</th>
            </tr>
            <tr class="header-blue">
                <asp:Repeater ID="rptHeaderNgay" runat="server">
                <ItemTemplate>
                    <th class='col-day <%# Eval("CssClass") %>'>
                        <div style="font-size: 10px;"><%# Eval("Ngay") %></div>
                        <div class="header-rotate">
                            <div><%# Eval("Thu") %></div>
                        </div>
                    </th>
                </ItemTemplate>
            </asp:Repeater>
            </tr>
        </thead>
        <tbody>
           <asp:Repeater ID="rptNhanVien" runat="server" OnItemDataBound="rptNhanVien_ItemDataBound">
    <ItemTemplate>
        <tr>
            <asp:HiddenField ID="hfMaNV" runat="server" Value='<%# Eval("MaNV") %>' />
            
            <td><%# Container.ItemIndex + 1 %></td>
            <td><%# Eval("HoTen") %></td>
            <td><%# Eval("TenChucVu") %></td>
            
          <asp:Repeater ID="rptNgayCong" runat="server">
    <ItemTemplate>
        <asp:HiddenField ID="hfNgay" runat="server"
            Value='<%# Eval("Ngay") %>' />

        <td class='<%# Eval("CssClassName") %>'>
            <asp:TextBox ID="txtCong" runat="server"
                Text='<%# Eval("GiaTriCong") %>' 
                CssClass="txt-chamcong" />
        </td>
    </ItemTemplate>
</asp:Repeater>


            <td><asp:Label ID="lblTong" runat="server" Text="0"></asp:Label></td>
            <td><asp:TextBox ID="txtGhiChu" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("GhiChu") %>'></asp:TextBox></td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
        </tbody>
    </table>
</div>
             <br/>

        

             <br/>
        <asp:Button ID="btnSave" runat="server" Text="Lưu dữ liệu" Visible="false" OnClick="btnSave_Click" CssClass="btn btn-primary" Height="47px" Width="143px" />
          
             <asp:Button ID="btnTongHopCong" runat="server" OnClick="btnTongHopCong_Click" Text="📊 Tổng hợp công tháng" CssClass="btn btn-primary"/>
          
             <asp:Button ID="btnResetCongThang" runat="server" CssClass="btn btn-warning" OnClick="btnResetCongThang_Click" OnClientClick="return confirm('Reset sẽ mở khóa công tháng. Bạn chắc chắn?');" Text="♻️ Reset công tháng đã tổng hợp"  />
          
    </div>
        <div id="modalThongBao" class="modal">
  <div class="modal-content">
    <span class="close" onclick="closeThongBao()">×</span>

    <h4>Yêu cầu chấm công chưa xử lý</h4>


        <asp:GridView ID="gvThongBao" runat="server"
    AutoGenerateColumns="false"
    CssClass="table"
    OnRowCommand="gvThongBao_RowCommand">

            <Columns>
                <asp:BoundField DataField="HoTen" HeaderText="Họ Tên" />
                <asp:BoundField DataField="Ngay" HeaderText="Ngày"
                    DataFormatString="{0:dd/MM HH:mm}" />
                <asp:BoundField DataField="GhiChu" HeaderText="Nội dung yêu cầu" />
                  <asp:TemplateField HeaderText="Hành động"
    ItemStyle-Width="90px"
    ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>

        <asp:Button runat="server"
            Text="✔"
            ToolTip="Duyệt"
            CssClass="btn btn-success btn-sm action-btn"
            CommandName="DUYET"
            CommandArgument='<%# Eval("MaNV") + "|" + Eval("Ngay") %>' />

        <asp:Button runat="server"
            Text="✖"
            ToolTip="Từ chối"
            CssClass="btn btn-danger btn-sm action-btn"
            CommandName="TUCHOI"
            CommandArgument='<%# Eval("MaNV") + "|" + Eval("Ngay") %>' />

    </ItemTemplate>
</asp:TemplateField>

            </Columns>
        </asp:GridView>

    </ContentTemplate>
</asp:UpdatePanel>


  
</div>

  <script type="text/javascript">
      // 1. Hàm tính tổng công từng hàng
      function TinhTongCongTungHang() {
          $('.table-chamcong tbody tr').each(function () {
              var row = $(this);
              var tong = 0;

              row.find('.txt-chamcong').each(function () {
                  var rawValue = $(this).val();
                  if (rawValue && rawValue.trim() !== "") {
                      // Thay dấu phẩy thành dấu chấm để parseFloat hiểu đúng
                      var cleanValue = rawValue.replace(',', '.');
                      var num = parseFloat(cleanValue);

                      if (!isNaN(num)) {
                          // Tính toán chính xác số thập phân
                          tong = Math.round((tong + num) * 100) / 100;
                      }
                  }
              });

              // Hiển thị kết quả: Nếu là số nguyên thì hiện nguyên, lẻ thì hiện tối đa 1 chữ số
              var displayTong = (tong % 1 === 0) ? tong : tong.toFixed(1);
              row.find('[id*="lblTong"]').text(displayTong);
          });
      }

      function openThongBao() {
          document.getElementById("modalThongBao").style.display = "block";
      }

      function closeThongBao() {
          document.getElementById("modalThongBao").style.display = "none";
      }

      // 2. Hàm khởi tạo Select2
      function applySelect2() {
          if ($('.select-search').length > 0) { // Kiểm tra xem có phần tử nào không mới chạy
              $('.select-search').select2({
                  placeholder: "-- Chọn giá trị --",
                  allowClear: true,
                  width: '100%',
                  language: {
                      noResults: function () { return "Không tìm thấy kết quả"; }
                  }
              });
          }
      }

      // 3. Khởi tạo khi trang tải xong (Dùng một ready duy nhất cho gọn)
      $(document).ready(function () {
          applySelect2();
          TinhTongCongTungHang();
      });

      // 4. Gắn sự kiện cho bảng chấm công
      $(document).on('input change', '.txt-chamcong', function () {
          TinhTongCongTungHang();
      });

      // 5. Xử lý cho UpdatePanel (ASP.NET WebForms)
      if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
          var prm = Sys.WebForms.PageRequestManager.getInstance();
          prm.add_endRequest(function (sender, e) {
              applySelect2();
              // Nếu bảng chấm công nằm trong UpdatePanel thì cần chạy lại dòng dưới
              TinhTongCongTungHang();
          });
      }

  </script>
 </asp:Content>