<template>
  <div class="admin-dashboard">
    <!-- Sidebar -->
    <TheSidebar />

    <!-- Khu vực bên phải -->
    <div class="admin-content">
      <!-- Topbar -->
      <TheTopbar />

      <!-- Nội dung chính -->
      <main class="admin-main">
        <router-view />
      </main>
    </div>
  </div>
</template>

<script setup>
import TheSidebar from '@/components/Admins/TheSidebar.vue'
import TheTopbar from '@/components/Admins/TheTopbar.vue'
</script>

<style scoped>
.admin-dashboard {
  display: flex;
  height: 100vh;
  background-color: #f3f4f6; /* nền xám nhạt */
}

/* Sidebar luôn bên trái */
.admin-dashboard > *:first-child {
  flex-shrink: 0;
}

/* Vùng bên phải */
.admin-content {
  display: flex;
  flex-direction: column;
  flex: 1;
  min-height: 100vh;
}

/* Topbar */
.admin-content > .topbar {
  height: 64px;
}

/* Main content */
.admin-main {
  flex: 1;
  padding: 24px;
  background-color: #f9fafb;
  overflow-y: auto;
}
</style>
