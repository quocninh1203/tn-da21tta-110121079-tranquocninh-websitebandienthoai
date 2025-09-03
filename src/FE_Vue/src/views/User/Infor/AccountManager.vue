<template>
  <TheMenu />
  <v-container fluid class="account-page">
    <v-row>
      <!-- Sidebar -->
      <v-col cols="12" md="3" class="sidebar-col">
        <v-card class="pa-4 sidebar-card">
          <v-list dense nav class="sidebar-list">
            <v-list-item
              v-for="(item, index) in menuItems"
              :key="index"
              :value="item.value"
              @click="navigate(item.value)"
              :active="isActive(item.value)"
              class="sidebar-item"
            >
              <p class="sidebar-label">{{ item.label }}</p>
            </v-list-item>
          </v-list>
        </v-card>
      </v-col>

      <!-- Nội dung -->
      <v-col cols="12" md="9" class="v-col-content">
        <v-card class="pa-6">
          <router-view />
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import TheMenu from '@/components/Global/TheMenu.vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/api/authStore'
import { computed } from 'vue'
import { getUserId } from '@/utils/jwtHelper'
import { useNotificationStore } from '@/stores/local/notification'

const authStore = useAuthStore()

const router = useRouter()
const route = useRoute()

const userId = computed(() => getUserId())

const menuItems = [
  { label: 'Thông tin cá nhân', value: 'profile' },
  { label: 'Đơn hàng', value: 'orders' },
  { label: 'Đăng xuất', value: 'logout' }
]

const logout = async () => {
  await authStore.logout(userId.value)
  if(authStore.success){
    setTimeout(() => {
      router.push('/') 
      useNotificationStore().addNotification(authStore.message, "success");
    }, 1000);
  }
}

const navigate = async (value) => {
  if (value === 'logout') {
    logout()
  } else {
    router.push(`/account/${value}`)
  }
}

const isActive = (value) => {
  return route.path === `/account/${value}`
}
</script>

<style scoped>
.account-page {
  margin-top: 0px;
  height: calc(100vh - 160px);
  overflow: hidden;
}

.v-row {
  height: 100%;
}

.v-col-content {
  height: 100%;
  overflow-y: auto;
}

/* Sidebar cột */
.sidebar-col {
  height: 100%;
}

/* Thẻ card của Sidebar */
.sidebar-card {
  height: 100%;
  background: linear-gradient(180deg, #ff5722 0%, #ff9800 100%);
  color: white;
  border-radius: 16px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
}

/* Danh sách trong Sidebar */
.sidebar-list {
  background-color: transparent;
}

/* Từng item */
.sidebar-item {
  color: white;
  border-radius: 8px;
  transition: background 0.2s ease;
}

/* Label trong item */
.sidebar-label {
  font-size: 20px;     /* Tăng kích thước chữ */
  font-weight: bold;   /* In đậm */
}

/* Khi hover */
.sidebar-item:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

/* Khi được chọn */
.sidebar-item.v-list-item--active {
  background-color: rgba(255, 255, 255, 0.25) !important;
}
</style>
