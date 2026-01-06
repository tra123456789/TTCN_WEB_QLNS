<%@ Page Language="C#" AutoEventWireup="true"     MasterPageFile="~/Site.Master" CodeBehind="ThemHopDong.aspx.cs" Inherits="TTCN_WEB_QLNS.ThemHopDong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">


   
  <style>
.cssadd {
    width: 560px;
    margin: 40px auto;
    padding: 30px 35px;
    background: #ffffff;
    border-radius: 10px;
    box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
    font-family: Arial, sans-serif;
}

.cssadd h2 {
    text-align: center;
    margin-bottom: 25px;
    color: #333;
}

.form-group {
    margin-bottom: 15px;
}

.form-group label {
    display: block;
    font-weight: 600;
    margin-bottom: 6px;
    color: #444;
}

.form-group input,
.form-group select {
    width: 100%;
    padding: 10px 12px;
    border-radius: 6px;
    border: 1px solid #ccc;
    font-size: 14px;
    transition: all 0.3s ease;
}

.form-group input:focus,
.form-group select:focus {
    border-color: #007bff;
    outline: none;
    box-shadow: 0 0 5px rgba(0,123,255,0.4);
}

#btnAddHD { margin-top: 20px; width: 100%; padding: 12px; background: #007bff; color: #fff; border: none; border-radius: 6px; font-weight: bold; cursor: pointer; }
#btnAddHD:hover {
    background: linear-gradient(135deg, #0056b3, #003f88);
    transform: translateY(-2px);
}
</style>

        <div>
           
            <div class="cssadd">
    <h2>THÊM / GIA HẠN HỢP ĐỒNG</h2>

    <div class="form-group">
        <label>Mã nhân viên</label>
        <asp:DropDownList ID="ddlNhanVien" runat="server"></asp:DropDownList>
        <asp:TextBox ID="txtMaNV" runat="server" Visible="false"></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Ngày bắt đầu</label>
        <asp:TextBox ID="txtNgayBatDau" runat="server" TextMode="Date"></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Ngày kết thúc</label>
        <asp:TextBox ID="txtNgayKetThuc" runat="server" TextMode="Date"></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Ngày ký</label>
        <asp:TextBox ID="txtNgayKi" runat="server" TextMode="Date"></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Nội dung hợp đồng</label>
        <asp:TextBox ID="txtNoiDung" runat="server"></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Lần ký</label>
        <asp:TextBox ID="txtLanKy" runat="server"></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Thời hạn</label>
        <asp:TextBox ID="txtThoiHan" runat="server"></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Lương cơ bản</label>
        <asp:TextBox ID="txtLuongCoBan" runat="server"></asp:TextBox>
    </div>

    <asp:Button
        ID="btnAddHD"
        runat="server"
        Text="Lưu hợp đồng"
        OnClick="btnAddHD_Click" />
</div>
            
    </div>
        
    </div>

</asp:Content>
