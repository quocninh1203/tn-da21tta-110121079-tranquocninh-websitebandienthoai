<template>
  <div class="notification-container">
    <div
      v-for="notification in notifications"
      :key="notification.id"
      :class="`notification ${notification.type}`"
    >
    <v-icon v-if="notification.type == 'error'">mdi-close-circle</v-icon>
    <v-icon v-else>mdi-check-circle</v-icon> {{ notification.message }}
    </div>
  </div>
</template>

<script setup>
import { useNotificationStore } from '@/stores/local/notification';
import { computed } from 'vue';

const notificationStore = useNotificationStore();
const notifications = computed(() => notificationStore.notifications);
</script>

<style scoped>
.notification-container {
  position: fixed;
  top: 10px;
  left: 50%;
  transform: translateX(-50%);
  display: flex;
  flex-direction: column;
  gap: 10px;
  z-index: 99999;
}

.notification {
  padding: 10px 15px;
  border-radius: 5px;
  font-size: 14px;
  color: #fff;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
  animation: fadeInOut 3s ease;
}

.notification.success {
  background-color: #4caf50;
}

.notification.error {
  background-color: #f44336;
}

@keyframes fadeInOut {
  0%, 80% {
    opacity: 1;
  }
  100% {
    opacity: 0;
  }
}
</style>
