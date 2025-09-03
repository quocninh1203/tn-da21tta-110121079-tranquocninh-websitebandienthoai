<template>
  <v-container class="fill-height register-bg" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="5">
        <template v-if="!showOtp">
          <v-card class="pa-6 register-card">
            <v-card-title class="headline text-center">Đăng ký tài khoản</v-card-title>
            <v-card-text>
              <form
                @submit.prevent="onRegister"
                ref="form"
                class="custom-form"
              >
                <div class="form-row">
                  <div class="form-group half">
                    <label for="username">
                      <i class="mdi mdi-account-circle icon icon-orange"></i>
                      Tên đăng nhập
                    </label>
                    <input
                      id="username"
                      v-model="username"
                      type="text"
                      class="custom-input"
                      required
                      autocomplete="username"
                    />
                  </div>
                  <div class="form-group half">
                    <label for="fullName">
                      <i class="mdi mdi-account icon icon-blue"></i>
                      Họ và tên
                    </label>
                    <input
                      id="fullName"
                      v-model="fullName"
                      type="text"
                      class="custom-input"
                      required
                      autocomplete="name"
                    />
                  </div>
                </div>
                <div class="form-row">
                  <div class="form-group half">
                    <label for="email">
                      <i class="mdi mdi-email icon icon-cyan"></i>
                      Email
                    </label>
                    <input
                      id="email"
                      v-model="email"
                      type="email"
                      class="custom-input"
                      required
                      autocomplete="email"
                    />
                  </div>
                  <div class="form-group half">
                    <label for="phone">
                      <i class="mdi mdi-phone icon icon-green"></i>
                      Số điện thoại
                    </label>
                    <input
                      id="phone"
                      v-model="phone"
                      type="tel"
                      class="custom-input"
                      required
                      autocomplete="tel"
                    />
                  </div>
                </div>
                <div class="form-group">
                  <label for="address">
                    <i class="mdi mdi-map-marker icon icon-red"></i>
                    Địa chỉ
                  </label>
                  <input
                    id="address"
                    v-model="address"
                    type="text"
                    class="custom-input"
                    autocomplete="address"
                    placeholder="Không bắt buộc"
                  />
                </div>
                <div class="form-row">
                  <div class="form-group half">
                    <label for="password">
                      <i class="mdi mdi-lock icon icon-yellow"></i>
                      Mật khẩu
                    </label>
                    <input
                      id="password"
                      v-model="password"
                      type="password"
                      class="custom-input"
                      required
                      minlength="6"
                      autocomplete="new-password"
                    />
                  </div>
                  <div class="form-group half">
                    <label for="confirmPassword">
                      <i class="mdi mdi-lock-check icon icon-purple"></i>
                      Xác nhận mật khẩu
                    </label>
                    <input
                      id="confirmPassword"
                      v-model="confirmPassword"
                      type="password"
                      class="custom-input"
                      required
                      minlength="6"
                      autocomplete="new-password"
                    />
                  </div>
                </div>
                <button type="submit" class="register-btn">Đăng ký</button>
              </form>
              <v-alert v-if="error" type="error" class="mt-3" dense>{{ error }}</v-alert>
              <div class="login-link">
                <span>Bạn đã có tài khoản?</span>
                <button class="btn-login" @click="onLogin">Đăng nhập</button>
              </div>
            </v-card-text>
          </v-card>
        </template>
        <template v-else>
          <div class="otp-only-wrapper">
            <VerifyOtp :email="email" />
          </div>
        </template>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import VerifyOtp from '@/components/Register/VerifyOtp.vue'
import { useAuthStore } from '@/stores/api/authStore'
import { useNotificationStore } from '@/stores/local/notification'

const username = ref('')
const fullName = ref('')
const email = ref('')
const phone = ref('')
const address = ref('')
const password = ref('')
const confirmPassword = ref('')
const error = ref('')
const showOtp = ref(false)
const router = useRouter()
const authStore = useAuthStore()

const onRegister = async () => {
  error.value = ''
  if (
    !username.value ||
    !fullName.value ||
    !email.value ||
    !phone.value ||
    !password.value ||
    !confirmPassword.value
  ) {
    error.value = 'Vui lòng nhập đầy đủ thông tin bắt buộc.'
    return
  }
  if (password.value !== confirmPassword.value) {
    error.value = 'Mật khẩu xác nhận không khớp.'
    return
  }
  try {
    const res = await authStore.register({
      username: username.value,
      fullName: fullName.value,
      email: email.value,
      phoneNumber: phone.value,
      address: address.value,
      password: password.value
    })
    if (res && res.success === false) {
      useNotificationStore().addNotification(res.message, "error")
      return
    }
    await authStore.sendOtp({ email: email.value })
    showOtp.value = true
  } catch (err) {
    error.value = err?.response?.data?.message || 'Đăng ký thất bại. Vui lòng thử lại.'
  }
}

const onLogin = () => {
  router.push({ name: 'login' })
}
</script>

<style scoped>
@import url('https://cdn.jsdelivr.net/npm/@mdi/font@7.4.47/css/materialdesignicons.min.css');

.register-bg {
  background: linear-gradient(135deg, #ffe0b2 0%, #fff3e0 100%);
  min-height: 100vh;
}
.register-card {
  border-radius: 18px;
  box-shadow: 0 8px 32px 0 rgba(255, 87, 34, 0.10);
  background: #fff;
}
.v-card-title {
  font-weight: 700;
  color: #e65100;
}
.custom-form {
  display: flex;
  flex-direction: column;
  gap: 12px;
}
.form-row {
  display: flex;
  gap: 12px;
}
.form-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
  margin-bottom: 0;
}
.form-group.half {
  flex: 1 1 0;
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
.register-btn {
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
.register-btn:hover {
  background: linear-gradient(90deg, #e65100 60%, #ff5722 100%);
  box-shadow: 0 4px 16px 0 #ffccbc;
}
.icon {
  font-size: 20px;
  vertical-align: middle;
}
.icon-blue { color: #ff5722; }
.icon-orange { color: #ff9800; }
.icon-cyan { color: #ffb300; }
.icon-green { color: #ff7043; }
.icon-yellow { color: #ffd600; }
.icon-purple { color: #d84315; }
.icon-red { color: #e53935; }

/* Nút chuyển sang đăng nhập */
.login-link {
  margin-top: 24px;
  text-align: center;
  font-size: 15px;
}
.btn-login {
  background: none;
  border: none;
  color: #ff5722;
  font-weight: 600;
  margin-left: 6px;
  cursor: pointer;
  text-decoration: underline;
  transition: color 0.2s;
  padding: 0;
}
.btn-login:hover {
  color: #e65100;
}
.otp-only-wrapper {
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 420px;
}
</style>
