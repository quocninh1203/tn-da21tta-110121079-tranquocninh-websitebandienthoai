<template>
  <aside class="sidebar">
    <div class="sidebar-header">Admin Panel</div>
    <ul class="sidebar-menu">
      <li><router-link to="/admin/dashboard" class="sidebar-link">📊 Dashboard</router-link></li>
      <li><router-link to="/admin/products" class="sidebar-link">📦 Sản phẩm</router-link></li>
      <li><router-link to="/admin/orders" class="sidebar-link">🧾 Đơn hàng</router-link></li>
      <li><router-link to="/admin/categories" class="sidebar-link">🗂️ Danh mục</router-link></li>
      <li><router-link to="/admin/customers" class="sidebar-link">👤 Khách hàng</router-link></li>
      <li><router-link to="/admin/settings" class="sidebar-link">⚙️ Cài đặt</router-link></li>
    </ul>
  </aside>
</template>

<style scoped>
.sidebar {
  width: 250px;
  background-color: #ffffff;
  box-shadow: 2px 0 8px rgba(0, 0, 0, 0.06);
  min-height: 100vh;
  position: sticky;
  top: 0;
  left: 0;
  z-index: 50;
  display: flex;
  flex-direction: column;
}

.sidebar-header {
  padding: 24px;
  font-size: 22px;
  font-weight: bold;
  color: #ff5722;
  border-bottom: 1px solid #ffe0b2;
  text-align: center;
  background-color: #fff8f0;
}

.sidebar-menu {
  list-style: none;
  padding: 0;
  margin: 0;
  flex-grow: 1;
}

.sidebar-link {
  display: block;
  padding: 14px 20px;
  color: #333;
  font-weight: 500;
  text-decoration: none;
  transition: all 0.2s ease-in-out;
  border-left: 4px solid transparent;
}

.sidebar-link:hover {
  background-color: #fff3e0;
  color: #ff5722;
  border-left: 4px solid #ff5722;
}

.router-link-active {
  background-color: #ffccbc;
  color: #d84315;
  font-weight: bold;
  border-left: 4px solid #ff5722;
}
</style>
