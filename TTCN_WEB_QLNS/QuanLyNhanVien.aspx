<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuanLyNhanVien.aspx.cs" Inherits="TTCN_WEB_QLNS.QuanLyNhanVien" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Quản Lý Nhân Viên</title>
  <script src="https://cdn.tailwindcss.com"></script>
  <script src="/_sdk/data_sdk.js"></script>
  <script src="/_sdk/element_sdk.js"></script>
  <style>
    body {
      box-sizing: border-box;
      margin: 0;
      padding: 0;
      font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    }
    
    * {
      box-sizing: border-box;
    }
    
    html, body {
      height: 100%;
    }

    .table-row {
      transition: background-color 0.2s;
    }

    .table-row:hover {
      background-color: rgba(0, 0, 0, 0.02);
    }

    .modal {
      display: none;
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background-color: rgba(0, 0, 0, 0.5);
      z-index: 1000;
      animation: fadeIn 0.3s;
    }

    .modal.active {
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .modal-content {
      max-width: 600px;
      width: 90%;
      max-height: 90%;
      overflow-y: auto;
      animation: slideUp 0.3s;
    }

    @keyframes fadeIn {
      from { opacity: 0; }
      to { opacity: 1; }
    }

    @keyframes slideUp {
      from { transform: translateY(50px); opacity: 0; }
      to { transform: translateY(0); opacity: 1; }
    }

    .status-badge {
      display: inline-block;
      padding: 6px 16px;
      border-radius: 20px;
      font-size: 13px;
      font-weight: 600;
    }

    .input-field {
      width: 100%;
      padding: 10px 12px;
      border: 1px solid #ddd;
      border-radius: 8px;
      font-size: 14px;
      transition: border-color 0.3s;
    }

    .input-field:focus {
      outline: none;
      border-color: #4f46e5;
    }

    .btn {
      padding: 10px 20px;
      border: none;
      border-radius: 8px;
      font-weight: 600;
      cursor: pointer;
      transition: all 0.3s;
    }

    .btn:hover {
      opacity: 0.9;
      transform: translateY(-1px);
    }

    .btn:disabled {
      opacity: 0.5;
      cursor: not-allowed;
      transform: none;
    }

    .action-btn {
      padding: 6px 12px;
      border: none;
      border-radius: 6px;
      font-size: 13px;
      font-weight: 600;
      cursor: pointer;
      transition: all 0.2s;
    }

    .action-btn:hover {
      opacity: 0.8;
    }
  </style>
  <style>@view-transition { navigation: auto; }</style>
 </head>
 <body>
  <div id="app" class="min-h-full p-6"><!-- Header -->
   <header class="mb-6">
    <div class="flex justify-between items-center flex-wrap gap-4 mb-6">
     <div>
      <h1 id="page-title" class="text-3xl font-bold mb-2"></h1>
     </div><button id="add-employee-btn" class="btn flex items-center gap-2"> <span class="text-xl">➕</span> <span id="add-button-text"></span> </button>
    </div><!-- Search Bar -->
    <div class="mb-4">
     <div class="flex gap-4 flex-wrap">
      <div class="flex-1 min-w-[250px]"><input type="text" id="search-input" placeholder="🔍 Tìm kiếm theo tên, mã NV, email, số điện thoại..." class="input-field">
      </div><select id="department-filter" class="input-field" style="min-width: 180px;"> <option value="">🏢 Tất cả phòng ban</option> <option value="Công nghệ">Công nghệ</option> <option value="Nhân sự">Nhân sự</option> <option value="Marketing">Marketing</option> <option value="Tài chính">Tài chính</option> <option value="Kinh doanh">Kinh doanh</option> </select> <select id="status-filter" class="input-field" style="min-width: 180px;"> <option value="">📊 Tất cả trạng thái</option> <option value="Đang làm việc">Đang làm việc</option> <option value="Nghỉ phép">Nghỉ phép</option> <option value="Thử việc">Thử việc</option> </select>
     </div>
    </div>
   </header><!-- Employee Table -->
   <div class="rounded-xl shadow-lg overflow-hidden">
    <div class="overflow-x-auto">
     <table class="w-full">
      <thead>
       <tr class="border-b-2">
        <th class="text-left p-4 font-bold">Mã NV</th>
        <th class="text-left p-4 font-bold">Họ và tên</th>
        <th class="text-left p-4 font-bold">Email</th>
        <th class="text-left p-4 font-bold">Số điện thoại</th>
        <th class="text-left p-4 font-bold">Phòng ban</th>
        <th class="text-left p-4 font-bold">Chức vụ</th>
        <th class="text-left p-4 font-bold">Ngày vào làm</th>
        <th class="text-left p-4 font-bold">Trạng thái</th>
        <th class="text-center p-4 font-bold">Thao tác</th>
       </tr>
      </thead>
      <tbody id="employee-table-body"><!-- Employee rows will be inserted here -->
      </tbody>
     </table>
    </div><!-- Empty State -->
    <div id="empty-state" class="text-center py-16 hidden">
     <div class="text-6xl mb-4">
      👥
     </div>
     <h3 class="text-xl font-bold mb-2">Không tìm thấy nhân viên</h3>
     <p class="opacity-75 mb-6">Thử điều chỉnh bộ lọc hoặc thêm nhân viên mới</p><button id="empty-add-btn" class="btn"> ➕ Thêm nhân viên mới </button>
    </div>
   </div>
  </div><!-- Add/Edit Employee Modal -->
  <div id="employee-modal" class="modal">
   <div class="modal-content rounded-xl shadow-2xl p-6">
    <div class="flex justify-between items-center mb-6">
     <h2 id="modal-title" class="text-2xl font-bold">Thêm nhân viên mới</h2><button id="close-modal" class="text-3xl hover:opacity-70 transition">×</button>
    </div>
    <form id="employee-form">
     <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
      <div><label for="full_name" class="block text-sm font-semibold mb-2">Họ và tên *</label> <input type="text" id="full_name" name="full_name" required class="input-field" placeholder="Nguyễn Văn A">
      </div>
      <div><label for="employee_code" class="block text-sm font-semibold mb-2">Mã nhân viên *</label> <input type="text" id="employee_code" name="employee_code" required class="input-field" placeholder="NV001">
      </div>
     </div>
     <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
      <div><label for="email" class="block text-sm font-semibold mb-2">Email *</label> <input type="email" id="email" name="email" required class="input-field" placeholder="email@company.com">
      </div>
      <div><label for="phone" class="block text-sm font-semibold mb-2">Số điện thoại *</label> <input type="tel" id="phone" name="phone" required class="input-field" placeholder="0123456789">
      </div>
     </div>
     <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
      <div><label for="department" class="block text-sm font-semibold mb-2">Phòng ban *</label> <select id="department" name="department" required class="input-field"> <option value="">Chọn phòng ban</option> <option value="Công nghệ">Công nghệ</option> <option value="Nhân sự">Nhân sự</option> <option value="Marketing">Marketing</option> <option value="Tài chính">Tài chính</option> <option value="Kinh doanh">Kinh doanh</option> </select>
      </div>
      <div><label for="position" class="block text-sm font-semibold mb-2">Chức vụ *</label> <input type="text" id="position" name="position" required class="input-field" placeholder="Developer, Manager...">
      </div>
     </div>
     <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
      <div><label for="status" class="block text-sm font-semibold mb-2">Trạng thái *</label> <select id="status" name="status" required class="input-field"> <option value="Đang làm việc">Đang làm việc</option> <option value="Nghỉ phép">Nghỉ phép</option> <option value="Thử việc">Thử việc</option> </select>
      </div>
      <div><label for="join_date" class="block text-sm font-semibold mb-2">Ngày vào làm *</label> <input type="date" id="join_date" name="join_date" required class="input-field">
      </div>
     </div>
     <div class="flex gap-3 justify-end"><button type="button" id="cancel-btn" class="btn"> Hủy </button> <button type="submit" id="submit-btn" class="btn"> 💾 Lưu nhân viên </button>
     </div>
    </form>
   </div>
  </div><!-- Delete Confirmation Modal -->
  <div id="delete-modal" class="modal">
   <div class="modal-content rounded-xl shadow-2xl p-6" style="max-width: 400px;">
    <div class="text-center mb-6">
     <div class="text-6xl mb-4">
      ⚠️
     </div>
     <h2 class="text-2xl font-bold mb-2">Xác nhận xóa</h2>
     <p class="opacity-75" id="delete-employee-name"></p>
    </div>
    <div class="flex gap-3 justify-center"><button id="cancel-delete-btn" class="btn"> Hủy </button> <button id="confirm-delete-btn" class="btn"> 🗑️ Xóa </button>
    </div>
   </div>
  </div>
  <script>
    const defaultConfig = {
      background_color: '#f3f4f6',
      table_color: '#ffffff',
      primary_color: '#4f46e5',
      text_color: '#1f2937',
      danger_color: '#dc2626',
      page_title: 'Quản lý nhân viên',
      add_button_text: 'Thêm nhân viên',
      font_family: 'Segoe UI',
      font_size: 14
    };

    let employees = [];
    let currentEditEmployee = null;
    let currentDeleteEmployee = null;

    const dataHandler = {
      onDataChanged(data) {
        employees = data;
        renderEmployees();
        updateEmployeeCount();
      }
    };

    async function initApp() {
      const result = await window.dataSdk.init(dataHandler);
      if (!result.isOk) {
        console.error('Failed to initialize Data SDK');
      }
    }

    function renderEmployees() {
      const tbody = document.getElementById('employee-table-body');
      const emptyState = document.getElementById('empty-state');
      const searchTerm = document.getElementById('search-input').value.toLowerCase();
      const departmentFilter = document.getElementById('department-filter').value;
      const statusFilter = document.getElementById('status-filter').value;

      let filteredEmployees = employees.filter(emp => {
        const matchesSearch = emp.full_name.toLowerCase().includes(searchTerm) ||
                            emp.employee_code.toLowerCase().includes(searchTerm) ||
                            emp.email.toLowerCase().includes(searchTerm) ||
                            emp.phone.toLowerCase().includes(searchTerm);
        const matchesDepartment = !departmentFilter || emp.department === departmentFilter;
        const matchesStatus = !statusFilter || emp.status === statusFilter;
        return matchesSearch && matchesDepartment && matchesStatus;
      });

      if (filteredEmployees.length === 0) {
        tbody.innerHTML = '';
        emptyState.classList.remove('hidden');
      } else {
        emptyState.classList.add('hidden');
        tbody.innerHTML = filteredEmployees.map(employee => createEmployeeRow(employee)).join('');
        
        // Attach event listeners
        filteredEmployees.forEach(employee => {
          const row = document.querySelector(`[data-employee-id="${employee.__backendId}"]`);
          if (row) {
            row.querySelector('.edit-btn').addEventListener('click', () => openEditModal(employee));
            row.querySelector('.delete-btn').addEventListener('click', () => openDeleteModal(employee));
          }
        });
      }
    }

    function createEmployeeRow(employee) {
      const statusColors = {
        'Đang làm việc': 'background-color: #d1fae5; color: #065f46;',
        'Nghỉ phép': 'background-color: #fed7aa; color: #92400e;',
        'Thử việc': 'background-color: #dbeafe; color: #1e40af;'
      };

      return `
        <tr class="table-row border-b" data-employee-id="${employee.__backendId}">
          <td class="p-4 font-semibold">${employee.employee_code}</td>
          <td class="p-4 font-medium">${employee.full_name}</td>
          <td class="p-4">${employee.email}</td>
          <td class="p-4">${employee.phone}</td>
          <td class="p-4">${employee.department}</td>
          <td class="p-4">${employee.position}</td>
          <td class="p-4">${formatDate(employee.join_date)}</td>
          <td class="p-4">
            <span class="status-badge" style="${statusColors[employee.status] || statusColors['Đang làm việc']}">${employee.status}</span>
          </td>
          <td class="p-4">
            <div class="flex gap-2 justify-center">
              <button class="edit-btn action-btn">✏️ Sửa</button>
              <button class="delete-btn action-btn">🗑️ Xóa</button>
            </div>
          </td>
        </tr>
      `;
    }

    function formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleDateString('vi-VN');
    }

    function updateEmployeeCount() {
      const countEl = document.getElementById('employee-count');
      countEl.textContent = `Tổng số: ${employees.length} nhân viên`;
    }

    function openAddModal() {
      currentEditEmployee = null;
      document.getElementById('modal-title').textContent = 'Thêm nhân viên mới';
      document.getElementById('employee-form').reset();
      document.getElementById('employee-modal').classList.add('active');
    }

    function openEditModal(employee) {
      currentEditEmployee = employee;
      document.getElementById('modal-title').textContent = 'Chỉnh sửa nhân viên';
      
      document.getElementById('full_name').value = employee.full_name;
      document.getElementById('employee_code').value = employee.employee_code;
      document.getElementById('email').value = employee.email;
      document.getElementById('phone').value = employee.phone;
      document.getElementById('department').value = employee.department;
      document.getElementById('position').value = employee.position;
      document.getElementById('status').value = employee.status;
      document.getElementById('join_date').value = employee.join_date;
      
      document.getElementById('employee-modal').classList.add('active');
    }

    function closeEmployeeModal() {
      document.getElementById('employee-modal').classList.remove('active');
      currentEditEmployee = null;
    }

    function openDeleteModal(employee) {
      currentDeleteEmployee = employee;
      document.getElementById('delete-employee-name').textContent = `Bạn có chắc chắn muốn xóa nhân viên "${employee.full_name}"?`;
      document.getElementById('delete-modal').classList.add('active');
    }

    function closeDeleteModal() {
      document.getElementById('delete-modal').classList.remove('active');
      currentDeleteEmployee = null;
    }

    async function handleFormSubmit(e) {
      e.preventDefault();
      
      if (employees.length >= 999 && !currentEditEmployee) {
        showNotification('⚠️ Đã đạt giới hạn 999 nhân viên. Vui lòng xóa bớt nhân viên trước khi thêm mới.');
        return;
      }

      const submitBtn = document.getElementById('submit-btn');
      submitBtn.disabled = true;
      submitBtn.textContent = '⏳ Đang lưu...';

      const formData = {
        id: currentEditEmployee ? currentEditEmployee.id : 'emp_' + Date.now(),
        full_name: document.getElementById('full_name').value,
        employee_code: document.getElementById('employee_code').value,
        email: document.getElementById('email').value,
        phone: document.getElementById('phone').value,
        department: document.getElementById('department').value,
        position: document.getElementById('position').value,
        status: document.getElementById('status').value,
        join_date: document.getElementById('join_date').value
      };

      let result;
      if (currentEditEmployee) {
        result = await window.dataSdk.update({ ...formData, __backendId: currentEditEmployee.__backendId });
      } else {
        result = await window.dataSdk.create(formData);
      }

      submitBtn.disabled = false;
      submitBtn.textContent = '💾 Lưu nhân viên';

      if (result.isOk) {
        closeEmployeeModal();
        showNotification(currentEditEmployee ? '✅ Cập nhật thành công!' : '✅ Thêm nhân viên thành công!');
      } else {
        showNotification('❌ Có lỗi xảy ra. Vui lòng thử lại.');
      }
    }

    async function handleDelete() {
      if (!currentDeleteEmployee) return;

      const confirmBtn = document.getElementById('confirm-delete-btn');
      confirmBtn.disabled = true;
      confirmBtn.textContent = '⏳ Đang xóa...';

      const result = await window.dataSdk.delete(currentDeleteEmployee);

      confirmBtn.disabled = false;
      confirmBtn.textContent = '🗑️ Xóa';

      if (result.isOk) {
        closeDeleteModal();
        showNotification('✅ Xóa nhân viên thành công!');
      } else {
        showNotification('❌ Có lỗi xảy ra. Vui lòng thử lại.');
      }
    }

    function showNotification(message) {
      const notification = document.createElement('div');
      notification.textContent = message;
      notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 16px 24px;
        border-radius: 8px;
        background-color: #1f2937;
        color: white;
        font-weight: 600;
        box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2);
        z-index: 2000;
        animation: slideInRight 0.3s;
      `;
      document.body.appendChild(notification);
      setTimeout(() => {
        notification.style.animation = 'slideOutRight 0.3s';
        setTimeout(() => notification.remove(), 300);
      }, 3000);
    }

    // Event Listeners
    document.getElementById('add-employee-btn').addEventListener('click', openAddModal);
    document.getElementById('empty-add-btn').addEventListener('click', openAddModal);
    document.getElementById('close-modal').addEventListener('click', closeEmployeeModal);
    document.getElementById('cancel-btn').addEventListener('click', closeEmployeeModal);
    document.getElementById('employee-form').addEventListener('submit', handleFormSubmit);
    document.getElementById('search-input').addEventListener('input', renderEmployees);
    document.getElementById('department-filter').addEventListener('change', renderEmployees);
    document.getElementById('status-filter').addEventListener('change', renderEmployees);
    document.getElementById('cancel-delete-btn').addEventListener('click', closeDeleteModal);
    document.getElementById('confirm-delete-btn').addEventListener('click', handleDelete);

    // Element SDK Configuration
    async function onConfigChange(config) {
      const backgroundColor = config.background_color || defaultConfig.background_color;
      const tableColor = config.table_color || defaultConfig.table_color;
      const primaryColor = config.primary_color || defaultConfig.primary_color;
      const textColor = config.text_color || defaultConfig.text_color;
      const dangerColor = config.danger_color || defaultConfig.danger_color;
      const fontFamily = config.font_family || defaultConfig.font_family;
      const fontSize = config.font_size || defaultConfig.font_size;

      document.body.style.backgroundColor = backgroundColor;
      document.body.style.color = textColor;
      document.body.style.fontFamily = `${fontFamily}, 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif`;
      document.body.style.fontSize = `${fontSize}px`;

      document.getElementById('page-title').textContent = config.page_title || defaultConfig.page_title;
      document.getElementById('page-title').style.fontSize = `${fontSize * 2.14}px`;
      document.getElementById('page-title').style.color = textColor;

      document.getElementById('add-button-text').textContent = config.add_button_text || defaultConfig.add_button_text;
      
      const addBtn = document.getElementById('add-employee-btn');
      addBtn.style.backgroundColor = primaryColor;
      addBtn.style.color = '#ffffff';

      const emptyAddBtn = document.getElementById('empty-add-btn');
      emptyAddBtn.style.backgroundColor = primaryColor;
      emptyAddBtn.style.color = '#ffffff';

      const table = document.querySelector('table');
      if (table) {
        table.style.backgroundColor = tableColor;
        table.style.color = textColor;
      }

      const thead = document.querySelector('thead tr');
      if (thead) {
        thead.style.backgroundColor = primaryColor;
        thead.style.color = '#ffffff';
      }

      const modalContents = document.querySelectorAll('.modal-content');
      modalContents.forEach(modal => {
        modal.style.backgroundColor = tableColor;
        modal.style.color = textColor;
      });

      const editBtns = document.querySelectorAll('.edit-btn');
      editBtns.forEach(btn => {
        btn.style.backgroundColor = primaryColor;
        btn.style.color = '#ffffff';
      });

      const deleteBtns = document.querySelectorAll('.delete-btn');
      deleteBtns.forEach(btn => {
        btn.style.backgroundColor = dangerColor;
        btn.style.color = '#ffffff';
      });

      const submitBtn = document.getElementById('submit-btn');
      if (submitBtn) {
        submitBtn.style.backgroundColor = primaryColor;
        submitBtn.style.color = '#ffffff';
      }

      const confirmDeleteBtn = document.getElementById('confirm-delete-btn');
      if (confirmDeleteBtn) {
        confirmDeleteBtn.style.backgroundColor = dangerColor;
        confirmDeleteBtn.style.color = '#ffffff';
      }

      const cancelBtns = document.querySelectorAll('#cancel-btn, #cancel-delete-btn');
      cancelBtns.forEach(btn => {
        btn.style.backgroundColor = '#6b7280';
        btn.style.color = '#ffffff';
      });
    }

    function mapToCapabilities(config) {
      return {
        recolorables: [
          {
            get: () => config.background_color || defaultConfig.background_color,
            set: (value) => {
              config.background_color = value;
              if (window.elementSdk) {
                window.elementSdk.setConfig({ background_color: value });
              }
            }
          },
          {
            get: () => config.table_color || defaultConfig.table_color,
            set: (value) => {
              config.table_color = value;
              if (window.elementSdk) {
                window.elementSdk.setConfig({ table_color: value });
              }
            }
          },
          {
            get: () => config.text_color || defaultConfig.text_color,
            set: (value) => {
              config.text_color = value;
              if (window.elementSdk) {
                window.elementSdk.setConfig({ text_color: value });
              }
            }
          },
          {
            get: () => config.primary_color || defaultConfig.primary_color,
            set: (value) => {
              config.primary_color = value;
              if (window.elementSdk) {
                window.elementSdk.setConfig({ primary_color: value });
              }
            }
          },
          {
            get: () => config.danger_color || defaultConfig.danger_color,
            set: (value) => {
              config.danger_color = value;
              if (window.elementSdk) {
                window.elementSdk.setConfig({ danger_color: value });
              }
            }
          }
        ],
        borderables: [],
        fontEditable: {
          get: () => config.font_family || defaultConfig.font_family,
          set: (value) => {
            config.font_family = value;
            if (window.elementSdk) {
              window.elementSdk.setConfig({ font_family: value });
            }
          }
        },
        fontSizeable: {
          get: () => config.font_size || defaultConfig.font_size,
          set: (value) => {
            config.font_size = value;
            if (window.elementSdk) {
              window.elementSdk.setConfig({ font_size: value });
            }
          }
        }
      };
    }

    function mapToEditPanelValues(config) {
      return new Map([
        ['page_title', config.page_title || defaultConfig.page_title],
        ['add_button_text', config.add_button_text || defaultConfig.add_button_text]
      ]);
    }

    if (window.elementSdk) {
      window.elementSdk.init({
        defaultConfig,
        onConfigChange,
        mapToCapabilities,
        mapToEditPanelValues
      });
    }

    // Initialize
    onConfigChange(defaultConfig);
    initApp();
  </script>
 <script>(function () {
         function c() {
             var b = a.contentDocument || a.contentWindow.document;
             if (b) { var d = b.createElement('script'); d.innerHTML = "window.__CF$cv$params={r:'9a1e9af4a2da06ff',t:'MTc2MzcxMTUyMi4wMDAwMDA='};var a=document.createElement('script');a.nonce='';a.src='/cdn-cgi/challenge-platform/scripts/jsd/main.js';document.getElementsByTagName('head')[0].appendChild(a);"; b.getElementsByTagName('head')[0].appendChild(d) }
    } if (document.body) {
        var a = document.createElement('iframe');
        a.height = 1; a.width = 1; a.style.position = 'absolute'; a.style.top = 0; a.style.left = 0; a.style.border = 'none'; a.style.visibility = 'hidden'; document.body.appendChild(a);
        if ('loading' !== document.readyState) c();
        else if (window.addEventListener) document.addEventListener('DOMContentLoaded', c);
        else { var e = document.onreadystatechange || function () { }; document.onreadystatechange = function (b) { e(b); 'loading' !== document.readyState && (document.onreadystatechange = e, c()) } }
    }
     })();</script>

 </body>
</html>
