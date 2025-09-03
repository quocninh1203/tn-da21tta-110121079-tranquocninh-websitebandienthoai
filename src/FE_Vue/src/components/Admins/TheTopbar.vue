<template>
  <header class="topbar">
    <div class="topbar-title">Trang quản trị</div>
    <div class="topbar-user">
      <span>Xin chào, Admin</span>
      <button class="logout-button" @click="logout">Đăng xuất</button>
    </div>
  </header>
</template>

<script setup>
const logout = () => {
  // Thực hiện logout và chuyển hướng
  location.href = '/login'
}
</script>

<style scoped>
.topbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px 24px;
  background-color: #ffffff;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.08);
  position: sticky;
  top: 0;
  z-index: 40;
}

.topbar-title {
  font-size: 20px;
  font-weight: 600;
  color: #ff5722;
}

.topbar-user {
  display: flex;
  align-items: center;
  gap: 16px;
  color: #555;
  font-weight: 500;
}

.logout-button {
  background-color: transparent;
  color: #f44336;
  border: none;
  font-weight: 600;
  cursor: pointer;
  transition: color 0.2s ease;
}

.logout-button:hover {
  color: #c62828;
  text-decoration: underline;
}
</style>
