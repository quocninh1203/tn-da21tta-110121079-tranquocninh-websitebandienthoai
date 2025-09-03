import './assets/main.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate';

import App from './App.vue';
import router from './router';

import 'vuetify/styles';
import { createVuetify } from 'vuetify';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';
import '@mdi/font/css/materialdesignicons.css';

// import { getAccessToken } from '@/utils/jwtHelper';
// import authService from '@/services/authService';
// import { useAuthStore } from '@/stores/api/authStore';

const app = createApp(App);
const vuetify = createVuetify({ components, directives });
const pinia = createPinia();
pinia.use(piniaPluginPersistedstate);

app.use(pinia);
app.use(router);
app.use(vuetify);

// ⚠️ Refresh accessToken nếu cần, trước khi mount app
(async () => {
  // const authStore = useAuthStore();
  // const accessToken = getAccessToken();

  // if (!accessToken) {
  //   try { 
  //     const res = await authService.refreshToken();
  //     const newAccessToken = res.data.model.accessToken;

  //     sessionStorage.setItem('token', newAccessToken);

  //     // Optionally: cập nhật trạng thái đăng nhập trong store
  //     authStore.setSuccess(true);
  //   } catch {
  //     //
  //     console.log("sai");
  //   }
  // }

  app.mount('#app');
})();
