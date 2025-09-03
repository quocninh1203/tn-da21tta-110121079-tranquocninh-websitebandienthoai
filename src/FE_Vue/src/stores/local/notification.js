import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useNotificationStore = defineStore('notification', () => {
  const notifications = ref([]);

  const addNotification = (message, type = 'success') => {
    notifications.value.push({ message, type, id: Date.now() });

    // Xóa thông báo sau 3 giây
    setTimeout(() => {
      notifications.value.shift();
    }, 3000);
  };

  return {
    notifications,
    addNotification,
  };
});
