<template>
  <div class="login-bg">
    <div class="login-card">
      <form class="login-form" @submit.prevent="submitLogin">
        <div class="login-title">Đăng nhập</div>
        <div class="input-group">
          <label for="username">
            <i class="mdi mdi-account-circle icon icon-orange"></i>
            Tên đăng nhập
          </label>
          <input v-model="data.userName" id="username" type="text" class="custom-input" placeholder="Tên đăng nhập" required />
        </div>
        <div class="input-group">
          <label for="password">
            <i class="mdi mdi-lock icon icon-yellow"></i>
            Mật khẩu
          </label>
          <input v-model="data.passWord" id="password" type="password" class="custom-input" placeholder="Mật khẩu" required />
        </div>
        <div class="options">
          <label><input type="checkbox" /> Ghi nhớ đăng nhập</label>
          <a href="#">Quên mật khẩu?</a>
        </div>
        <button type="submit" class="login-btn">Đăng nhập</button>
        <div class="register-link">
          <span>Chưa có tài khoản?</span>
          <a href="/register">Đăng ký</a>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter, useRoute } from 'vue-router';

import { useAuthStore } from '@/stores/api/authStore';
import { useNotificationStore } from '@/stores/local/notification'; // Import store Pinia
  
const route = useRoute();
const router = useRouter();
const authStore = useAuthStore();
  
// const username = ref("");
// const password = ref("");
const data = ref({
  userName: ref(""),
  passWord: ref("")
})

const submitLogin = async () => {
  await authStore.login(data.value)
  
  if(authStore.success){
    setTimeout(() => {
      const redirect = route.query.redirect || '/';
      router.push(redirect); 
      
      useNotificationStore().addNotification(authStore.message, "success");
    }, 1000);

  }else {
    useNotificationStore().addNotification(authStore.message, "error");
  }
  
}
</script>  

<style scoped>
@import url('https://cdn.jsdelivr.net/npm/@mdi/font@7.4.47/css/materialdesignicons.min.css');

.login-bg {
  background: linear-gradient(135deg, #ffe0b2 0%, #fff3e0 100%);
  min-height: 100vh;
  width: 100vw;
  display: flex;
  align-items: center;
  justify-content: center;
}
.login-card {
  background: #fff;
  border-radius: 18px;
  box-shadow: 0 8px 32px 0 rgba(255, 87, 34, 0.10);
  padding: 36px 32px 28px 32px;
  max-width: 400px;
  width: 100%;
  margin: 40px 0;
  display: flex;
  flex-direction: column;
  align-items: stretch;
}
.login-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #e65100;
  text-align: center;
  margin-bottom: 24px;
}
.login-form {
  display: flex;
  flex-direction: column;
  gap: 18px;
}
.input-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}
label {
  font-weight: 500;
  color: #e65100;
  font-size: 15px;
  margin-bottom: 2px;
  display: flex;
  align-items: center;
  gap: 6px;
}
.custom-input {
  border: 1.5px solid #ffccbc;
  border-radius: 8px;
  padding: 12px 14px;
  font-size: 15px;
  background: #fff8f0;
  transition: border 0.2s, box-shadow 0.2s;
  outline: none;
  color: #222;
}
.custom-input:focus {
  border: 1.5px solid #ff5722;
  background: #ffe0b2;
  box-shadow: 0 2px 8px 0 #ffccbc;
}
.login-btn {
  font-weight: bold;
  font-size: 16px;
  letter-spacing: 1px;
  border: none;
  border-radius: 8px;
  background: linear-gradient(90deg, #ff5722 60%, #ff9800 100%);
  color: #fff;
  padding: 12px 0;
  margin-top: 10px;
  box-shadow: 0 2px 8px 0 #ffccbc;
  cursor: pointer;
  transition: background 0.2s, box-shadow 0.2s;
}
.login-btn:hover {
  background: linear-gradient(90deg, #e65100 60%, #ff5722 100%);
  box-shadow: 0 4px 16px 0 #ffccbc;
}
.icon {
  font-size: 20px;
  vertical-align: middle;
}
.icon-orange { color: #ff9800; }
.icon-yellow { color: #ffd600; }
.options {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 14px;
  margin-bottom: 0;
}
.options a {
  color: #ff5722;
  text-decoration: none;
  font-weight: 500;
}
.options a:hover {
  text-decoration: underline;
}
.register-link {
  margin-top: 18px;
  text-align: center;
  font-size: 15px;
}
.register-link a {
  color: #ff5722;
  font-weight: 600;
  margin-left: 6px;
  text-decoration: underline;
  transition: color 0.2s;
}
.register-link a:hover {
  color: #e65100;
}
</style>